using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CommandLine;
using FEngLib;
using FEngLib.Data;
using FEngRender.Data;
using FEngViewer.Properties;
using JetBrains.Annotations;
using SharpGL;

namespace FEngViewer
{
    public partial class PackageView : Form
    {
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
            imageList.Images.Add("TreeItem_ScriptEvent", Resources.TreeItem_ScriptEvent);
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
            viewOutput.Render(_currentRenderTree);
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
                viewOutput.SelectedNode = viewNode;
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
            viewOutput.Init(Path.Combine(Path.GetDirectoryName(path) ?? "", "textures"));
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

        private float rotCube = 0;

        private void openglControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = this.openglControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            gl.Translate(1.5f, 0.0f, -4);				// Move Right And Into The Screen

            gl.Rotate(rotCube += 3, 1.0f, 1.0f, 1.0f);			// Rotate The Cube On X, Y & Z

            gl.Begin(OpenGL.GL_QUADS);					// Start Drawing The Cube

            gl.Color(0.0f, 1.0f, 0.0f);			// Set The Color To Green
            gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Top)
            gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Top)
            gl.Vertex(-1.0f, 1.0f, 1.0f);			// Bottom Left Of The Quad (Top)
            gl.Vertex(1.0f, 1.0f, 1.0f);			// Bottom Right Of The Quad (Top)


            gl.Color(1.0f, 0.5f, 0.0f);			// Set The Color To Orange
            gl.Vertex(1.0f, -1.0f, 1.0f);			// Top Right Of The Quad (Bottom)
            gl.Vertex(-1.0f, -1.0f, 1.0f);			// Top Left Of The Quad (Bottom)
            gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Bottom)
            gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Bottom)

            gl.Color(1.0f, 0.0f, 0.0f);			// Set The Color To Red
            gl.Vertex(1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Front)
            gl.Vertex(-1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Front)
            gl.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Front)
            gl.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Front)

            gl.Color(1.0f, 1.0f, 0.0f);			// Set The Color To Yellow
            gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Back)
            gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Back)
            gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Back)
            gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Back)

            gl.Color(0.0f, 0.0f, 1.0f);			// Set The Color To Blue
            gl.Vertex(-1.0f, 1.0f, 1.0f);			// Top Right Of The Quad (Left)
            gl.Vertex(-1.0f, 1.0f, -1.0f);			// Top Left Of The Quad (Left)
            gl.Vertex(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Quad (Left)
            gl.Vertex(-1.0f, -1.0f, 1.0f);			// Bottom Right Of The Quad (Left)

            gl.Color(1.0f, 0.0f, 1.0f);			// Set The Color To Violet
            gl.Vertex(1.0f, 1.0f, -1.0f);			// Top Right Of The Quad (Right)
            gl.Vertex(1.0f, 1.0f, 1.0f);			// Top Left Of The Quad (Right)
            gl.Vertex(1.0f, -1.0f, 1.0f);			// Bottom Left Of The Quad (Right)
            gl.Vertex(1.0f, -1.0f, -1.0f);			// Bottom Right Of The Quad (Right)
            gl.End();                       // Done Drawing The Q

            gl.Flush();

            //gl.Translate(-1.5f, 0.0f, -6.0f);				// Move Left And Into The Screen

            //gl.Rotate(0, 0.0f, 1.0f, 0.0f);				// Rotate The Pyramid On It's Y Axis

            //gl.Begin(OpenGL.GL_TRIANGLES);					// Start Drawing The Pyramid

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Front)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Left Of Triangle (Front)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Right Of Triangle (Front)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Right)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(1.0f, -1.0f, 1.0f);			// Left Of Triangle (Right)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Right Of Triangle (Right)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Back)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(1.0f, -1.0f, -1.0f);			// Left Of Triangle (Back)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Right Of Triangle (Back)

            //gl.Color(1.0f, 0.0f, 0.0f);			// Red
            //gl.Vertex(0.0f, 1.0f, 0.0f);			// Top Of Triangle (Left)
            //gl.Color(0.0f, 0.0f, 1.0f);			// Blue
            //gl.Vertex(-1.0f, -1.0f, -1.0f);			// Left Of Triangle (Left)
            //gl.Color(0.0f, 1.0f, 0.0f);			// Green
            //gl.Vertex(-1.0f, -1.0f, 1.0f);			// Right Of Triangle (Left)
            //gl.End();

            gl.Flush();
        }
    }
}