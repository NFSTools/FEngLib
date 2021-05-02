
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelObjResID = new System.Windows.Forms.Label();
            this.labelObjFlags = new System.Windows.Forms.Label();
            this.labelObjGUID = new System.Windows.Forms.Label();
            this.labelObjHash = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelObjType = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.viewOutput = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.labelPkgName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.viewOutput);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 781);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labelPkgName);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeView1);
            this.splitContainer2.Size = new System.Drawing.Size(340, 781);
            this.splitContainer2.SplitterDistance = 100;
            this.splitContainer2.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(340, 677);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelObjResID);
            this.groupBox1.Controls.Add(this.labelObjFlags);
            this.groupBox1.Controls.Add(this.labelObjGUID);
            this.groupBox1.Controls.Add(this.labelObjHash);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labelObjType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 491);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 287);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object details";
            // 
            // labelObjResID
            // 
            this.labelObjResID.AutoSize = true;
            this.labelObjResID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjResID.Location = new System.Drawing.Point(69, 113);
            this.labelObjResID.Name = "labelObjResID";
            this.labelObjResID.Size = new System.Drawing.Size(154, 23);
            this.labelObjResID.TabIndex = 11;
            this.labelObjResID.Text = "WWWWWWWWW";
            this.labelObjResID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjFlags
            // 
            this.labelObjFlags.AutoSize = true;
            this.labelObjFlags.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjFlags.Location = new System.Drawing.Point(69, 90);
            this.labelObjFlags.Name = "labelObjFlags";
            this.labelObjFlags.Size = new System.Drawing.Size(154, 23);
            this.labelObjFlags.TabIndex = 10;
            this.labelObjFlags.Text = "WWWWWWWWW";
            this.labelObjFlags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjGUID
            // 
            this.labelObjGUID.AutoSize = true;
            this.labelObjGUID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjGUID.Location = new System.Drawing.Point(69, 67);
            this.labelObjGUID.Name = "labelObjGUID";
            this.labelObjGUID.Size = new System.Drawing.Size(154, 23);
            this.labelObjGUID.TabIndex = 9;
            this.labelObjGUID.Text = "WWWWWWWWW";
            this.labelObjGUID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjHash
            // 
            this.labelObjHash.AutoSize = true;
            this.labelObjHash.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjHash.Location = new System.Drawing.Point(69, 44);
            this.labelObjHash.Name = "labelObjHash";
            this.labelObjHash.Size = new System.Drawing.Size(154, 23);
            this.labelObjHash.TabIndex = 8;
            this.labelObjHash.Text = "WWWWWWWWW";
            this.labelObjHash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(329, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 23);
            this.label6.TabIndex = 7;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjType
            // 
            this.labelObjType.AutoSize = true;
            this.labelObjType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjType.Location = new System.Drawing.Point(69, 23);
            this.labelObjType.Name = "labelObjType";
            this.labelObjType.Size = new System.Drawing.Size(154, 23);
            this.labelObjType.TabIndex = 6;
            this.labelObjType.Text = "WWWWWWWWW";
            this.labelObjType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(6, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "ResID:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(11, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Flags:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(8, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "GUID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(11, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Hash:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // 
            // labelPkgName
            // 
            this.labelPkgName.AutoSize = true;
            this.labelPkgName.Location = new System.Drawing.Point(22, 20);
            this.labelPkgName.Name = "labelPkgName";
            this.labelPkgName.Size = new System.Drawing.Size(95, 15);
            this.labelPkgName.TabIndex = 0;
            this.labelPkgName.Text = "WWWWWWWW";
            // 
            // PackageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 781);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PackageView";
            this.Text = "Form1";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PictureBox viewOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labelObjResID;
        private System.Windows.Forms.Label labelObjFlags;
        private System.Windows.Forms.Label labelObjGUID;
        private System.Windows.Forms.Label labelObjHash;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelObjType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPkgName;
    }
}