namespace ProbeMon
{
    partial class FrameControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameControl));
            this.AirPCapDevList = new System.Windows.Forms.ComboBox();
            this.label1ChnNo = new System.Windows.Forms.Label();
            this.StartCapture = new System.Windows.Forms.Button();
            this.ChannelNumberSelect = new System.Windows.Forms.ComboBox();
            this.LinuxP48011 = new System.Windows.Forms.CheckBox();
            this.LinuxP48012 = new System.Windows.Forms.CheckBox();
            this.WinP48014 = new System.Windows.Forms.CheckBox();
            this.WinP48016 = new System.Windows.Forms.CheckBox();
            this.LinuxP48015 = new System.Windows.Forms.CheckBox();
            this.LocalCapDev = new System.Windows.Forms.CheckBox();
            this.WinP48017 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // AirPCapDevList
            // 
            this.AirPCapDevList.BackColor = System.Drawing.SystemColors.Control;
            this.AirPCapDevList.FormattingEnabled = true;
            this.AirPCapDevList.Location = new System.Drawing.Point(150, 122);
            this.AirPCapDevList.Name = "AirPCapDevList";
            this.AirPCapDevList.Size = new System.Drawing.Size(351, 21);
            this.AirPCapDevList.TabIndex = 6;
            // 
            // label1ChnNo
            // 
            this.label1ChnNo.AutoSize = true;
            this.label1ChnNo.Location = new System.Drawing.Point(569, 126);
            this.label1ChnNo.Name = "label1ChnNo";
            this.label1ChnNo.Size = new System.Drawing.Size(66, 13);
            this.label1ChnNo.TabIndex = 5;
            this.label1ChnNo.Text = "Channel No.";
            // 
            // StartCapture
            // 
            this.StartCapture.Location = new System.Drawing.Point(292, 170);
            this.StartCapture.Name = "StartCapture";
            this.StartCapture.Size = new System.Drawing.Size(75, 23);
            this.StartCapture.TabIndex = 4;
            this.StartCapture.Text = "Start";
            this.StartCapture.UseVisualStyleBackColor = true;
            this.StartCapture.Click += new System.EventHandler(this.StartCapture_Click);
            // 
            // ChannelNumberSelect
            // 
            this.ChannelNumberSelect.BackColor = System.Drawing.SystemColors.Control;
            this.ChannelNumberSelect.FormattingEnabled = true;
            this.ChannelNumberSelect.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "36",
            "38",
            "40",
            "42",
            "44",
            "46",
            "48",
            "52",
            "56",
            "60",
            "64",
            "100",
            "104",
            "108",
            "112",
            "116",
            "120",
            "124",
            "128",
            "132",
            "136",
            "140",
            "149",
            "153",
            "157",
            "161",
            "165",
            "183",
            "184",
            "185",
            "186",
            "187",
            "188",
            "189",
            "192",
            "196",
            "131",
            "132",
            "133",
            "134",
            "135",
            "136",
            "137",
            "138",
            "149",
            "153",
            "157",
            "161",
            "165"});
            this.ChannelNumberSelect.Location = new System.Drawing.Point(511, 122);
            this.ChannelNumberSelect.Name = "ChannelNumberSelect";
            this.ChannelNumberSelect.Size = new System.Drawing.Size(52, 21);
            this.ChannelNumberSelect.TabIndex = 3;
            // 
            // LinuxP48011
            // 
            this.LinuxP48011.AutoSize = true;
            this.LinuxP48011.Checked = true;
            this.LinuxP48011.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LinuxP48011.Location = new System.Drawing.Point(13, 13);
            this.LinuxP48011.Name = "LinuxP48011";
            this.LinuxP48011.Size = new System.Drawing.Size(186, 17);
            this.LinuxP48011.TabIndex = 11;
            this.LinuxP48011.Text = "Linux Capture Device, Port 48011";
            this.LinuxP48011.UseVisualStyleBackColor = true;
            // 
            // LinuxP48012
            // 
            this.LinuxP48012.AutoSize = true;
            this.LinuxP48012.Checked = true;
            this.LinuxP48012.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LinuxP48012.Location = new System.Drawing.Point(13, 31);
            this.LinuxP48012.Name = "LinuxP48012";
            this.LinuxP48012.Size = new System.Drawing.Size(186, 17);
            this.LinuxP48012.TabIndex = 12;
            this.LinuxP48012.Text = "Linux Capture Device, Port 48012";
            this.LinuxP48012.UseVisualStyleBackColor = true;
            // 
            // WinP48014
            // 
            this.WinP48014.AutoSize = true;
            this.WinP48014.Checked = true;
            this.WinP48014.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WinP48014.Location = new System.Drawing.Point(12, 68);
            this.WinP48014.Name = "WinP48014";
            this.WinP48014.Size = new System.Drawing.Size(205, 17);
            this.WinP48014.TabIndex = 13;
            this.WinP48014.Text = "Windows Capture Device, Port 48014";
            this.WinP48014.UseVisualStyleBackColor = true;
            // 
            // WinP48016
            // 
            this.WinP48016.AutoSize = true;
            this.WinP48016.Checked = true;
            this.WinP48016.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WinP48016.Location = new System.Drawing.Point(12, 86);
            this.WinP48016.Name = "WinP48016";
            this.WinP48016.Size = new System.Drawing.Size(205, 17);
            this.WinP48016.TabIndex = 14;
            this.WinP48016.Text = "Windows Capture Device, Port 48016";
            this.WinP48016.UseVisualStyleBackColor = true;
            // 
            // LinuxP48015
            // 
            this.LinuxP48015.AutoSize = true;
            this.LinuxP48015.Checked = true;
            this.LinuxP48015.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LinuxP48015.Location = new System.Drawing.Point(13, 50);
            this.LinuxP48015.Name = "LinuxP48015";
            this.LinuxP48015.Size = new System.Drawing.Size(186, 17);
            this.LinuxP48015.TabIndex = 15;
            this.LinuxP48015.Text = "Linux Capture Device, Port 48015";
            this.LinuxP48015.UseVisualStyleBackColor = true;
            // 
            // LocalCapDev
            // 
            this.LocalCapDev.AutoSize = true;
            this.LocalCapDev.Location = new System.Drawing.Point(12, 121);
            this.LocalCapDev.Name = "LocalCapDev";
            this.LocalCapDev.Size = new System.Drawing.Size(129, 17);
            this.LocalCapDev.TabIndex = 16;
            this.LocalCapDev.Text = "Local AirPcap Device";
            this.LocalCapDev.UseVisualStyleBackColor = true;
            // 
            // WinP48017
            // 
            this.WinP48017.AutoSize = true;
            this.WinP48017.Checked = true;
            this.WinP48017.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WinP48017.Location = new System.Drawing.Point(12, 104);
            this.WinP48017.Name = "WinP48017";
            this.WinP48017.Size = new System.Drawing.Size(205, 17);
            this.WinP48017.TabIndex = 17;
            this.WinP48017.Text = "Windows Capture Device, Port 48017";
            this.WinP48017.UseVisualStyleBackColor = true;
            // 
            // FrameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 202);
            this.Controls.Add(this.WinP48017);
            this.Controls.Add(this.LocalCapDev);
            this.Controls.Add(this.LinuxP48015);
            this.Controls.Add(this.WinP48016);
            this.Controls.Add(this.WinP48014);
            this.Controls.Add(this.LinuxP48012);
            this.Controls.Add(this.LinuxP48011);
            this.Controls.Add(this.AirPCapDevList);
            this.Controls.Add(this.StartCapture);
            this.Controls.Add(this.ChannelNumberSelect);
            this.Controls.Add(this.label1ChnNo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(658, 240);
            this.MinimumSize = new System.Drawing.Size(658, 240);
            this.Name = "FrameControl";
            this.Text = "Frame Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrameControl_FormClosing);
            this.Load += new System.EventHandler(this.FrameControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartCapture;
        public System.Windows.Forms.ComboBox ChannelNumberSelect;
        private System.Windows.Forms.Label label1ChnNo;
        public System.Windows.Forms.ComboBox AirPCapDevList;
        private System.Windows.Forms.CheckBox LinuxP48011;
        private System.Windows.Forms.CheckBox LinuxP48012;
        private System.Windows.Forms.CheckBox WinP48014;
        private System.Windows.Forms.CheckBox WinP48016;
        private System.Windows.Forms.CheckBox LinuxP48015;
        private System.Windows.Forms.CheckBox LocalCapDev;
        private System.Windows.Forms.CheckBox WinP48017;
    }
}

