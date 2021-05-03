using System.ComponentModel;

namespace FEngViewer
{
    partial class ObjectDetailsView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelObjDataSize = new System.Windows.Forms.Label();
            this.labelObjDataRotation = new System.Windows.Forms.Label();
            this.labelObjDataPosition = new System.Windows.Forms.Label();
            this.labelObjDataPivot = new System.Windows.Forms.Label();
            this.labelObjDataColor = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
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
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(590, 297);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object details";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelObjDataSize);
            this.groupBox2.Controls.Add(this.labelObjDataRotation);
            this.groupBox2.Controls.Add(this.labelObjDataPosition);
            this.groupBox2.Controls.Add(this.labelObjDataPivot);
            this.groupBox2.Controls.Add(this.labelObjDataColor);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(5, 138);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(579, 154);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data";
            // 
            // labelObjDataSize
            // 
            this.labelObjDataSize.AutoSize = true;
            this.labelObjDataSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjDataSize.Location = new System.Drawing.Point(74, 101);
            this.labelObjDataSize.Name = "labelObjDataSize";
            this.labelObjDataSize.Size = new System.Drawing.Size(95, 15);
            this.labelObjDataSize.TabIndex = 9;
            this.labelObjDataSize.Text = "WWWWWWWW";
            this.labelObjDataSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjDataRotation
            // 
            this.labelObjDataRotation.AutoSize = true;
            this.labelObjDataRotation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjDataRotation.Location = new System.Drawing.Point(74, 80);
            this.labelObjDataRotation.Name = "labelObjDataRotation";
            this.labelObjDataRotation.Size = new System.Drawing.Size(95, 15);
            this.labelObjDataRotation.TabIndex = 8;
            this.labelObjDataRotation.Text = "WWWWWWWW";
            this.labelObjDataRotation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjDataPosition
            // 
            this.labelObjDataPosition.AutoSize = true;
            this.labelObjDataPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjDataPosition.Location = new System.Drawing.Point(74, 59);
            this.labelObjDataPosition.Name = "labelObjDataPosition";
            this.labelObjDataPosition.Size = new System.Drawing.Size(95, 15);
            this.labelObjDataPosition.TabIndex = 7;
            this.labelObjDataPosition.Text = "WWWWWWWW";
            this.labelObjDataPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjDataPivot
            // 
            this.labelObjDataPivot.AutoSize = true;
            this.labelObjDataPivot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjDataPivot.Location = new System.Drawing.Point(74, 38);
            this.labelObjDataPivot.Name = "labelObjDataPivot";
            this.labelObjDataPivot.Size = new System.Drawing.Size(95, 15);
            this.labelObjDataPivot.TabIndex = 6;
            this.labelObjDataPivot.Text = "WWWWWWWW";
            this.labelObjDataPivot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjDataColor
            // 
            this.labelObjDataColor.AutoSize = true;
            this.labelObjDataColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjDataColor.Location = new System.Drawing.Point(74, 17);
            this.labelObjDataColor.Name = "labelObjDataColor";
            this.labelObjDataColor.Size = new System.Drawing.Size(95, 15);
            this.labelObjDataColor.TabIndex = 5;
            this.labelObjDataColor.Text = "WWWWWWWW";
            this.labelObjDataColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(34, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 15);
            this.label11.TabIndex = 4;
            this.label11.Text = "Size:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(8, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 15);
            this.label10.TabIndex = 3;
            this.label10.Text = "Rotation:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(12, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Position:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(29, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "Pivot:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(26, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Color:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelObjResID
            // 
            this.labelObjResID.AutoSize = true;
            this.labelObjResID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjResID.Location = new System.Drawing.Point(60, 85);
            this.labelObjResID.Name = "labelObjResID";
            this.labelObjResID.Size = new System.Drawing.Size(116, 17);
            this.labelObjResID.TabIndex = 11;
            this.labelObjResID.Text = "WWWWWWWWW";
            this.labelObjResID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjFlags
            // 
            this.labelObjFlags.AutoSize = true;
            this.labelObjFlags.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjFlags.Location = new System.Drawing.Point(60, 68);
            this.labelObjFlags.Name = "labelObjFlags";
            this.labelObjFlags.Size = new System.Drawing.Size(116, 17);
            this.labelObjFlags.TabIndex = 10;
            this.labelObjFlags.Text = "WWWWWWWWW";
            this.labelObjFlags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjGUID
            // 
            this.labelObjGUID.AutoSize = true;
            this.labelObjGUID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjGUID.Location = new System.Drawing.Point(60, 50);
            this.labelObjGUID.Name = "labelObjGUID";
            this.labelObjGUID.Size = new System.Drawing.Size(116, 17);
            this.labelObjGUID.TabIndex = 9;
            this.labelObjGUID.Text = "WWWWWWWWW";
            this.labelObjGUID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjHash
            // 
            this.labelObjHash.AutoSize = true;
            this.labelObjHash.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjHash.Location = new System.Drawing.Point(60, 33);
            this.labelObjHash.Name = "labelObjHash";
            this.labelObjHash.Size = new System.Drawing.Size(116, 17);
            this.labelObjHash.TabIndex = 8;
            this.labelObjHash.Text = "WWWWWWWWW";
            this.labelObjHash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(288, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 17);
            this.label6.TabIndex = 7;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelObjType
            // 
            this.labelObjType.AutoSize = true;
            this.labelObjType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelObjType.Location = new System.Drawing.Point(60, 17);
            this.labelObjType.Name = "labelObjType";
            this.labelObjType.Size = new System.Drawing.Size(116, 17);
            this.labelObjType.TabIndex = 6;
            this.labelObjType.Text = "WWWWWWWWW";
            this.labelObjType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(5, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "ResID:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(10, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Flags:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(7, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "GUID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(10, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Hash:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectDetailsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ObjectDetailsView";
            this.Size = new System.Drawing.Size(590, 297);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelObjDataSize;
        private System.Windows.Forms.Label labelObjDataRotation;
        private System.Windows.Forms.Label labelObjDataPosition;
        private System.Windows.Forms.Label labelObjDataPivot;
        private System.Windows.Forms.Label labelObjDataColor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
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
    }
}