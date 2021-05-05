
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
            this.labelCoordDisplay = new System.Windows.Forms.Label();
            this.objectDetailsView1 = new FEngViewer.ObjectDetailsControl();
            this.viewOutput = new ImageSharpRenderControl();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpenFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleObjectVisibilityItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeObjectPositionItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeObjectColorItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.objectContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelCoordDisplay);
            this.splitContainer1.Panel2.Controls.Add(this.objectDetailsView1);
            this.splitContainer1.Panel2.Controls.Add(this.viewOutput);
            this.splitContainer1.Size = new System.Drawing.Size(937, 990);
            this.splitContainer1.SplitterDistance = 269;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(269, 990);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // labelCoordDisplay
            // 
            this.labelCoordDisplay.AutoSize = true;
            this.labelCoordDisplay.Location = new System.Drawing.Point(4, 487);
            this.labelCoordDisplay.Name = "labelCoordDisplay";
            this.labelCoordDisplay.Size = new System.Drawing.Size(72, 15);
            this.labelCoordDisplay.TabIndex = 2;
            this.labelCoordDisplay.Text = "X:    0   Y:    0";
            // 
            // objectDetailsView1
            // 
            this.objectDetailsView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectDetailsView1.Location = new System.Drawing.Point(3, 529);
            this.objectDetailsView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.objectDetailsView1.Name = "objectDetailsView1";
            this.objectDetailsView1.Size = new System.Drawing.Size(661, 457);
            this.objectDetailsView1.TabIndex = 1;
            // 
            // viewOutput
            // 
            this.viewOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewOutput.BackColor = System.Drawing.Color.Black;
            this.viewOutput.Location = new System.Drawing.Point(0, 0);
            this.viewOutput.MaximumSize = new System.Drawing.Size(640, 480);
            this.viewOutput.MinimumSize = new System.Drawing.Size(640, 480);
            this.viewOutput.Name = "viewOutput";
            this.viewOutput.Size = new System.Drawing.Size(640, 480);
            this.viewOutput.TabIndex = 0;
            this.viewOutput.TabStop = false;
            this.viewOutput.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewOutput_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(937, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpenFileMenuItem
            // 
            this.OpenFileMenuItem.Name = "OpenFileMenuItem";
            this.OpenFileMenuItem.Size = new System.Drawing.Size(48, 20);
            this.OpenFileMenuItem.Text = "Open";
            this.OpenFileMenuItem.Click += new System.EventHandler(this.OpenFileMenuItem_Click);
            // 
            // objectContextMenu
            // 
            this.objectContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleObjectVisibilityItem,
            this.changeObjectPositionItem,
            this.changeObjectColorItem});
            this.objectContextMenu.Name = "objectContextMenu";
            this.objectContextMenu.Size = new System.Drawing.Size(162, 70);
            // 
            // toggleObjectVisibilityItem
            // 
            this.toggleObjectVisibilityItem.Name = "toggleObjectVisibilityItem";
            this.toggleObjectVisibilityItem.Size = new System.Drawing.Size(161, 22);
            this.toggleObjectVisibilityItem.Text = "Toggle visibility";
            this.toggleObjectVisibilityItem.Click += new System.EventHandler(this.toggleObjectVisibilityItem_Click);
            // 
            // changeObjectPositionItem
            // 
            this.changeObjectPositionItem.Name = "changeObjectPositionItem";
            this.changeObjectPositionItem.Size = new System.Drawing.Size(161, 22);
            this.changeObjectPositionItem.Text = "Change position";
            // 
            // changeObjectColorItem
            // 
            this.changeObjectColorItem.Name = "changeObjectColorItem";
            this.changeObjectColorItem.Size = new System.Drawing.Size(161, 22);
            this.changeObjectColorItem.Text = "Change color";
            // 
            // PackageView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(937, 1017);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PackageView";
            this.Text = "FEngViewer";
            this.Load += new System.EventHandler(this.PackageView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.objectContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ImageSharpRenderControl viewOutput;
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
    }
}