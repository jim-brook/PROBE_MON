namespace ProbeMon
{
    partial class AddAlarms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAlarms));
            this.StationAddressWatchList = new System.Windows.Forms.ListBox();
            this.AlarmAddressList = new System.Windows.Forms.ListBox();
            this.SSIDWatchList = new System.Windows.Forms.ListBox();
            this.AlarmSSIDList = new System.Windows.Forms.ListBox();
            this.StaAddrToAdd = new System.Windows.Forms.TextBox();
            this.ManualAddToStaWatchList = new System.Windows.Forms.Button();
            this.ManualAddToSSIDWatchList = new System.Windows.Forms.Button();
            this.SSIDToAdd = new System.Windows.Forms.TextBox();
            this.RemoveStationAlarmBtn = new System.Windows.Forms.Button();
            this.RemoveSSIDAlarm = new System.Windows.Forms.Button();
            this.MaxRssi = new System.Windows.Forms.ComboBox();
            this.AddMaxRSSI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StationAddressWatchList
            // 
            this.StationAddressWatchList.FormattingEnabled = true;
            this.StationAddressWatchList.Location = new System.Drawing.Point(1, 2);
            this.StationAddressWatchList.Name = "StationAddressWatchList";
            this.StationAddressWatchList.Size = new System.Drawing.Size(181, 290);
            this.StationAddressWatchList.TabIndex = 0;
            // 
            // AlarmAddressList
            // 
            this.AlarmAddressList.FormattingEnabled = true;
            this.AlarmAddressList.Location = new System.Drawing.Point(320, 2);
            this.AlarmAddressList.Name = "AlarmAddressList";
            this.AlarmAddressList.Size = new System.Drawing.Size(181, 290);
            this.AlarmAddressList.TabIndex = 1;
            // 
            // SSIDWatchList
            // 
            this.SSIDWatchList.FormattingEnabled = true;
            this.SSIDWatchList.Location = new System.Drawing.Point(1, 324);
            this.SSIDWatchList.Name = "SSIDWatchList";
            this.SSIDWatchList.Size = new System.Drawing.Size(181, 290);
            this.SSIDWatchList.TabIndex = 2;
            // 
            // AlarmSSIDList
            // 
            this.AlarmSSIDList.FormattingEnabled = true;
            this.AlarmSSIDList.Location = new System.Drawing.Point(320, 324);
            this.AlarmSSIDList.Name = "AlarmSSIDList";
            this.AlarmSSIDList.Size = new System.Drawing.Size(181, 290);
            this.AlarmSSIDList.TabIndex = 3;
            // 
            // StaAddrToAdd
            // 
            this.StaAddrToAdd.Location = new System.Drawing.Point(196, 77);
            this.StaAddrToAdd.Name = "StaAddrToAdd";
            this.StaAddrToAdd.Size = new System.Drawing.Size(111, 20);
            this.StaAddrToAdd.TabIndex = 11;
            // 
            // ManualAddToStaWatchList
            // 
            this.ManualAddToStaWatchList.Location = new System.Drawing.Point(208, 114);
            this.ManualAddToStaWatchList.Name = "ManualAddToStaWatchList";
            this.ManualAddToStaWatchList.Size = new System.Drawing.Size(75, 23);
            this.ManualAddToStaWatchList.TabIndex = 12;
            this.ManualAddToStaWatchList.Text = "Add";
            this.ManualAddToStaWatchList.UseVisualStyleBackColor = true;
            this.ManualAddToStaWatchList.Click += new System.EventHandler(this.ManualAddToStaWatchList_Click);
            // 
            // ManualAddToSSIDWatchList
            // 
            this.ManualAddToSSIDWatchList.Location = new System.Drawing.Point(208, 464);
            this.ManualAddToSSIDWatchList.Name = "ManualAddToSSIDWatchList";
            this.ManualAddToSSIDWatchList.Size = new System.Drawing.Size(75, 23);
            this.ManualAddToSSIDWatchList.TabIndex = 16;
            this.ManualAddToSSIDWatchList.Text = "Add";
            this.ManualAddToSSIDWatchList.UseVisualStyleBackColor = true;
            this.ManualAddToSSIDWatchList.Click += new System.EventHandler(this.ManualAddToSSIDWatchList_Click);
            // 
            // SSIDToAdd
            // 
            this.SSIDToAdd.Location = new System.Drawing.Point(196, 426);
            this.SSIDToAdd.Name = "SSIDToAdd";
            this.SSIDToAdd.Size = new System.Drawing.Size(111, 20);
            this.SSIDToAdd.TabIndex = 15;
            // 
            // RemoveStationAlarmBtn
            // 
            this.RemoveStationAlarmBtn.Location = new System.Drawing.Point(208, 143);
            this.RemoveStationAlarmBtn.Name = "RemoveStationAlarmBtn";
            this.RemoveStationAlarmBtn.Size = new System.Drawing.Size(75, 23);
            this.RemoveStationAlarmBtn.TabIndex = 17;
            this.RemoveStationAlarmBtn.Text = "Remove";
            this.RemoveStationAlarmBtn.UseVisualStyleBackColor = true;
            this.RemoveStationAlarmBtn.Click += new System.EventHandler(this.RemoveStationAlarmBtn_Click);
            // 
            // RemoveSSIDAlarm
            // 
            this.RemoveSSIDAlarm.Location = new System.Drawing.Point(208, 493);
            this.RemoveSSIDAlarm.Name = "RemoveSSIDAlarm";
            this.RemoveSSIDAlarm.Size = new System.Drawing.Size(75, 23);
            this.RemoveSSIDAlarm.TabIndex = 18;
            this.RemoveSSIDAlarm.Text = "Remove";
            this.RemoveSSIDAlarm.UseVisualStyleBackColor = true;
            this.RemoveSSIDAlarm.Click += new System.EventHandler(this.RemoveSSIDAlarm_Click);
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
            this.MaxRssi.Location = new System.Drawing.Point(221, 282);
            this.MaxRssi.Name = "MaxRssi";
            this.MaxRssi.Size = new System.Drawing.Size(53, 21);
            this.MaxRssi.TabIndex = 19;
            // 
            // AddMaxRSSI
            // 
            this.AddMaxRSSI.Location = new System.Drawing.Point(208, 309);
            this.AddMaxRSSI.Name = "AddMaxRSSI";
            this.AddMaxRSSI.Size = new System.Drawing.Size(75, 23);
            this.AddMaxRSSI.TabIndex = 20;
            this.AddMaxRSSI.Text = "Add";
            this.AddMaxRSSI.UseVisualStyleBackColor = true;
            this.AddMaxRSSI.Click += new System.EventHandler(this.AddMaxRSSI_Click);
            // 
            // AddAlarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 614);
            this.Controls.Add(this.AddMaxRSSI);
            this.Controls.Add(this.MaxRssi);
            this.Controls.Add(this.RemoveSSIDAlarm);
            this.Controls.Add(this.RemoveStationAlarmBtn);
            this.Controls.Add(this.ManualAddToSSIDWatchList);
            this.Controls.Add(this.SSIDToAdd);
            this.Controls.Add(this.ManualAddToStaWatchList);
            this.Controls.Add(this.StaAddrToAdd);
            this.Controls.Add(this.AlarmSSIDList);
            this.Controls.Add(this.SSIDWatchList);
            this.Controls.Add(this.AlarmAddressList);
            this.Controls.Add(this.StationAddressWatchList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddAlarms";
            this.Text = "Add Alarms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox StationAddressWatchList;
        private System.Windows.Forms.ListBox AlarmAddressList;
        private System.Windows.Forms.ListBox SSIDWatchList;
        private System.Windows.Forms.ListBox AlarmSSIDList;
        private System.Windows.Forms.TextBox StaAddrToAdd;
        private System.Windows.Forms.Button ManualAddToStaWatchList;
        private System.Windows.Forms.Button ManualAddToSSIDWatchList;
        private System.Windows.Forms.TextBox SSIDToAdd;
        private System.Windows.Forms.Button RemoveStationAlarmBtn;
        private System.Windows.Forms.Button RemoveSSIDAlarm;
        private System.Windows.Forms.ComboBox MaxRssi;
        private System.Windows.Forms.Button AddMaxRSSI;
    }
}