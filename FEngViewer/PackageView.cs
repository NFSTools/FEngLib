using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using FEngLib;
using FEngLib.Data;
using FEngRender;
using JetBrains.Annotations;
using SixLabors.ImageSharp;

namespace FEngViewer
{
    public partial class PackageView : Form
    {
        public PackageView()
        {
            InitializeComponent();
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
            this.Text = package.Name;
            labelPkgName.Text = package.Name;
            var nodes = GeneratePackageHierarchy(package);
            PopulateTreeView(nodes);
            var renderer = new PackageRenderer(package, options.TextureDir);
            var image = renderer.Render();
            var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            viewOutput.Image = System.Drawing.Image.FromStream(stream);
        }

        private List<FEObjectViewNode> GeneratePackageHierarchy(FrontendPackage package)
        {
            var sorted = package.Objects.OrderBy(o => o.Parent?.Guid).ThenBy(o => o.Guid).ToList();
            var flatNodes = sorted.ConvertAll(obj => new FEObjectViewNode(obj));

            var nestedNodes = new List<FEObjectViewNode>();

            var groupLookup = new Dictionary<uint, FEObjectViewNode>();
            var groups = flatNodes.FindAll(node => node.Obj.Type == FEObjType.FE_Group);
            // LUT for GUID -> group
            foreach (var node in groups)
            {
                groupLookup.Add(node.Obj.Guid, node);
            }

            // directly add all objects that don't belong to any group
            var rootObjects = flatNodes.FindAll(node => node.Obj.Parent == null);
            nestedNodes.AddRange(rootObjects);
            flatNodes.RemoveAll(node => node.Obj.Parent == null);

            // only objects that belong to another object are left
            foreach (var node in flatNodes)
            {
                groupLookup[node.Obj.Parent.Guid].Children.Add(node);
            }

            return nestedNodes;
        }

        private void PopulateTreeView(IEnumerable<FEObjectViewNode> feObjectNodes)
        {
            // map group guid to children guids
            treeView1.BeginUpdate();

            ApplyObjectsToTreeNodes(feObjectNodes, treeView1.Nodes);

            treeView1.ExpandAll();
            treeView1.EndUpdate();
        }

        private static void ApplyObjectsToTreeNodes(IEnumerable<FEObjectViewNode> objectNodes,
            TreeNodeCollection treeNodes)
        {
            foreach (var feObjectNode in objectNodes)
            {
                var feObj = feObjectNode.Obj;
                var objTreeNode = treeNodes.Add($"{feObj.Type} {feObj.Guid:X}");
                objTreeNode.Tag = feObjectNode;
                if (feObjectNode.Children.Count > 0)
                {
                    ApplyObjectsToTreeNodes(feObjectNode.Children, objTreeNode.Nodes);
                }
            }
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

        [UsedImplicitly]
        private class Options
        {
            [Option('i', "input", Required = true)]
            public string InputFile { get; set; }

            [Option('t', "textures", Required = true)]
            public string TextureDir { get; set; }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateObjectDetails((FEObjectViewNode) e.Node.Tag);
        }

        private void UpdateObjectDetails(FEObjectViewNode nodeTag)
        {
            var obj = nodeTag.Obj;
            labelObjType.Text = obj.Type.ToString();
            labelObjHash.Text = $"{obj.NameHash:X}";
            labelObjGUID.Text = $"{obj.Guid:X}";
            labelObjFlags.Text = $"{obj.Flags:X}";
            labelObjResID.Text = obj.ResourceIndex.ToString();

            labelObjDataColor.Text = obj.Color.ToString();
            labelObjDataPivot.Text = obj.Pivot.ToString();
            labelObjDataPosition.Text = obj.Position.ToString();
            labelObjDataRotation.Text = obj.Rotation.ToString();
            labelObjDataSize.Text = obj.Size.ToString();
        }
    }
}