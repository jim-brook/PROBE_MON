namespace ProbeMon
{
    partial class AddNewStationAddrAlarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewStationAddrAlarm));
            this.AddBtn = new System.Windows.Forms.Button();
            this.SetInRangeAlarm = new System.Windows.Forms.CheckBox();
            this.StaAddr = new System.Windows.Forms.TextBox();
            this.SetRssiMax = new System.Windows.Forms.CheckBox();
            this.SetRssiMin = new System.Windows.Forms.CheckBox();
            this.MaxRssi = new System.Windows.Forms.ComboBox();
            this.MinRssi = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(156, 325);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 0;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // SetInRangeAlarm
            // 
            this.SetInRangeAlarm.AutoSize = true;
            this.SetInRangeAlarm.Location = new System.Drawing.Point(32, 13);
            this.SetInRangeAlarm.Name = "SetInRangeAlarm";
            this.SetInRangeAlarm.Size = new System.Drawing.Size(70, 17);
            this.SetInRangeAlarm.TabIndex = 1;
            this.SetInRangeAlarm.Text = "In Range";
            this.SetInRangeAlarm.UseVisualStyleBackColor = true;
            // 
            // StaAddr
            // 
            this.StaAddr.Location = new System.Drawing.Point(120, 13);
            this.StaAddr.Name = "StaAddr";
            this.StaAddr.Size = new System.Drawing.Size(121, 20);
            this.StaAddr.TabIndex = 2;
            // 
            // SetRssiMax
            // 
            this.SetRssiMax.AutoSize = true;
            this.SetRssiMax.Location = new System.Drawing.Point(32, 48);
            this.SetRssiMax.Name = "SetRssiMax";
            this.SetRssiMax.Size = new System.Drawing.Size(69, 17);
            this.SetRssiMax.TabIndex = 3;
            this.SetRssiMax.Text = "Rssi Max";
            this.SetRssiMax.UseVisualStyleBackColor = true;
            // 
            // SetRssiMin
            // 
            this.SetRssiMin.AutoSize = true;
            this.SetRssiMin.Location = new System.Drawing.Point(32, 81);
            this.SetRssiMin.Name = "SetRssiMin";
            this.SetRssiMin.Size = new System.Drawing.Size(66, 17);
            this.SetRssiMin.TabIndex = 5;
            this.SetRssiMin.Text = "Rssi Min";
            this.SetRssiMin.UseVisualStyleBackColor = true;
            // 
            // MaxRssi
            // 
            this.MaxRssi.FormattingEnabled = true;
            this.MaxRssi.Items.AddRange(new object[] {
            "0",
            "-1",
            "-2",
            "-3",
            "-4",
            "-5",
            "-6",
            "-7",
            "-8",
            "-9",
            "-10",
            "-11",
            "-12",
            "-13",
            "-14",
            "-15",
            "-16",
            "-17",
            "-18",
            "-19",
            "-20",
            "-21",
            "-22",
            "-23",
            "-24",
            "-25",
            "-26",
            "-27",
            "-28",
            "-29",
            "-30",
            "-31",
            "-32",
            "-33",
            "-34",
            "-35",
            "-36",
            "-37",
            "-38",
            "-39",
            "-40",
            "-41",
            "-42",
            "-43",
            "-44",
            "-45",
            "-46",
            "-47",
            "-48",
            "-49",
            "-50",
            "-51",
            "-52",
            "-53",
            "-54",
            "-55",
            "-56",
            "-57",
            "-58",
            "-59",
            "-60",
            "-61",
            "-62",
            "-63",
            "-64",
            "-65",
            "-66",
            "-67",
            "-68",
            "-69",
            "-70",
            "-71",
            "-72",
            "-73",
            "-74",
            "-75",
            "-76",
            "-77",
            "-78",
            "-79",
            "-80",
            "-81",
            "-82",
            "-83",
            "-84",
            "-85",
            "-86",
            "-87",
            "-88",
            "-89",
            "-90",
            "-91",
            "-92",
            "-93",
            "-94",
            "-95",
            "-96",
            "-97",
            "-98",
            "-99",
            "-100"});
            this.MaxRssi.Location = new System.Drawing.Point(120, 48);
            this.MaxRssi.Name = "MaxRssi";
            this.MaxRssi.Size = new System.Drawing.Size(53, 21);
            this.MaxRssi.TabIndex = 7;
            // 
            // MinRssi
            // 
            this.MinRssi.FormattingEnabled = true;
            this.MinRssi.Items.AddRange(new object[] {
            "0",
            "-1",
            "-2",
            "-3",
            "-4",
            "-5",
            "-6",
            "-7",
            "-8",
            "-9",
            "-10",
            "-11",
            "-12",
            "-13",
            "-14",
            "-15",
            "-16",
            "-17",
            "-18",
            "-19",
            "-20",
            "-21",
            "-22",
            "-23",
            "-24",
            "-25",
            "-26",
            "-27",
            "-28",
            "-29",
            "-30",
            "-31",
            "-32",
            "-33",
            "-34",
            "-35",
            "-36",
            "-37",
            "-38",
            "-39",
            "-40",
            "-41",
            "-42",
            "-43",
            "-44",
            "-45",
            "-46",
            "-47",
            "-48",
            "-49",
            "-50",
            "-51",
            "-52",
            "-53",
            "-54",
            "-55",
            "-56",
            "-57",
            "-58",
            "-59",
            "-60",
            "-61",
            "-62",
            "-63",
            "-64",
            "-65",
            "-66",
            "-67",
            "-68",
            "-69",
            "-70",
            "-71",
            "-72",
            "-73",
            "-74",
            "-75",
            "-76",
            "-77",
            "-78",
            "-79",
            "-80",
            "-81",
            "-82",
            "-83",
            "-84",
            "-85",
            "-86",
            "-87",
            "-88",
            "-89",
            "-90",
            "-91",
            "-92",
            "-93",
            "-94",
            "-95",
            "-96",
            "-97",
            "-98",
            "-99",
            "-100"});
            this.MinRssi.Location = new System.Drawing.Point(120, 81);
            this.MinRssi.Name = "MinRssi";
            this.MinRssi.Size = new System.Drawing.Size(53, 21);
            this.MinRssi.TabIndex = 8;
            // 
            // AddNewStationAddrAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 360);
            this.Controls.Add(this.MinRssi);
            this.Controls.Add(this.MaxRssi);
            this.Controls.Add(this.SetRssiMin);
            this.Controls.Add(this.SetRssiMax);
            this.Controls.Add(this.StaAddr);
            this.Controls.Add(this.SetInRangeAlarm);
            this.Controls.Add(this.AddBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddNewStationAddrAlarm";
            this.Text = "Add New Station Address Alarm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.CheckBox SetInRangeAlarm;
        private System.Windows.Forms.CheckBox SetRssiMax;
        private System.Windows.Forms.CheckBox SetRssiMin;
        public System.Windows.Forms.TextBox StaAddr;
        private System.Windows.Forms.ComboBox MaxRssi;
        private System.Windows.Forms.ComboBox MinRssi;
    }
}