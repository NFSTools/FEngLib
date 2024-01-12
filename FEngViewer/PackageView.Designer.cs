
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
            components = new System.ComponentModel.Container();
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            splitContainer2 = new SplitContainer();
            viewOutput = new GLRenderControl();
            labelCoordDisplay = new Label();
            groupBgColor = new GroupBox();
            radioBgGreen = new RadioButton();
            radioBgBlack = new RadioButton();
            objectPropertyGrid = new PropertyGrid();
            colorDialog1 = new ColorDialog();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            OpenFileMenuItem = new ToolStripMenuItem();
            SaveFileMenuItem = new ToolStripMenuItem();
            objectContextMenu = new ContextMenuStrip(components);
            deleteToolStripMenuItem = new ToolStripMenuItem();
            scriptContextMenu = new ContextMenuStrip(components);
            toggleScriptItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            toolStripPausePlayButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            toolStripScriptSpeedCombox = new ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBgColor.SuspendLayout();
            menuStrip1.SuspendLayout();
            objectContextMenu.SuspendLayout();
            scriptContextMenu.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(0, 55);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(1499, 1060);
            splitContainer1.SplitterDistance = 429;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(429, 1060);
            treeView1.TabIndex = 1;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.MouseDown += treeView1_MouseDown;
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(viewOutput);
            splitContainer2.Panel1.Controls.Add(labelCoordDisplay);
            splitContainer2.Panel1.Controls.Add(groupBgColor);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(objectPropertyGrid);
            splitContainer2.Size = new System.Drawing.Size(1060, 1054);
            splitContainer2.SplitterDistance = 649;
            splitContainer2.TabIndex = 5;
            // 
            // viewOutput
            // 
            viewOutput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            viewOutput.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            viewOutput.BackColor = System.Drawing.Color.Black;
            viewOutput.Location = new System.Drawing.Point(3, 2);
            viewOutput.Margin = new Padding(3, 2, 3, 2);
            viewOutput.MaximumSize = new System.Drawing.Size(640, 480);
            viewOutput.MinimumSize = new System.Drawing.Size(640, 480);
            viewOutput.Name = "viewOutput";
            viewOutput.PlayEnabled = false;
            viewOutput.SelectedNode = null;
            viewOutput.Size = new System.Drawing.Size(640, 480);
            viewOutput.TabIndex = 0;
            viewOutput.TabStop = false;
            viewOutput.MouseClick += viewOutput_MouseClick;
            viewOutput.MouseMove += viewOutput_MouseMove;
            // 
            // labelCoordDisplay
            // 
            labelCoordDisplay.AutoSize = true;
            labelCoordDisplay.Location = new System.Drawing.Point(3, 484);
            labelCoordDisplay.Name = "labelCoordDisplay";
            labelCoordDisplay.Size = new System.Drawing.Size(72, 15);
            labelCoordDisplay.TabIndex = 2;
            labelCoordDisplay.Text = "X:    0   Y:    0";
            // 
            // groupBgColor
            // 
            groupBgColor.Controls.Add(radioBgGreen);
            groupBgColor.Controls.Add(radioBgBlack);
            groupBgColor.Location = new System.Drawing.Point(489, 484);
            groupBgColor.Name = "groupBgColor";
            groupBgColor.Size = new System.Drawing.Size(154, 58);
            groupBgColor.TabIndex = 3;
            groupBgColor.TabStop = false;
            groupBgColor.Text = "Background";
            // 
            // radioBgGreen
            // 
            radioBgGreen.AutoSize = true;
            radioBgGreen.Location = new System.Drawing.Point(77, 26);
            radioBgGreen.Name = "radioBgGreen";
            radioBgGreen.Size = new System.Drawing.Size(56, 19);
            radioBgGreen.TabIndex = 1;
            radioBgGreen.Text = "Green";
            radioBgGreen.UseVisualStyleBackColor = true;
            radioBgGreen.CheckedChanged += radioBgBlack_CheckedChanged;
            // 
            // radioBgBlack
            // 
            radioBgBlack.AutoSize = true;
            radioBgBlack.Checked = true;
            radioBgBlack.Location = new System.Drawing.Point(6, 26);
            radioBgBlack.Name = "radioBgBlack";
            radioBgBlack.Size = new System.Drawing.Size(53, 19);
            radioBgBlack.TabIndex = 0;
            radioBgBlack.TabStop = true;
            radioBgBlack.Text = "Black";
            radioBgBlack.UseVisualStyleBackColor = true;
            radioBgBlack.CheckedChanged += radioBgBlack_CheckedChanged;
            // 
            // objectPropertyGrid
            // 
            objectPropertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            objectPropertyGrid.Location = new System.Drawing.Point(3, 3);
            objectPropertyGrid.Name = "objectPropertyGrid";
            objectPropertyGrid.Size = new System.Drawing.Size(401, 1044);
            objectPropertyGrid.TabIndex = 4;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1499, 24);
            menuStrip1.TabIndex = 4;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenFileMenuItem, SaveFileMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // OpenFileMenuItem
            // 
            OpenFileMenuItem.Name = "OpenFileMenuItem";
            OpenFileMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            OpenFileMenuItem.Size = new System.Drawing.Size(180, 22);
            OpenFileMenuItem.Text = "Open";
            OpenFileMenuItem.Click += OpenFileMenuItem_Click;
            // 
            // SaveFileMenuItem
            // 
            SaveFileMenuItem.Name = "SaveFileMenuItem";
            SaveFileMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            SaveFileMenuItem.Size = new System.Drawing.Size(180, 22);
            SaveFileMenuItem.Text = "Save";
            SaveFileMenuItem.Click += SaveFileMenuItem_Click;
            // 
            // objectContextMenu
            // 
            objectContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            objectContextMenu.Items.AddRange(new ToolStripItem[] { deleteToolStripMenuItem });
            objectContextMenu.Name = "objectContextMenu";
            objectContextMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // scriptContextMenu
            // 
            scriptContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            scriptContextMenu.Items.AddRange(new ToolStripItem[] { toggleScriptItem });
            scriptContextMenu.Name = "scriptContextMenu";
            scriptContextMenu.Size = new System.Drawing.Size(181, 26);
            // 
            // toggleScriptItem
            // 
            toggleScriptItem.Name = "toggleScriptItem";
            toggleScriptItem.Size = new System.Drawing.Size(180, 22);
            toggleScriptItem.Text = "toolStripMenuItem1";
            toggleScriptItem.Click += toggleScriptItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripPausePlayButton, toolStripSeparator1, toolStripLabel1, toolStripScriptSpeedCombox });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1499, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripPausePlayButton
            // 
            toolStripPausePlayButton.Enabled = false;
            toolStripPausePlayButton.Image = Properties.Resources.Action_Pause;
            toolStripPausePlayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripPausePlayButton.Name = "toolStripPausePlayButton";
            toolStripPausePlayButton.Size = new System.Drawing.Size(58, 22);
            toolStripPausePlayButton.Text = "Pause";
            toolStripPausePlayButton.Click += toolStripPausePlayButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(39, 22);
            toolStripLabel1.Text = "Speed";
            // 
            // toolStripScriptSpeedCombox
            // 
            toolStripScriptSpeedCombox.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripScriptSpeedCombox.Name = "toolStripScriptSpeedCombox";
            toolStripScriptSpeedCombox.Size = new System.Drawing.Size(121, 25);
            // PackageView
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = System.Drawing.SystemColors.ControlLight;
            ClientSize = new System.Drawing.Size(1499, 1152);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(splitContainer1);
            Name = "PackageView";
            Text = "FEngViewer";
            Load += PackageView_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBgColor.ResumeLayout(false);
            groupBgColor.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            objectContextMenu.ResumeLayout(false);
            scriptContextMenu.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GLRenderControl viewOutput;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label labelCoordDisplay;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip objectContextMenu;
        private System.Windows.Forms.ContextMenuStrip scriptContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toggleScriptItem;
        private System.Windows.Forms.GroupBox groupBgColor;
        private System.Windows.Forms.RadioButton radioBgGreen;
        private System.Windows.Forms.RadioButton radioBgBlack;
        private System.Windows.Forms.PropertyGrid objectPropertyGrid;
        private SplitContainer splitContainer2;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem OpenFileMenuItem;
        private ToolStripMenuItem SaveFileMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripPausePlayButton;
        private ToolStripComboBox toolStripScriptSpeedCombox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel1;
    }
}