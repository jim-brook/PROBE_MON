using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProbeMon
{
    public partial class AddToWatchList : Form
    {
        public FrameControl Context;
        public AddToWatchList(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }
        public void SelectStationAddress(object SelectedItem)
        {
            int FieldEnd;
            string Item = SelectedItem as string;
            if (Item == null || Item == string.Empty) { return; }
            FieldEnd = Item.IndexOf('\t');
            char[] SourceAdd = new char[FieldEnd];
            Item.CopyTo(0, SourceAdd, 0, FieldEnd);
            string SrcAddr = new string(SourceAdd);
            //Console.WriteLine(SrcAddr);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            char[] DestAdd = new char[FieldEnd];
            Item.CopyTo(0, DestAdd, 0, FieldEnd);
            string DstAddr = new string(DestAdd);
            //Console.WriteLine(DstAddr);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            char[] bssid = new char[FieldEnd];
            Item.CopyTo(0, bssid, 0, FieldEnd);
            string BSSID = new string(bssid);
            // Console.WriteLine(BSSID);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            char[] chan = new char[FieldEnd];
            Item.CopyTo(0, chan, 0, FieldEnd);
            string RadioTapChannel = new string(chan);
            //Console.WriteLine(RadioTapChannel);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            char[] rssi = new char[FieldEnd];
            Item.CopyTo(0, rssi, 0, FieldEnd);
            string RadioTapRSSI = new string(rssi);
            //Console.WriteLine(RadioTapRSSI);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            char[] frametype = new char[FieldEnd];
            Item.CopyTo(0, frametype, 0, FieldEnd);
            string FrameType = new string(frametype);
            //Console.WriteLine(RadioTapRSSI);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);



            FieldEnd = Item.IndexOf('\t');
            char[] timestamp = new char[FieldEnd];
            Item.CopyTo(0, timestamp, 0, FieldEnd);
            string TimeStamp = new string(timestamp);
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.Length;
            char[] ssid = new char[FieldEnd];
            Item.CopyTo(0, ssid, 0, FieldEnd);
            string SSID = new string(ssid);
            //Console.WriteLine(SSID);

            StationInfo StaInformation = new StationInfo();
            List<StationInfo> FUList = new List<StationInfo>();
            FUList.Add(StaInformation);
            StaInformation.SrcAddr = SrcAddr;
            StaInformation.DstAddr = DstAddr;
            StaInformation.BSSID = BSSID;
            StaInformation.RadioTapChannel = RadioTapChannel;
            StaInformation.RadioTapRSSI = RadioTapRSSI;
            StaInformation.FrameType = FrameType;
            StaInformation.TimeStamp = TimeStamp;
            StaInformation.SSID = SSID;

            FrameInformationDGV.DataSource = null;
            // FrameInformationDGV.ColumnCount = 1;
            FrameInformationDGV.DataSource = FUList;
            FrameInformationDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            FrameInformationDGV.RowHeadersWidth = 4;
            FrameInformationDGV.RowHeadersVisible = false;
            FrameInformationDGV.Invalidate();
            FrameInformationDGV.ClearSelection();

        }

        private void FrameInformationDGV_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void FrameInformationDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FrameInformationDGV.CurrentCell == null) { return; }
            if (FrameInformationDGV.CurrentCell.ColumnIndex.Equals(0) && e.RowIndex != -1)
            {
                if (FrameInformationDGV.CurrentCell != null && FrameInformationDGV.CurrentCell.Value != null)
                {
                    string SrcAdd = (string)FrameInformationDGV.CurrentCell.Value;
                    Context.WatchList.AddStationAddress(SrcAdd);
                    FrameInformationDGV.CurrentCell.Style.ForeColor = Color.Blue;
                    FrameInformationDGV.CurrentCell.Style.BackColor = Color.White;
                }
            }
            else if (FrameInformationDGV.CurrentCell.ColumnIndex.Equals(1) && e.RowIndex != -1)
            {
                if (FrameInformationDGV.CurrentCell != null && FrameInformationDGV.CurrentCell.Value != null)
                {
                    string DstAdd = (string)FrameInformationDGV.CurrentCell.Value;
                    Context.WatchList.AddStationAddress(DstAdd);
                    FrameInformationDGV.CurrentCell.Style.ForeColor = Color.Blue;
                    FrameInformationDGV.CurrentCell.Style.BackColor = Color.White;
                }
            }
            else if (FrameInformationDGV.CurrentCell.ColumnIndex.Equals(7) && e.RowIndex != -1)
            {
                if (FrameInformationDGV.CurrentCell != null && FrameInformationDGV.CurrentCell.Value != null)
                {
                    string TargetSSID = (string)FrameInformationDGV.CurrentCell.Value;
                    Context.WatchList.AddSSID(TargetSSID);
                    FrameInformationDGV.CurrentCell.Style.ForeColor = Color.Blue;
                    FrameInformationDGV.CurrentCell.Style.BackColor = Color.White;
                }
            }
        }

        private void AddSourceAddressBtn_Click(object sender, EventArgs e)
        {
            string SrcAdd = FrameInformationDGV.Rows[0].Cells[0].Value.ToString();
            Context.WatchList.AddStationAddress(SrcAdd);
        }

        private void AddDestinationAddress_Click(object sender, EventArgs e)
        {
            string DestAddr = FrameInformationDGV.Rows[0].Cells[1].Value.ToString();
            Context.WatchList.AddStationAddress(DestAddr);
        }

        private void AddSSID_Click(object sender, EventArgs e)
        {
            string TargetSSID = FrameInformationDGV.Rows[0].Cells[7].Value.ToString();
            Context.WatchList.AddSSID(TargetSSID);
        }
    }
    public class StationInfo
    {
        public string SrcAddr { get; set; }
        public string DstAddr { get; set; }
        public string BSSID { get; set; }
        public string RadioTapChannel { get; set; }
        public string RadioTapRSSI { get; set; }
        public string FrameType { get; set; }
        public string TimeStamp { get; set; }
        public string SSID { get; set; }
    }
}
