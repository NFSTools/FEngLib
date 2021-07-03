using System.ComponentModel;

namespace FEngViewer
{
    partial class GLRenderControl
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
            this.openglControl1 = new SharpGL.OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.openglControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // openglControl1
            // 
            this.openglControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openglControl1.DrawFPS = false;
            this.openglControl1.Location = new System.Drawing.Point(0, 0);
            this.openglControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openglControl1.Name = "openglControl1";
            this.openglControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openglControl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openglControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openglControl1.Size = new System.Drawing.Size(640, 480);
            this.openglControl1.TabIndex = 0;
            this.openglControl1.OpenGLInitialized += new System.EventHandler(this.openglControl1_OpenGLInitialized);
            this.openglControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openglControl1_OpenGLDraw);
            this.openglControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openglControl1_MouseMove);
            // 
            // GLRenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openglControl1);
            this.Name = "GLRenderControl";
            this.Size = new System.Drawing.Size(640, 480);
            ((System.ComponentModel.ISupportInitialize)(this.openglControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SharpGL.OpenGLControl openglControl1;
    }
}