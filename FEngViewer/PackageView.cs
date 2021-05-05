using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CommandLine;
using FEngLib;
using FEngLib.Data;
using FEngRender;
using FEngRender.Data;
using FEngViewer.Properties;
using JetBrains.Annotations;
using static SixLabors.ImageSharp.ImageExtensions;

namespace FEngViewer
{
    public partial class PackageView : Form
    {
        private RenderTreeRenderer _renderer;
        private RenderTree _currentRenderTree;

        public PackageView()
        {
            InitializeComponent();

            var imageList = new ImageList();
            imageList.Images.Add("TreeItem_Package", Resources.TreeItem_Package);
            imageList.Images.Add("TreeItem_String", Resources.TreeItem_String);
            imageList.Images.Add("TreeItem_Group", Resources.TreeItem_Group);
            imageList.Images.Add("TreeItem_Image", Resources.TreeItem_Image);
            imageList.Images.Add("TreeItem_Script", Resources.TreeItem_Script);
            imageList.Images.Add("TreeItem_ScriptTrack", Resources.TreeItem_ScriptTrack);
            imageList.Images.Add("TreeItem_Movie", Resources.TreeItem_Movie);
            imageList.Images.Add("TreeItem_ColoredImage", Resources.TreeItem_ColoredImage);
            imageList.Images.Add("TreeItem_MultiImage", Resources.TreeItem_MultiImage);
            treeView1.ImageList = imageList;
        }

        private void PackageView_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();

            var opts = Parser.Default.ParseArguments<Options>(args);
            
            opts.WithParsed(parsed => LoadFile(parsed.InputFile))
                .WithNotParsed(err => Application.Exit());
        }

        private void Render()
        {
            var image = _renderer.Render(_currentRenderTree);
            var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            viewOutput.Image = Image.FromStream(stream);
        }


        private void PopulateTreeView(FrontendPackage package, IEnumerable<RenderTreeNode> feObjectNodes)
        {
            // map group guid to children guids
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            var rootNode = treeView1.Nodes.Add(package.Name);
            ApplyObjectsToTreeNodes(feObjectNodes, rootNode.Nodes);

            rootNode.Expand();
            // treeView1.ExpandAll();
            treeView1.EndUpdate();

            treeView1.SelectedNode = rootNode;
        }

        private static void ApplyObjectsToTreeNodes(IEnumerable<RenderTreeNode> objectNodes,
            TreeNodeCollection treeNodes)
        {
            foreach (var feObjectNode in objectNodes)
            {
                var objTreeNode = CreateObjectTreeNode(treeNodes, feObjectNode);
                if (feObjectNode is RenderTreeGroup grp)
                    ApplyObjectsToTreeNodes(grp, objTreeNode.Nodes);
            }
        }

        private static TreeNode CreateObjectTreeNode(TreeNodeCollection collection, RenderTreeNode viewNode)
        {
            var feObj = viewNode.FrontendObject;
            var nodeImageKey = feObj.Type switch
            {
                FEObjType.FE_String => "TreeItem_String",
                FEObjType.FE_Image => "TreeItem_Image",
                FEObjType.FE_Group => "TreeItem_Group",
                FEObjType.FE_Movie => "TreeItem_Movie",
                FEObjType.FE_ColoredImage => "TreeItem_ColoredImage",
                FEObjType.FE_MultiImage => "TreeItem_MultiImage",
                _ => null
            };

            var nodeText = $"{feObj.Name ?? feObj.NameHash.ToString("X")}";

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
            if (e.Node.Tag is RenderTreeNode viewNode)
            {
                objectDetailsView1.Visible = true;
                objectDetailsView1.UpdateObjectDetails(viewNode);
                _renderer.SelectedNode = viewNode;
                //_renderer.SelectedObjectGuid = viewNode.FrontendObject.Guid;
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
            [Option('i', "input")]
            public string InputFile { get; set; }
        }

        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "FNG Files (*.fng)|*.fng|All files (*.*)|*.*";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadFile(ofd.FileName);
            }
        }

        private void LoadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            var package = LoadPackageFromChunk(path);
            // window title
            _currentRenderTree = RenderTree.Create(package);
            PopulateTreeView(package, _currentRenderTree);
            //_renderer = new PackageRenderer(package, Path.Combine(Path.GetDirectoryName(path) ?? "", "textures"));
            _renderer = new RenderTreeRenderer();
            _renderer.LoadTextures(Path.Combine(Path.GetDirectoryName(path) ?? "", "textures"));
            Render();

            Text = package.Name;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            TreeNode hit_node = treeView1.GetNodeAt(e.X, e.Y);
            treeView1.SelectedNode = hit_node;

            if (hit_node?.Tag is RenderTreeNode)
            {
                objectContextMenu.Show(treeView1, new Point(e.X, e.Y));
            }
        }

        private void toggleObjectVisibilityItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is RenderTreeNode viewNode)
            {
                var frontendObject = viewNode.FrontendObject;
                if ((frontendObject.Flags & FE_ObjectFlags.FF_Invisible) != 0)
                {
                    frontendObject.Flags &= ~FE_ObjectFlags.FF_Invisible;
                }
                else
                {
                    frontendObject.Flags |= FE_ObjectFlags.FF_Invisible;
                }

                Render();
            }
        }
    }
}