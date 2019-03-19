namespace ProbeMon
{
    partial class Alarms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alarms));
            this.CurrentAlarms = new System.Windows.Forms.ListView();
            this.ClearAlarms = new System.Windows.Forms.Button();
            this.Closebtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentAlarms
            // 
            this.CurrentAlarms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentAlarms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentAlarms.Location = new System.Drawing.Point(1, 3);
            this.CurrentAlarms.Name = "CurrentAlarms";
            this.CurrentAlarms.Size = new System.Drawing.Size(977, 289);
            this.CurrentAlarms.TabIndex = 0;
            this.CurrentAlarms.UseCompatibleStateImageBehavior = false;
            // 
            // ClearAlarms
            // 
            this.ClearAlarms.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ClearAlarms.Location = new System.Drawing.Point(409, 298);
            this.ClearAlarms.Name = "ClearAlarms";
            this.ClearAlarms.Size = new System.Drawing.Size(75, 23);
            this.ClearAlarms.TabIndex = 1;
            this.ClearAlarms.Text = "Clear";
            this.ClearAlarms.UseVisualStyleBackColor = true;
            this.ClearAlarms.Click += new System.EventHandler(this.ClearAlarms_Click);
            // 
            // Closebtn
            // 
            this.Closebtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Closebtn.Location = new System.Drawing.Point(495, 298);
            this.Closebtn.Name = "Closebtn";
            this.Closebtn.Size = new System.Drawing.Size(75, 23);
            this.Closebtn.TabIndex = 2;
            this.Closebtn.Text = "Close";
            this.Closebtn.UseVisualStyleBackColor = true;
            this.Closebtn.Click += new System.EventHandler(this.Closebtn_Click);
            // 
            // Alarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(979, 326);
            this.ControlBox = false;
            this.Controls.Add(this.Closebtn);
            this.Controls.Add(this.ClearAlarms);
            this.Controls.Add(this.CurrentAlarms);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Alarms";
            this.Text = "Alarms";
            this.Load += new System.EventHandler(this.Alarms_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarms_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView CurrentAlarms;
        private System.Windows.Forms.Button ClearAlarms;
        private System.Windows.Forms.Button Closebtn;

    }
}