
using System.Windows.Forms;

namespace FEngViewer
{
    partial class PackageView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.viewOutput = new FEngViewer.GLRenderControl();
            this.labelCoordDisplay = new System.Windows.Forms.Label();
            this.objectDetailsView1 = new FEngViewer.ObjectDetailsControl();
            this.groupBgColor = new System.Windows.Forms.GroupBox();
            this.radioBgGreen = new System.Windows.Forms.RadioButton();
            this.radioBgBlack = new System.Windows.Forms.RadioButton();
            this.objectPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpenFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleObjectVisibilityItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeObjectPositionItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeObjectColorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleScriptItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBgColor.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.objectContextMenu.SuspendLayout();
            this.scriptContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1499, 1054);
            this.splitContainer1.SplitterDistance = 429;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(429, 1054);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.viewOutput);
            this.splitContainer2.Panel1.Controls.Add(this.labelCoordDisplay);
            this.splitContainer2.Panel1.Controls.Add(this.objectDetailsView1);
            this.splitContainer2.Panel1.Controls.Add(this.groupBgColor);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectPropertyGrid);
            this.splitContainer2.Size = new System.Drawing.Size(1060, 1051);
            this.splitContainer2.SplitterDistance = 649;
            this.splitContainer2.TabIndex = 5;
            // 
            // viewOutput
            // 
            this.viewOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewOutput.BackColor = System.Drawing.Color.Black;
            this.viewOutput.Location = new System.Drawing.Point(3, 2);
            this.viewOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.viewOutput.MaximumSize = new System.Drawing.Size(640, 480);
            this.viewOutput.MinimumSize = new System.Drawing.Size(640, 480);
            this.viewOutput.Name = "viewOutput";
            this.viewOutput.SelectedNode = null;
            this.viewOutput.Size = new System.Drawing.Size(640, 480);
            this.viewOutput.TabIndex = 0;
            this.viewOutput.TabStop = false;
            this.viewOutput.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewOutput_MouseMove);
            this.viewOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.viewOutput_MouseClick);
            // 
            // labelCoordDisplay
            // 
            this.labelCoordDisplay.AutoSize = true;
            this.labelCoordDisplay.Location = new System.Drawing.Point(3, 484);
            this.labelCoordDisplay.Name = "labelCoordDisplay";
            this.labelCoordDisplay.Size = new System.Drawing.Size(92, 20);
            this.labelCoordDisplay.TabIndex = 2;
            this.labelCoordDisplay.Text = "X:    0   Y:    0";
            // 
            // objectDetailsView1
            // 
            this.objectDetailsView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectDetailsView1.Location = new System.Drawing.Point(3, 654);
            this.objectDetailsView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.objectDetailsView1.Name = "objectDetailsView1";
            this.objectDetailsView1.Size = new System.Drawing.Size(643, 393);
            this.objectDetailsView1.TabIndex = 1;
            // 
            // groupBgColor
            // 
            this.groupBgColor.Controls.Add(this.radioBgGreen);
            this.groupBgColor.Controls.Add(this.radioBgBlack);
            this.groupBgColor.Location = new System.Drawing.Point(489, 484);
            this.groupBgColor.Name = "groupBgColor";
            this.groupBgColor.Size = new System.Drawing.Size(154, 58);
            this.groupBgColor.TabIndex = 3;
            this.groupBgColor.TabStop = false;
            this.groupBgColor.Text = "Background";
            // 
            // radioBgGreen
            // 
            this.radioBgGreen.AutoSize = true;
            this.radioBgGreen.Location = new System.Drawing.Point(77, 26);
            this.radioBgGreen.Name = "radioBgGreen";
            this.radioBgGreen.Size = new System.Drawing.Size(69, 24);
            this.radioBgGreen.TabIndex = 1;
            this.radioBgGreen.Text = "Green";
            this.radioBgGreen.UseVisualStyleBackColor = true;
            this.radioBgGreen.CheckedChanged += new System.EventHandler(this.radioBgBlack_CheckedChanged);
            // 
            // radioBgBlack
            // 
            this.radioBgBlack.AutoSize = true;
            this.radioBgBlack.Checked = true;
            this.radioBgBlack.Location = new System.Drawing.Point(6, 26);
            this.radioBgBlack.Name = "radioBgBlack";
            this.radioBgBlack.Size = new System.Drawing.Size(65, 24);
            this.radioBgBlack.TabIndex = 0;
            this.radioBgBlack.TabStop = true;
            this.radioBgBlack.Text = "Black";
            this.radioBgBlack.UseVisualStyleBackColor = true;
            this.radioBgBlack.CheckedChanged += new System.EventHandler(this.radioBgBlack_CheckedChanged);
            // 
            // objectPropertyGrid
            // 
            this.objectPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.objectPropertyGrid.Name = "objectPropertyGrid";
            this.objectPropertyGrid.Size = new System.Drawing.Size(401, 1044);
            this.objectPropertyGrid.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1499, 28);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpenFileMenuItem
            // 
            this.OpenFileMenuItem.Name = "OpenFileMenuItem";
            this.OpenFileMenuItem.Size = new System.Drawing.Size(59, 24);
            this.OpenFileMenuItem.Text = "Open";
            this.OpenFileMenuItem.Click += new System.EventHandler(this.OpenFileMenuItem_Click);
            // 
            // objectContextMenu
            // 
            this.objectContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.objectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleObjectVisibilityItem,
            this.changeObjectPositionItem,
            this.changeObjectColorItem});
            this.objectContextMenu.Name = "objectContextMenu";
            this.objectContextMenu.Size = new System.Drawing.Size(187, 76);
            // 
            // toggleObjectVisibilityItem
            // 
            this.toggleObjectVisibilityItem.Name = "toggleObjectVisibilityItem";
            this.toggleObjectVisibilityItem.Size = new System.Drawing.Size(186, 24);
            this.toggleObjectVisibilityItem.Text = "Toggle visibility";
            this.toggleObjectVisibilityItem.Click += new System.EventHandler(this.toggleObjectVisibilityItem_Click);
            // 
            // changeObjectPositionItem
            // 
            this.changeObjectPositionItem.Name = "changeObjectPositionItem";
            this.changeObjectPositionItem.Size = new System.Drawing.Size(186, 24);
            this.changeObjectPositionItem.Text = "Change position";
            // 
            // changeObjectColorItem
            // 
            this.changeObjectColorItem.Name = "changeObjectColorItem";
            this.changeObjectColorItem.Size = new System.Drawing.Size(186, 24);
            this.changeObjectColorItem.Text = "Change color";
            // 
            // scriptContextMenu
            // 
            this.scriptContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.scriptContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleScriptItem});
            this.scriptContextMenu.Name = "scriptContextMenu";
            this.scriptContextMenu.Size = new System.Drawing.Size(212, 28);
            // 
            // toggleScriptItem
            // 
            this.toggleScriptItem.Name = "toggleScriptItem";
            this.toggleScriptItem.Size = new System.Drawing.Size(211, 24);
            this.toggleScriptItem.Text = "toolStripMenuItem1";
            this.toggleScriptItem.Click += new System.EventHandler(this.toggleScriptItem_Click);
            // 
            // PackageView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1499, 1152);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PackageView";
            this.Text = "FEngViewer";
            this.Load += new System.EventHandler(this.PackageView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBgColor.ResumeLayout(false);
            this.groupBgColor.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.objectContextMenu.ResumeLayout(false);
            this.scriptContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GLRenderControl viewOutput;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private ObjectDetailsControl objectDetailsView1;
        private System.Windows.Forms.Label labelCoordDisplay;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpenFileMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip objectContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toggleObjectVisibilityItem;
        private System.Windows.Forms.ToolStripMenuItem changeObjectPositionItem;
        private System.Windows.Forms.ToolStripMenuItem changeObjectColorItem;
        private System.Windows.Forms.ContextMenuStrip scriptContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toggleScriptItem;
        private System.Windows.Forms.GroupBox groupBgColor;
        private System.Windows.Forms.RadioButton radioBgGreen;
        private System.Windows.Forms.RadioButton radioBgBlack;
        private System.Windows.Forms.PropertyGrid objectPropertyGrid;
        private SplitContainer splitContainer2;
    }
}