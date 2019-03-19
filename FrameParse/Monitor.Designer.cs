namespace ProbeMon
{
    partial class Monitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            this.MainList = new System.Windows.Forms.ListBox();
            this.WatchListBtn = new System.Windows.Forms.Button();
            this.EditWatchListBtn = new System.Windows.Forms.Button();
            this.EditAlarmsBtn = new System.Windows.Forms.Button();
            this.ShowActiveAlarms = new System.Windows.Forms.Button();
            this.ProbeRequestsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainList
            // 
            this.MainList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MainList.FormattingEnabled = true;
            this.MainList.Location = new System.Drawing.Point(2, 4);
            this.MainList.Name = "MainList";
            this.MainList.Size = new System.Drawing.Size(977, 312);
            this.MainList.TabIndex = 0;
            this.MainList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainList_MouseClick);
            this.MainList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainList_MouseDoubleClick);
            // 
            // WatchListBtn
            // 
            this.WatchListBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.WatchListBtn.Location = new System.Drawing.Point(442, 328);
            this.WatchListBtn.Name = "WatchListBtn";
            this.WatchListBtn.Size = new System.Drawing.Size(75, 23);
            this.WatchListBtn.TabIndex = 1;
            this.WatchListBtn.Text = "Watch List";
            this.WatchListBtn.UseVisualStyleBackColor = true;
            this.WatchListBtn.Click += new System.EventHandler(this.WatchListbtn_Click);
            // 
            // EditWatchListBtn
            // 
            this.EditWatchListBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.EditWatchListBtn.Location = new System.Drawing.Point(542, 328);
            this.EditWatchListBtn.Name = "EditWatchListBtn";
            this.EditWatchListBtn.Size = new System.Drawing.Size(75, 23);
            this.EditWatchListBtn.TabIndex = 2;
            this.EditWatchListBtn.Text = "Edit Watch";
            this.EditWatchListBtn.UseVisualStyleBackColor = true;
            this.EditWatchListBtn.Click += new System.EventHandler(this.EditWatchListBtn_Click);
            // 
            // EditAlarmsBtn
            // 
            this.EditAlarmsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.EditAlarmsBtn.Location = new System.Drawing.Point(642, 328);
            this.EditAlarmsBtn.Name = "EditAlarmsBtn";
            this.EditAlarmsBtn.Size = new System.Drawing.Size(75, 23);
            this.EditAlarmsBtn.TabIndex = 3;
            this.EditAlarmsBtn.Text = "Edit Alarms";
            this.EditAlarmsBtn.UseVisualStyleBackColor = true;
            this.EditAlarmsBtn.Click += new System.EventHandler(this.EditAlarmsBtn_Click);
            // 
            // ShowActiveAlarms
            // 
            this.ShowActiveAlarms.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ShowActiveAlarms.Location = new System.Drawing.Point(351, 328);
            this.ShowActiveAlarms.Name = "ShowActiveAlarms";
            this.ShowActiveAlarms.Size = new System.Drawing.Size(75, 23);
            this.ShowActiveAlarms.TabIndex = 4;
            this.ShowActiveAlarms.Text = "Alarms";
            this.ShowActiveAlarms.UseVisualStyleBackColor = true;
            this.ShowActiveAlarms.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProbeRequestsBtn
            // 
            this.ProbeRequestsBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ProbeRequestsBtn.Location = new System.Drawing.Point(261, 328);
            this.ProbeRequestsBtn.Name = "ProbeRequestsBtn";
            this.ProbeRequestsBtn.Size = new System.Drawing.Size(75, 23);
            this.ProbeRequestsBtn.TabIndex = 5;
            this.ProbeRequestsBtn.Text = "Requests";
            this.ProbeRequestsBtn.UseVisualStyleBackColor = true;
            this.ProbeRequestsBtn.Click += new System.EventHandler(this.ProbeRequestsBtn_Click);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(979, 353);
            this.ControlBox = false;
            this.Controls.Add(this.ProbeRequestsBtn);
            this.Controls.Add(this.ShowActiveAlarms);
            this.Controls.Add(this.EditAlarmsBtn);
            this.Controls.Add(this.EditWatchListBtn);
            this.Controls.Add(this.WatchListBtn);
            this.Controls.Add(this.MainList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Monitor";
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.Monitor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Monitor_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox MainList;
        private System.Windows.Forms.Button WatchListBtn;
        private System.Windows.Forms.Button EditWatchListBtn;
        private System.Windows.Forms.Button EditAlarmsBtn;
        private System.Windows.Forms.Button ShowActiveAlarms;
        private System.Windows.Forms.Button ProbeRequestsBtn;


    }
}