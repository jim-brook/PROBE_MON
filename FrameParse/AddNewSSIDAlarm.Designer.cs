namespace ProbeMon
{
    partial class AddNewSSIDAlarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewSSIDAlarm));
            this.NetworkSSID = new System.Windows.Forms.TextBox();
            this.SetSSIDRequestsAlarm = new System.Windows.Forms.CheckBox();
            this.AddBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NetworkSSID
            // 
            this.NetworkSSID.Location = new System.Drawing.Point(59, 12);
            this.NetworkSSID.Name = "NetworkSSID";
            this.NetworkSSID.Size = new System.Drawing.Size(251, 20);
            this.NetworkSSID.TabIndex = 4;
            // 
            // SetSSIDRequestsAlarm
            // 
            this.SetSSIDRequestsAlarm.AutoSize = true;
            this.SetSSIDRequestsAlarm.Location = new System.Drawing.Point(38, 40);
            this.SetSSIDRequestsAlarm.Name = "SetSSIDRequestsAlarm";
            this.SetSSIDRequestsAlarm.Size = new System.Drawing.Size(114, 17);
            this.SetSSIDRequestsAlarm.TabIndex = 3;
            this.SetSSIDRequestsAlarm.Text = "Network Requests";
            this.SetSSIDRequestsAlarm.UseVisualStyleBackColor = true;
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(150, 328);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 5;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // AddNewSSIDAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 363);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.NetworkSSID);
            this.Controls.Add(this.SetSSIDRequestsAlarm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddNewSSIDAlarm";
            this.Text = "Add New SSID Alarm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox NetworkSSID;
        private System.Windows.Forms.CheckBox SetSSIDRequestsAlarm;
        private System.Windows.Forms.Button AddBtn;
    }
}