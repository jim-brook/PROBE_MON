using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace ProbeMon
{
    public partial class Alarms : Form
    {
        public FrameControl Context;
        public Alarms(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }

        private void Alarms_Load(object sender, EventArgs e)
        {
            AddColumns();
        }
        public void AddColumns()
        {
            CurrentAlarms.View = View.Details;
            CurrentAlarms.GridLines = true;
            CurrentAlarms.FullRowSelect = false;
            CurrentAlarms.Columns.Add("Alarm Object");
            CurrentAlarms.Columns.Add("Alarm Type");
            CurrentAlarms.Columns.Add("Time");
            CurrentAlarms.Columns.Add("Details");
            CurrentAlarms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            CurrentAlarms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        public delegate void MyDelegate(AlarmInformation AlarmInfo);
        public void AddAlarms(AlarmInformation AlarmInfo)
        {
            StringBuilder sb = new StringBuilder();
            string[] LvFields = new string[4];
            sb.Clear();
            LvFields[0] = AlarmInfo.AlarmObject.ToString();
            LvFields[1] = AlarmInfo.AlarmProperties.ToString();
            LvFields[2] = AlarmInfo.TimeStamp.ToShortTimeString();
            if (AlarmInfo.AlarmObject == AlarmInformation.AlarmPropertiesObject.StationAddress)
            {
                sb.Append("StaAddr: " + AlarmInfo.RequestedStationAddress.ToString());
                sb.Append(" RSSI: " + AlarmInfo.RadioTapRssiDBM.ToString());
                sb.Append(" Ch: " + AlarmInfo.Channel);
                sb.Append(" Loc: " + AlarmInfo.LocId.ToString()); 
                if (AlarmInfo.AlarmProperties == AlarmInformation.AlarmPropertiesType.StationInRange)
                {
                    if ((AlarmInfo.RequestedNetworkSSID != null) && (AlarmInfo.RequestedNetworkSSID != string.Empty))
                    {
                        sb.Append(" SSID: " + AlarmInfo.RequestedNetworkSSID.ToString());
                    }
                }
                if (AlarmInfo.AlarmProperties == AlarmInformation.AlarmPropertiesType.RadioTapRSSIHigh)
                {
                    sb.Append(" RSSI High: " + AlarmInfo.RadioTapRssiDBMThreshold.ToString());
                }
                if (AlarmInfo.AlarmProperties == AlarmInformation.AlarmPropertiesType.RadioTapRSSILow)
                {
                    sb.Append(" RSSI Low: " + AlarmInfo.RadioTapRssiDBMThreshold.ToString());
                }
                if (AlarmInfo.AlarmProperties == AlarmInformation.AlarmPropertiesType.RadioTapRSSIMax)
                {
                    sb.Append(" RSSI Max: " + AlarmInfo.RadioTapRssiDBM.ToString());
                }
                sb.Append(" FrameType: " + AlarmInfo.FrameType.ToString());
                if (AlarmInfo.Tag != null)
                {
                    sb.Append(" Tag: " + AlarmInfo.Tag.ToString());
                }

            }
            else
            {
                sb.Append("SSID: " + AlarmInfo.RequestedNetworkSSID.ToString() );
                sb.Append(" Packet StaAddr: " + AlarmInfo.RequestedStationAddress.ToString());
                sb.Append(" Packet RSSI: " + AlarmInfo.RadioTapRssiDBM.ToString());
                sb.Append(" FrameTpye: " + AlarmInfo.FrameType.ToString());
            }



            LvFields[3] = sb.ToString();
            ListViewItem LvItem;
            LvItem = new ListViewItem(LvFields);
            CurrentAlarms.BeginUpdate();
            CurrentAlarms.Items.Add(LvItem);
            //Font BetterFont = new Font("Times New Roman", 12f, FontStyle.Regular);
            //ListViewItem Item = CurrentAlarms.Items[0];
            //Item.Font = BetterFont;
            //CurrentAlarms.Items[0].SubItems[0].Font = BetterFont;

            CurrentAlarms.Items[0].UseItemStyleForSubItems = false;
            CurrentAlarms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            CurrentAlarms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            CurrentAlarms.EnsureVisible(CurrentAlarms.Items.Count - 1);
            CurrentAlarms.EndUpdate();
            PlayAlertSound();
        }
        public void PlayAlertSound()
        {
            if (Environment.OSVersion.Version.Major == 6)
            {
                try
                {
                    string FilePath1 = "C:\\Windows\\Media\\Windows Print complete.wav";
                    SoundPlayer simpleSound1 = new SoundPlayer(FilePath1);
                    simpleSound1.Play();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());                    
                }
            }
            if (Environment.OSVersion.Version.Major == 5)
            {
                try
                {
                    string FilePath1 = "C:\\Windows\\Media\\Windows XP Shutdown.wav";
                    SoundPlayer simpleSound1 = new SoundPlayer(FilePath1);
                    simpleSound1.Play();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());                    
                }
            }
        }

        private void ClearAlarms_Click(object sender, EventArgs e)
        {
            CurrentAlarms.BeginUpdate();
            CurrentAlarms.Clear();
            AddColumns();
            CurrentAlarms.EndUpdate();

        }

        private void Alarms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                e.SuppressKeyPress = true;

            }
        }

        private void Closebtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }

    public class AlarmInformation
    {
        public enum AlarmPropertiesObject
        {
            StationAddress = 1,
            NetworkSSID = 2,
            Reserved = 4,
            Reserved1 = 8,
            Reserved2 = 16,
            Reserved3 = 32,
            Reserved4 = 64,
            Reserved5 = 128,
            Reserved6 = 256,
            Reserved7 = 512,
            Reserved8 = 1024,
            Reserved9 = 2048,
            Reserved10 = 4096
        }
        public enum AlarmPropertiesType
        {
            StationInRange = 1,
            RadioTapRSSIHigh = 2,
            RadioTapRSSILow = 4,
            SSIDRequested = 8,
            RadioTapRSSIMax = 16,
            Reserved1 = 32,
            Reserved2 = 64,
            Reserved3 = 128,
            Reserved4 = 256,
            Reserved5 = 512,
            Reserved6 = 1024
        }
        public AlarmPropertiesObject AlarmObject;
        public AlarmPropertiesType AlarmProperties;
        public string TargetStationAddress;
        public string RequestedStationAddress;
        public string TargetNetworkSSID;
        public string RequestedNetworkSSID;
        public string Channel;
        public sbyte RadioTapRssiDBM;
        public sbyte RadioTapRssiDBMThreshold;
        public DateTime TimeStamp;
        public int LocId;
        public string Tag;
        public PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes FrameType;
    }
}
