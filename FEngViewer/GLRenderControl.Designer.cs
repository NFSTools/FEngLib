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
            openglControl1 = new SharpGL.OpenGLControl();
            ((ISupportInitialize)openglControl1).BeginInit();
            SuspendLayout();
            // 
            // openglControl1
            // 
            openglControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            openglControl1.DrawFPS = false;
            openglControl1.Location = new System.Drawing.Point(0, 0);
            openglControl1.Margin = new System.Windows.Forms.Padding(4);
            openglControl1.Name = "openglControl1";
            openglControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            openglControl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
            openglControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            openglControl1.Size = new System.Drawing.Size(560, 360);
            openglControl1.TabIndex = 0;
            openglControl1.OpenGLInitialized += openglControl1_OpenGLInitialized;
            openglControl1.OpenGLDraw += openglControl1_OpenGLDraw;
            openglControl1.MouseClick += openglControl1_MouseClick;
            openglControl1.MouseMove += openglControl1_MouseMove;
            // 
            // GLRenderControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(openglControl1);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "GLRenderControl";
            Size = new System.Drawing.Size(560, 360);
            ((ISupportInitialize)openglControl1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SharpGL.OpenGLControl openglControl1;
    }
}