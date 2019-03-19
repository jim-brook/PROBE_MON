namespace ProbeMon
{
    partial class AddToWatchList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddToWatchList));
            this.FrameInformationDGV = new System.Windows.Forms.DataGridView();
            this.AddSourceAddressBtn = new System.Windows.Forms.Button();
            this.AddDestinationAddress = new System.Windows.Forms.Button();
            this.AddSSID = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FrameInformationDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // FrameInformationDGV
            // 
            this.FrameInformationDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameInformationDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FrameInformationDGV.Location = new System.Drawing.Point(4, 4);
            this.FrameInformationDGV.Name = "FrameInformationDGV";
            this.FrameInformationDGV.Size = new System.Drawing.Size(842, 48);
            this.FrameInformationDGV.TabIndex = 0;
            this.FrameInformationDGV.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.FrameInformationDGV_CellBeginEdit);
            this.FrameInformationDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FrameInformationDGV_CellClick);
            // 
            // AddSourceAddressBtn
            // 
            this.AddSourceAddressBtn.Location = new System.Drawing.Point(7, 59);
            this.AddSourceAddressBtn.Name = "AddSourceAddressBtn";
            this.AddSourceAddressBtn.Size = new System.Drawing.Size(78, 23);
            this.AddSourceAddressBtn.TabIndex = 1;
            this.AddSourceAddressBtn.Text = "Add  Address";
            this.AddSourceAddressBtn.UseVisualStyleBackColor = true;
            this.AddSourceAddressBtn.Click += new System.EventHandler(this.AddSourceAddressBtn_Click);
            // 
            // AddDestinationAddress
            // 
            this.AddDestinationAddress.Location = new System.Drawing.Point(93, 59);
            this.AddDestinationAddress.Name = "AddDestinationAddress";
            this.AddDestinationAddress.Size = new System.Drawing.Size(78, 23);
            this.AddDestinationAddress.TabIndex = 2;
            this.AddDestinationAddress.Text = "Add  Address";
            this.AddDestinationAddress.UseVisualStyleBackColor = true;
            this.AddDestinationAddress.Click += new System.EventHandler(this.AddDestinationAddress_Click);
            // 
            // AddSSID
            // 
            this.AddSSID.Location = new System.Drawing.Point(696, 59);
            this.AddSSID.Name = "AddSSID";
            this.AddSSID.Size = new System.Drawing.Size(78, 23);
            this.AddSSID.TabIndex = 3;
            this.AddSSID.Text = "Add  SSID";
            this.AddSSID.UseVisualStyleBackColor = true;
            this.AddSSID.Click += new System.EventHandler(this.AddSSID_Click);
            // 
            // AddToWatchList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 89);
            this.Controls.Add(this.AddSSID);
            this.Controls.Add(this.AddDestinationAddress);
            this.Controls.Add(this.AddSourceAddressBtn);
            this.Controls.Add(this.FrameInformationDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1500, 127);
            this.MinimumSize = new System.Drawing.Size(16, 127);
            this.Name = "AddToWatchList";
            this.Text = "Add To Watch List";
            ((System.ComponentModel.ISupportInitialize)(this.FrameInformationDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView FrameInformationDGV;
        private System.Windows.Forms.Button AddSourceAddressBtn;
        private System.Windows.Forms.Button AddDestinationAddress;
        private System.Windows.Forms.Button AddSSID;
    }
}