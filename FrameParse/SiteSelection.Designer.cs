namespace ProbeMon
{
    partial class SiteSelection
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
            this.SelectedAlarmSite = new System.Windows.Forms.ComboBox();
            this.SiteSelectedBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectedAlarmSite
            // 
            this.SelectedAlarmSite.FormattingEnabled = true;
            this.SelectedAlarmSite.Items.AddRange(new object[] {
            "0",
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
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.SelectedAlarmSite.Location = new System.Drawing.Point(109, 114);
            this.SelectedAlarmSite.Name = "SelectedAlarmSite";
            this.SelectedAlarmSite.Size = new System.Drawing.Size(61, 21);
            this.SelectedAlarmSite.TabIndex = 0;
            // 
            // SiteSelectedBtn
            // 
            this.SiteSelectedBtn.Location = new System.Drawing.Point(100, 144);
            this.SiteSelectedBtn.Name = "SiteSelectedBtn";
            this.SiteSelectedBtn.Size = new System.Drawing.Size(75, 23);
            this.SiteSelectedBtn.TabIndex = 1;
            this.SiteSelectedBtn.Text = "Select";
            this.SiteSelectedBtn.UseVisualStyleBackColor = true;
            this.SiteSelectedBtn.Click += new System.EventHandler(this.SiteSelectedBtn_Click);
            // 
            // SiteSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.SiteSelectedBtn);
            this.Controls.Add(this.SelectedAlarmSite);
            this.Name = "SiteSelection";
            this.Text = "Select Site";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox SelectedAlarmSite;
        private System.Windows.Forms.Button SiteSelectedBtn;
    }
}