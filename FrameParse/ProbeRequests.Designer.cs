namespace ProbeMon
{
    partial class ProbeRequests
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProbeRequests));
            this.CloseBtn = new System.Windows.Forms.Button();
            this.RequestFramesList = new System.Windows.Forms.ListBox();
            this.PauseResumeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CloseBtn.Location = new System.Drawing.Point(361, 321);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 0;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // RequestFramesList
            // 
            this.RequestFramesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestFramesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RequestFramesList.FormattingEnabled = true;
            this.RequestFramesList.Location = new System.Drawing.Point(3, 3);
            this.RequestFramesList.Name = "RequestFramesList";
            this.RequestFramesList.Size = new System.Drawing.Size(874, 312);
            this.RequestFramesList.TabIndex = 1;
            // 
            // PauseResumeBtn
            // 
            this.PauseResumeBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PauseResumeBtn.Location = new System.Drawing.Point(442, 321);
            this.PauseResumeBtn.Name = "PauseResumeBtn";
            this.PauseResumeBtn.Size = new System.Drawing.Size(75, 23);
            this.PauseResumeBtn.TabIndex = 2;
            this.PauseResumeBtn.Text = "Pause";
            this.PauseResumeBtn.UseVisualStyleBackColor = true;
            this.PauseResumeBtn.Click += new System.EventHandler(this.PauseResumeBtn_Click);
            // 
            // ProbeRequests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(877, 352);
            this.ControlBox = false;
            this.Controls.Add(this.PauseResumeBtn);
            this.Controls.Add(this.RequestFramesList);
            this.Controls.Add(this.CloseBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ProbeRequests";
            this.Text = "Probe Request Frames";
            this.Load += new System.EventHandler(this.ProbeRequests_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProbeRequests_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.ListBox RequestFramesList;
        private System.Windows.Forms.Button PauseResumeBtn;
    }
}