using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngRender.Data;
using FEngViewer.Properties;
using JetBrains.Annotations;

namespace FEngViewer;

public partial class PackageView : Form
{
    private RenderTree _currentRenderTree;
    private Package _currentPackage;

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
        imageList.Images.Add("TreeItem_ScriptEvent", Resources.TreeItem_ScriptEvent);
        imageList.Images.Add("TreeItem_Movie", Resources.TreeItem_Movie);
        imageList.Images.Add("TreeItem_ColoredImage", Resources.TreeItem_ColoredImage);
        imageList.Images.Add("TreeItem_MultiImage", Resources.TreeItem_MultiImage);
        imageList.Images.Add("TreeItem_ObjectList", Resources.TreeItem_ObjectList);
        imageList.Images.Add("TreeItem_ResourceList", Resources.TreeItem_ResourceList);
        imageList.Images.Add("TreeItem_GenericResource", Resources.TreeItem_GenericResource);
        imageList.Images.Add("TreeItem_Font", Resources.TreeItem_Font);
        imageList.Images.Add("TreeItem_Keyframe", Resources.TreeItem_Keyframe);
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
        viewOutput.Render(_currentRenderTree);
    }


    private void PopulateTreeView(Package package, IEnumerable<RenderTreeNode> feObjectNodes)
    {
        // map group guid to children guids
        treeView1.BeginUpdate();
        treeView1.Nodes.Clear();

        var rootNode = treeView1.Nodes.Add(package.Name);

        var resourceListNode = rootNode.Nodes.Add("Resources");
        resourceListNode.ImageKey = resourceListNode.SelectedImageKey = "TreeItem_ResourceList";

        foreach (var resourceRequest in package.ResourceRequests)
        {
            var resourceRequestNode = resourceListNode.Nodes.Add(resourceRequest.Name);

            resourceRequestNode.ImageKey = resourceRequestNode.SelectedImageKey = resourceRequest.Type switch
            {
                ResourceType.Image => "TreeItem_Image",
                ResourceType.MultiImage => "TreeItem_MultiImage",
                ResourceType.Movie => "TreeItem_Movie",
                ResourceType.Font => "TreeItem_Font",
                _ => "TreeItem_GenericResource"
            };
        }

        //var objectListNode = rootNode.Nodes.Add("Objects");
        //objectListNode.ImageKey = "TreeItem_ObjectList";
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
            ObjectType.String => "TreeItem_String",
            ObjectType.Image => "TreeItem_Image",
            ObjectType.Group => "TreeItem_Group",
            ObjectType.Movie => "TreeItem_Movie",
            ObjectType.ColoredImage => "TreeItem_ColoredImage",
            ObjectType.MultiImage => "TreeItem_MultiImage",
            _ => null
        };

        var nodeText = $"{feObj.Name ?? feObj.NameHash.ToString("X")}";

        if (nodeImageKey == null)
        {
            nodeText = feObj.Type + " " + nodeText;
        }

        var objTreeNode = collection.Add(nodeText);
        objTreeNode.Tag = viewNode;
        objTreeNode.Name = nodeText;
        if (nodeImageKey != null) objTreeNode.ImageKey = objTreeNode.SelectedImageKey = nodeImageKey;

        // Create nodes for scripts
        feObj.Scripts.ForEach(scr => CreateScriptTreeNode(objTreeNode.Nodes, scr));

        return objTreeNode;
    }

    private static void CreateScriptTreeNode(TreeNodeCollection collection, Script script)
    {
        var node = collection.Add(script.Name ?? $"0x{script.Id:X}");
        // ReSharper disable once LocalizableElement
        node.ImageKey = node.SelectedImageKey = "TreeItem_Script";
        node.Tag = script;

        foreach (var scriptEvent in script.Events)
        {
            var eventNode = node.Nodes.Add($"0x{scriptEvent.EventId:X} -> {scriptEvent.Target:X} @ T={scriptEvent.Time}");
            eventNode.ImageKey = eventNode.SelectedImageKey = "TreeItem_ScriptEvent";
        }

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

            foreach (var key in track.DeltaKeys)
            {
                var keyNode = trackNode.Nodes.Add($"{key.Val} @ T={key.Time}");
                keyNode.ImageKey = keyNode.SelectedImageKey = "TreeItem_Keyframe";
            }
        }
    }

    private static Package LoadPackageFromChunk(string path)
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
            viewOutput.SelectedNode = viewNode;
            objectPropertyGrid.SelectedObject = new ObjectViewWrapper(viewNode.FrontendObject);
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
        
    private void viewOutput_MouseClick(object sender, MouseEventArgs e)
    {
        bool WithinBounds(RenderTreeNode node, float x, float y)
        {
            var extents = node.Get2DExtents();

            return extents.HasValue && extents.Value.Contains((int) x, (int) y);
        }
        var feX = e.X - 320;
        var feY = e.Y - 240;
            
        // get highest Z rendertreenode with the click location in bounds
        var renderTree = RenderTree.GetAllTreeNodesForRendering(_currentRenderTree);
        try
        {
            var candidates = renderTree
                .Where(node => WithinBounds(node, feX, feY))
                //.OrderByDescending(n => n.GetZ());
                .OrderBy(node => // smallest area first => most "specific" candidate wins
                {
                    var sz = node.Get2DExtents().Value.Size;
                    return sz.Height * sz.Width; // area 
                });

            var top = candidates.First();

            var feObj = top.FrontendObject;
            var key = $"{feObj.Name ?? feObj.NameHash.ToString("X")}";
            var foundNodes = treeView1.Nodes.Find(key, true);
            treeView1.SelectedNode = foundNodes[0];
            treeView1.Focus();
        }
        catch (Exception)
        {
            // if linq stuff didn't find anything -
            // ignored
        }
    }

    [UsedImplicitly]
    private class Options
    {
        [Option('i', "input")]
        public string InputFile { get; [UsedImplicitly] set; }
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
        _currentPackage = package;
        _currentRenderTree = RenderTree.Create(package);
        PopulateTreeView(package, _currentRenderTree);
        viewOutput.Init(Path.Combine(Path.GetDirectoryName(path) ?? "", "textures"));
        Render();

        Text = package.Name;
    }

    private void treeView1_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        TreeNode hit_node = treeView1.GetNodeAt(e.X, e.Y);
        treeView1.SelectedNode = hit_node;

        var ctxPoint = new Point(e.X, e.Y);

        if (hit_node?.Tag is RenderTreeNode)
        {
            objectContextMenu.Show(treeView1, ctxPoint);
        }
        else if (hit_node?.Tag is Script script)
        {
            var viewNode = (RenderTreeNode)hit_node.Parent.Tag;

            toggleScriptItem.Text = ReferenceEquals(viewNode.CurrentScript, script) ? "Stop" : "Start";

            scriptContextMenu.Show(treeView1, ctxPoint);
        }
    }

    private void toggleObjectVisibilityItem_Click(object sender, EventArgs e)
    {
        if (treeView1.SelectedNode?.Tag is RenderTreeNode viewNode)
        {
            viewNode.FrontendObject.Flags ^= ObjectFlags.HideInEdit;

            Render();
        }
    }

    private void toggleScriptItem_Click(object sender, EventArgs e)
    {
        if (treeView1.SelectedNode?.Tag is Script script)
        {
            var viewNode = (RenderTreeNode)treeView1.SelectedNode.Parent.Tag;
            viewNode.SetScript(ReferenceEquals(viewNode.CurrentScript, script) ? null : script);
        }
    }

    private void radioBgBlack_CheckedChanged(object sender, EventArgs e)
    {
        if (sender == radioBgBlack)
        {
            viewOutput.BackgroundColor = new Color4(0, 0, 0, 255);
        }
        else if (sender == radioBgGreen)
        {
            viewOutput.BackgroundColor = new Color4(0, 255, 0, 255);
        }

        Render();
    }
}