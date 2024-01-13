namespace FEngViewer
{
    partial class TrackEditorControl
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
            panel1 = new System.Windows.Forms.Panel();
            durationLabel = new System.Windows.Forms.Label();
            currentTimeLabel = new System.Windows.Forms.Label();
            scriptTrackBar = new System.Windows.Forms.TrackBar();
            statusLabel = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scriptTrackBar).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(statusLabel);
            panel1.Controls.Add(durationLabel);
            panel1.Controls.Add(currentTimeLabel);
            panel1.Controls.Add(scriptTrackBar);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(649, 150);
            panel1.TabIndex = 0;
            // 
            // durationLabel
            // 
            durationLabel.AutoSize = true;
            durationLabel.Location = new System.Drawing.Point(345, 24);
            durationLabel.Name = "durationLabel";
            durationLabel.Size = new System.Drawing.Size(55, 15);
            durationLabel.TabIndex = 3;
            durationLabel.Text = "09:59.999";
            // 
            // currentTimeLabel
            // 
            currentTimeLabel.AutoSize = true;
            currentTimeLabel.Location = new System.Drawing.Point(27, 24);
            currentTimeLabel.Name = "currentTimeLabel";
            currentTimeLabel.Size = new System.Drawing.Size(55, 15);
            currentTimeLabel.TabIndex = 2;
            currentTimeLabel.Text = "00:00.000";
            // 
            // scriptTrackBar
            // 
            scriptTrackBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            scriptTrackBar.Enabled = false;
            scriptTrackBar.LargeChange = 100;
            scriptTrackBar.Location = new System.Drawing.Point(82, 20);
            scriptTrackBar.Maximum = 1000;
            scriptTrackBar.Name = "scriptTrackBar";
            scriptTrackBar.Size = new System.Drawing.Size(263, 45);
            scriptTrackBar.SmallChange = 10;
            scriptTrackBar.TabIndex = 1;
            scriptTrackBar.TickFrequency = 100;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            statusLabel.Location = new System.Drawing.Point(27, 50);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(352, 15);
            statusLabel.TabIndex = 4;
            statusLabel.Text = "Click the \"Pause\" button in the toolbar to unlock the scrubber.";
            // 
            // TrackEditorControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            MinimumSize = new System.Drawing.Size(649, 150);
            Name = "TrackEditorControl";
            Size = new System.Drawing.Size(649, 150);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scriptTrackBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TrackBar scriptTrackBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label currentTimeLabel;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label statusLabel;
    }
}
