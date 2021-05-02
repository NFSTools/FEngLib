using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using FEngLib;
using FEngLib.Data;
using FEngRender;
using FEngViewer.Properties;
using JetBrains.Annotations;
using SixLabors.ImageSharp;
using Image = System.Drawing.Image;

namespace FEngViewer
{
    public partial class PackageView : Form
    {
        private readonly ImageList _imageList;
        private PackageRenderer _renderer;

        public PackageView()
        {
            InitializeComponent();
            _imageList = new ImageList();
            _imageList.Images.Add("TreeItem_Package", Resources.TreeItem_Package);
            _imageList.Images.Add("TreeItem_String", Resources.TreeItem_String);
            _imageList.Images.Add("TreeItem_Group", Resources.TreeItem_Group);
            _imageList.Images.Add("TreeItem_Image", Resources.TreeItem_Image);
            _imageList.Images.Add("TreeItem_Script", Resources.TreeItem_Script);
            _imageList.Images.Add("TreeItem_ScriptTrack", Resources.TreeItem_ScriptTrack);
            treeView1.ImageList = _imageList;
        }

        private void PackageView_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();

            var opts = Parser.Default.ParseArguments<Options>(args);
            opts.WithNotParsed(errors => Application.Exit());
            var options = ((Parsed<Options>) opts).Value;

            if (!File.Exists(options.InputFile))
            {
                Console.Error.WriteLine("File not found: {0}", options.InputFile);
                Application.Exit();
            }

            var package = LoadPackageFromChunk(options.InputFile);
            // window title
            Text = package.Name;
            labelPkgName.Text = package.Name;
            var nodes = GeneratePackageHierarchy(package);
            PopulateTreeView(package, nodes);
            _renderer = new PackageRenderer(package, options.TextureDir);
            Render();
        }

        private void Render()
        {
            var image = _renderer.Render();
            var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            viewOutput.Image = Image.FromStream(stream);
        }

        private List<FEObjectViewNode> GeneratePackageHierarchy(FrontendPackage package)
        {
            var sorted = package.Objects.OrderBy(o => o.Parent?.Guid).ThenBy(o => o.Guid).ToList();
            var flatNodes = sorted.ConvertAll(obj => new FEObjectViewNode(obj));

            var nestedNodes = new List<FEObjectViewNode>();

            var groupLookup = new Dictionary<uint, FEObjectViewNode>();
            var groups = flatNodes.FindAll(node => node.Obj.Type == FEObjType.FE_Group);
            // LUT for GUID -> group
            foreach (var node in groups) groupLookup.Add(node.Obj.Guid, node);

            // directly add all objects that don't belong to any group
            var rootObjects = flatNodes.FindAll(node => node.Obj.Parent == null);
            nestedNodes.AddRange(rootObjects);
            flatNodes.RemoveAll(node => node.Obj.Parent == null);

            // only objects that belong to another object are left
            foreach (var node in flatNodes) groupLookup[node.Obj.Parent.Guid].Children.Add(node);

            return nestedNodes;
        }

        private void PopulateTreeView(FrontendPackage package, IEnumerable<FEObjectViewNode> feObjectNodes)
        {
            // map group guid to children guids
            treeView1.BeginUpdate();

            var rootNode = treeView1.Nodes.Add(package.Name);
            ApplyObjectsToTreeNodes(feObjectNodes, rootNode.Nodes);

            rootNode.Expand();
            // treeView1.ExpandAll();
            treeView1.EndUpdate();

            treeView1.SelectedNode = rootNode;
        }

        private static void ApplyObjectsToTreeNodes(IEnumerable<FEObjectViewNode> objectNodes,
            TreeNodeCollection treeNodes)
        {
            foreach (var feObjectNode in objectNodes)
            {
                var objTreeNode = CreateObjectTreeNode(treeNodes, feObjectNode);
                if (feObjectNode.Children.Count > 0) ApplyObjectsToTreeNodes(feObjectNode.Children, objTreeNode.Nodes);
            }
        }

        private static TreeNode CreateObjectTreeNode(TreeNodeCollection collection, FEObjectViewNode viewNode)
        {
            var feObj = viewNode.Obj;
            var nodeImageKey = feObj.Type switch
            {
                FEObjType.FE_String => "TreeItem_String",
                FEObjType.FE_Image => "TreeItem_Image",
                FEObjType.FE_Group => "TreeItem_Group",
                _ => null
            };

            var nodeText = $"{feObj.Name ?? feObj.Guid.ToString("X")}";

            if (nodeImageKey == null)
            {
                Debug.WriteLine("Encountered an object type that we don't have an icon for: " + feObj.Type);
                nodeText = feObj.Type + " " + nodeText;
            }

            var objTreeNode = collection.Add(nodeText);
            objTreeNode.Tag = viewNode;
            if (nodeImageKey != null) objTreeNode.ImageKey = objTreeNode.SelectedImageKey = nodeImageKey;

            // Create nodes for scripts
            feObj.Scripts.ForEach(scr => CreateScriptTreeNode(objTreeNode.Nodes, scr));

            return objTreeNode;
        }

        private static TreeNode CreateScriptTreeNode(TreeNodeCollection collection, FrontendScript script)
        {
            var node = collection.Add(script.Name ?? $"0x{script.Id:X}");
            // ReSharper disable once LocalizableElement
            node.ImageKey = node.SelectedImageKey = "TreeItem_Script";

            foreach (var track in script.Tracks)
            {
                var trackName = track.Offset switch
                {
                    /*
                     *                             {0, "FETrack_Color"},
                            {4, "FETrack_Pivot"},
                            {7, "FETrack_Position"},
                            {10, "FETrack_Rotation"},
                            {14, "FETrack_Size"},
                            {17, "FETrack_UpperLeft"},
                            {19, "FETrack_UpperRight"},
                            {21, "FETrack_FrameNumber OR FETrack_Color1"},
                            {25, "FETrack_Color2"},
                            {29, "FETrack_Color3"},
                            {33, "FETrack_Color4"}
                     */
                    0 => "Color",
                    4 => "Pivot",
                    7 => "Position",
                    10 => "Rotation",
                    14 => "Size",
                    17 => "UpperLeft",
                    19 => "UpperRight",
                    21 => "FrameNumber/Color1",
                    25 => "Color2",
                    29 => "Color3",
                    33 => "Color4",
                    _ => $"track-{track.Offset}"
                };

                var trackNode = node.Nodes.Add(trackName);
                trackNode.ImageKey = trackNode.SelectedImageKey = "TreeItem_ScriptTrack";
            }

            return node;
        }

        private static FrontendPackage LoadPackageFromChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var fr = new BinaryReader(fs);
            var marker = fr.ReadUInt32();
            switch (marker)
            {
                case 0x30203:
                    fs.Seek(0x10, SeekOrigin.Begin);
                    break;
                case 0xE76E4546:
                    fs.Seek(0x8, SeekOrigin.Begin);
                    break;
                default:
                    throw new InvalidDataException($"Invalid FEng chunk file: {path}");
            }

            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;

            using var mr = new BinaryReader(ms);
            return new FrontendPackageLoader().Load(mr);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is FEObjectViewNode viewNode)
            {
                objectDetailsView1.Visible = true;
                objectDetailsView1.UpdateObjectDetails(viewNode);
                _renderer.SelectedObjectGuid = viewNode.Obj.Guid;
                Render();
            }
            else
            {
                objectDetailsView1.Visible = false;
            }
        }

        private void viewOutput_MouseMove(object sender, MouseEventArgs e)
        {
            labelCoordDisplay.Text = $"FE: ({e.X - 320,4:D}, {e.Y - 240,4:D}) | Real: ({e.X,4:D}, {e.Y,4:D})";
        }

        [UsedImplicitly]
        private class Options
        {
            [Option('i', "input", Required = true)]
            public string InputFile { get; set; }

            [Option('t', "textures", Required = true)]
            public string TextureDir { get; set; }
        }
    }
}