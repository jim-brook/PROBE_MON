namespace ProbeMon
{
    partial class DetailsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailsView));
            this.DetailedView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // DetailedView
            // 
            this.DetailedView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailedView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DetailedView.FormattingEnabled = true;
            this.DetailedView.Location = new System.Drawing.Point(3, 0);
            this.DetailedView.Name = "DetailedView";
            this.DetailedView.Size = new System.Drawing.Size(1114, 585);
            this.DetailedView.TabIndex = 0;
            // 
            // DetailsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1119, 589);
            this.Controls.Add(this.DetailedView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DetailsView";
            this.Text = "Details View";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox DetailedView;
    }
}