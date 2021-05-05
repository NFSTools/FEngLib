
namespace FEngViewer
{
    partial class ImageSharpRenderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewOutput = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.viewOutput)).BeginInit();
            this.SuspendLayout();
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
            this.viewOutput.TabIndex = 1;
            this.viewOutput.TabStop = false;
            // 
            // ImageSharpRenderControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.viewOutput);
            this.Name = "ImageSharpRenderControl";
            this.Size = new System.Drawing.Size(640, 480);
            ((System.ComponentModel.ISupportInitialize)(this.viewOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox viewOutput;
    }
}
