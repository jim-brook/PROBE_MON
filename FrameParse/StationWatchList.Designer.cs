namespace ProbeMon
{
    partial class StationWatchList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StationWatchList));
            this.AddrWatchList = new System.Windows.Forms.ListBox();
            this.SSIDWatchList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddrWatchList
            // 
            this.AddrWatchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddrWatchList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AddrWatchList.FormattingEnabled = true;
            this.AddrWatchList.Location = new System.Drawing.Point(3, 6);
            this.AddrWatchList.Name = "AddrWatchList";
            this.AddrWatchList.Size = new System.Drawing.Size(964, 130);
            this.AddrWatchList.TabIndex = 0;
            this.AddrWatchList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddrWatchList_MouseClick);
            // 
            // SSIDWatchList
            // 
            this.SSIDWatchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SSIDWatchList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SSIDWatchList.FormattingEnabled = true;
            this.SSIDWatchList.Location = new System.Drawing.Point(3, 6);
            this.SSIDWatchList.Name = "SSIDWatchList";
            this.SSIDWatchList.Size = new System.Drawing.Size(964, 130);
            this.SSIDWatchList.TabIndex = 1;
            this.SSIDWatchList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SSIDWatchList_MouseClick);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(450, 304);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.AddrWatchList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SSIDWatchList);
            this.splitContainer1.Size = new System.Drawing.Size(974, 295);
            this.splitContainer1.SplitterDistance = 147;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 3;
            // 
            // StationWatchList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(979, 326);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "StationWatchList";
            this.Text = "Station Watch List";
            this.Load += new System.EventHandler(this.StationWatchList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StationWatchList_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox AddrWatchList;
        private System.Windows.Forms.ListBox SSIDWatchList;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.SplitContainer splitContainer1;
    }
}