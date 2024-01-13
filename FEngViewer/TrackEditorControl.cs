using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FEngLib.Scripts;
using FEngRender.Data;

namespace FEngViewer
{
    public delegate void TimeChanged(int NewTime);

    public partial class TrackEditorControl : UserControl
    {
        public event TimeChanged OnTimeChanged;

        public RenderTreeNode SelectedNode { get; set; }

        private bool _suppressEventGeneration;

        public TrackEditorControl()
        {
            InitializeComponent();
            scriptTrackBar.ValueChanged += ScriptTrackBar_OnValueChanged;
            UpdateTimeLabels();
        }

        public void UpdateData()
        {
            if (SelectedNode?.GetCurrentScript() is { } currentScript)
            {
                _suppressEventGeneration = true;
                {
                    scriptTrackBar.Minimum = 0;
                    scriptTrackBar.Maximum = (int)currentScript.Length + 1;
                    scriptTrackBar.Value = Math.Max(0, SelectedNode.GetScriptTime());
                    UpdateTimeLabels();
                }
                _suppressEventGeneration = false;
                Visible = true;
                scriptTrackBar.Enabled = !AppService.Instance.PlaybackEnabled;
                statusLabel.Visible = AppService.Instance.PlaybackEnabled;
            }
            else
            {
                Visible = false;
                scriptTrackBar.Enabled = false;
            }
        }

        private void ScriptTrackBar_OnValueChanged(object sender, EventArgs e)
        {
            if (sender is not TrackBar trackBar)
                return;
            if (!_suppressEventGeneration && trackBar.Enabled)
            {
                OnTimeChanged?.Invoke(trackBar.Value);
            }
        }

        private void UpdateTimeLabels()
        {
            var currentTime = scriptTrackBar.Value;
            var currentLength = scriptTrackBar.Maximum;

            currentTimeLabel.Text = FormatMilliseconds(currentTime);
            durationLabel.Text = FormatMilliseconds(currentLength);
        }

        private static string FormatMilliseconds(int ms)
        {
            return TimeSpan.FromMilliseconds(ms).ToString("g");
        }
    }
}
