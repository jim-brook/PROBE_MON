using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using SharpPcap;
using SharpPcap.AirPcap;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProbeMon
{
    public partial class FrameControl : Form
    {
        /// <summary>
        /// StationAlarms describes the MAC station and some of it's properties used for threshold comparison and alarm generation.
        /// <value>InRange = Alarms if station comes in range of receiver.  True/False.</value>
        /// <value>RadioTapRSSIHigh = Alrams if the station's RSSI exceeds the threshold value contained in RadioTapRSSIValueHigh. True/False.</value>
        /// <value>RadioTapRSSILow  = Alrams if the station's RSSI drops below the threshold value contained in RadioTapRSSIValueLow. True/False.</value> 
        /// <value>RadioTapRSSIValueHigh = The actual RSSI value used during comparison. This gets assigned automatically.</value>
        /// <value>RadioTapRSSIValueLow =  The actual RSSI value used during comparison. This gets assigned automatically.</value>
        /// <value>LocId = The cell/receiver number where signal was received.</value>
        /// <value>StationAddress = The station MAC address.</value>
        /// </summary>
        [Serializable]
        public struct StationAlarms
        {
            public bool InRange;
            public bool RadioTapRSSIHigh;
            public bool RadioTapRSSILow;
            public sbyte RadioTapRSSIValueHigh;
            public sbyte RadioTapRSSIValueLow;
            public int LocId;
            public String StationAddress;
        }
        /// <summary>
        /// SSIDAlarms is used to compare against frames orginating from 802.11 clients that contain an SSID. These could be probe requests, association requests, etc.
        /// <value>StationRequest = Boolean used to indicate that it is used against client requests with an SSID.</value>
        /// <value>SSID = The SSID to compare against and alarm on.</value>
        /// </summary>
        [Serializable]
        public struct SSIDAlarms
        {
            public bool StationRequest;
            public string SSID;
        }
        [Serializable]
        public struct StationTags
        {
            public string MAC;
            public string Tag;
        }
        public bool ProgramIsClosing;
#if USE_UDP
        public UDPRxSeq UdpEngine;
#endif
        public TcpIPC[] IpcComm;
        public int IpcCommCounter;
        public PacketParsing PacketParser;
        public AirPcapDeviceList AirPCapDevices;
        public Monitor IncomingPacketMonitor;
        public StationWatchList WatchList;
        public Alarms ActiveAlarms;
        public ProbeRequests ProbeRequestors;
        public volatile List<StationTags> StationTagList = new List<StationTags>();
        public volatile List<string> StationWatchList = new List<string>();
        public volatile List<string> SSIDWatchList = new List<string>();
        public volatile List<StationAlarms> StationAlarmsList = new List<StationAlarms>();
        public volatile List<SSIDAlarms> SSIDAlarmsList = new List<SSIDAlarms>();
        public bool WriteFramesToSql;
        public volatile sbyte MaxRSSIAlarm;
        public int PortNumber;
        public Thread GetRemoteFramesThread;
        public enum CaptureType
        {
            None = 0,
            LocalDevice = 1,
            RemoteLinuxDevice = 2,
            RemoteWinDevice = 3,
            RemoteLinuxNoSSID = 4

        }
        public struct IncomingPacketDescriptor
        {
            public CaptureType PacketSource;
            public Packet packet;
            public int PortNo;
            public byte[] RawPacket;

        }
        public FrameControl()
        {
            InitializeComponent();
            IpcCommCounter = 0;
            IpcComm = new TcpIPC[255];
            ProgramIsClosing = false;            
            IncomingPacketMonitor = new Monitor(this);
            PacketParser = new PacketParsing(this);
            WatchList = new StationWatchList(this);
            ActiveAlarms = new Alarms(this);
            ProbeRequestors = new ProbeRequests(this);
            WriteFramesToSql = true;
            MaxRSSIAlarm = 0;
            PortNumber = 0;        
        }

        private void FrameControl_Load(object sender, EventArgs e)
        {
            Loading LoadingProgress = new Loading();
            LoadingProgress.LoadProg.Value = 50;
            LoadingProgress.Show();
            ActiveAlarms.Show();            
            IncomingPacketMonitor.Show();            
            WatchList.Show();          
            PacketParser.Show();
            ProbeRequestors.Show();
            LoadingProgress.Activate();            
            ChannelNumberSelect.SelectedIndex = 0;  
            PacketParser.Hide();
            AirPCapDevices = AirPcapDeviceList.Instance;
            if (AirPCapDevices.Count == 0)
            {
                MessageBox.Show("No AirPCap Devices Found. Re-run as Administrator.\n", "Zero AirPCap Devices", MessageBoxButtons.OK);
                LoadingProgress.Close();
                return;
            }
            foreach (AirPcapDevice dev in AirPCapDevices)
            {
               AirPCapDevList.Items.Add(dev.Description);              
            }
            LoadingProgress.LoadProg.Value = 100;
            LoadingProgress.Hide();
            LoadingProgress.Close();           
        }
        private void FrameControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            int counter = 0;
            ProgramIsClosing = true;         
            for (counter = 0; counter < IpcCommCounter; counter++)
            {
                IpcComm[counter].DeserializedFramesReadyWait.Set();
            }
            ProbeRequestors.CleanUpTimer.Stop();
            ProbeRequestors.CleanUpTimer.Enabled = false;
            IncomingPacketMonitor.AlarmCheckReadyWait.Set();
            PacketParser.PacketBytesReadyWait.Set();
            PacketParser.PacketDataReadyWait.Set();
            PacketParser.PacketMetaDataReadyWait.Set();
            IncomingPacketMonitor.IncomingFrameReadyWait.Set();
            WatchList.UpdateUIWait.Set();
            SaveAddressWatchList();
            SaveSSIDWatchList();
            SaveStationAddressAlarmList();
            SaveSSIDAlarmList();
            SaveStationTags();     
        }
  
        public void SaveStationTags()
        {
            try
            {
                string path = "StationTags.dat";
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, StationTagList);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured writing to file StationTags.dat", Ex.Message.ToString());
            }

        }

        public void ReadStationTags()
        {
            try
            {
                string path = "StationTags.dat";
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                StationTagList = (List<StationTags>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured reading from file StationTags.dat", Ex.Message.ToString());
            }
        }
        public void SaveSSIDAlarmList()
        {
            try
            {
                string path = "SSIDAlarms.dat";
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, SSIDAlarmsList);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured writing to file SSIDAlarms.dat", Ex.Message.ToString());
            }
        }
        public void SaveStationAddressAlarmList()
        {
            try
            {
                string path = "AddrAlarms.dat";
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, StationAlarmsList);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured writing to file AddrAlarms.dat", Ex.Message.ToString());
            }
        }
        public void ReadSSIDAlarmList()
        {
            try
            {
                string path = "SSIDAlarms.dat";
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                SSIDAlarmsList = (List<SSIDAlarms>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured reading from file SSIDAlarms.dat", Ex.Message.ToString());
            }
        }
        public void ReadStationAddressAlarmList()
        {
            try
            {
                string path = "AddrAlarms.dat";
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                StationAlarmsList = (List<StationAlarms>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured reading from file AddrAlarms.dat", Ex.Message.ToString());
            }
        }
        public void SaveAddressWatchList()
        {
            try
            {
                string path = "AddrWatch.dat";
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, StationWatchList);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured writing to file AddrWatch.dat", Ex.Message.ToString());
            }
        }
        public void ReadAddressWatchList()
        {
            try
            {
                string path = "AddrWatch.dat";
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                StationWatchList = (List<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured reading from file AddrWatch.dat", Ex.Message.ToString());
            }
        }
        public void SaveSSIDWatchList()
        {
            try
            {
                string path = "SSIDWatch.dat";
                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, SSIDWatchList);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured writing to file SSIDWatch.dat", Ex.Message.ToString());
            }
        }
        public void ReadSSIDWatchList()
        {
            try
            {
                string path = "SSIDWatch.dat";
                FileStream stream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                SSIDWatchList = (List<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception occured reading from file SSIDWatch.dat", Ex.Message.ToString());
            }
        }
        private void StartCapture_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ProcToStart = new ProcessStartInfo();
            string Instance = string.Empty;
            if (LocalCapDev.Checked)
            {

                Instance = "Local Device";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.LocalDevice, 48009);
                if (ChannelNumberSelect.SelectedItem as string == string.Empty)
                {
                    MessageBox.Show("Please Select A Channel From The List!", "No Channel Selected", MessageBoxButtons.OK);
                    return;
                }
                else if (AirPCapDevList.SelectedItem == null)
                {
                    MessageBox.Show("Please Select An AirPCapDevice From The List!", "No AirPCapDevice Selected", MessageBoxButtons.OK);
                    return;
                }
                WriteFramesToSql = false;
                string args = ChannelNumberSelect.SelectedItem + " " + AirPCapDevList.SelectedIndex.ToString();
                ProcToStart.Arguments = args;
                ProcToStart.FileName = "CaptureConsole.exe";
                Process.Start(ProcToStart);
                System.Threading.Thread.Sleep(750);

                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;


            }
            if (WinP48014.Checked)
            {
                Instance = "48014";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteWinDevice, 48014);
                WriteFramesToSql = false;
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }
            if (WinP48016.Checked)
            {
                Instance = "48016";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteWinDevice, 48016);
                WriteFramesToSql = false;
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }
            if (WinP48017.Checked)
            {
                Instance = "48017";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteWinDevice, 48017);
                WriteFramesToSql = false;
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }
            if (LinuxP48011.Checked)
            {
                Instance = "48011";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteLinuxDevice, 48011);
                WriteFramesToSql = false;
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }
            if (LinuxP48012.Checked)
            {
                Instance = "48012";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteLinuxDevice, 48012); 
                WriteFramesToSql = false;
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }
            if (LinuxP48015.Checked)
            {
                Instance = "48015";
                LoadLists();
                IpcComm[IpcCommCounter++] = new TcpIPC(this, CaptureType.RemoteLinuxDevice, 48015);
                WriteFramesToSql = false; 
                System.Threading.Thread.Sleep(750);
                LocalCapDev.Enabled = false;
                WinP48014.Enabled = false;
                WinP48016.Enabled = false;
                WinP48017.Enabled = false;
                LinuxP48011.Enabled = false;
                LinuxP48012.Enabled = false;
                LinuxP48015.Enabled = false;
                ChannelNumberSelect.Enabled = false;
                AirPCapDevList.Enabled = false;
                StartCapture.Enabled = false;
            }  
            ActiveAlarms.Text += " " + Instance.ToString();
            IncomingPacketMonitor.Text += " " + Instance.ToString();
            WatchList.Text += " " + Instance.ToString();
            this.Text += " " + Instance.ToString();
            System.Threading.Thread.Sleep(750);
            IncomingPacketMonitor.Activate();
            ProbeRequestors.CleanUpTimer.Start();
            ProbeRequestors.CleanUpTimer.Enabled = true;
        }
        public void LoadLists()
        {
            ReadAddressWatchList();
            ReadSSIDWatchList();
            ReadStationAddressAlarmList();
            ReadSSIDAlarmList();
            ReadStationTags();
        }  
    }
}



