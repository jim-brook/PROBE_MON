using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProbeMon
{
    public partial class Monitor : Form
    {
        public ContextMenuStrip MainListContextMenu;
        public List<RadioPlusMAC> IncomingFrameList;
        public List<RadioPlusMAC> DisplayFrameList;
        public List<RadioPlusMAC> AlarmCheckFrameList;
        public object IncoimingFrameListLock = new object();
        public object AlarmCheckFrameListLock = new object();     
        public object DisplayFrameListLock = new object();
        public EventWaitHandle IncomingFrameReadyWait;
        public EventWaitHandle AlarmCheckReadyWait;
        public Thread IncoimingFrameThread;
        public Thread AlarmCheckFrameThread;
        public FrameControl Context;
        public bool ScrollingEnabled;
        public Monitor(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
            ScrollingEnabled = true;

            IncomingFrameList = new List<RadioPlusMAC>();
            DisplayFrameList = new List<RadioPlusMAC>();
            AlarmCheckFrameList = new List<RadioPlusMAC>();
            IncomingFrameReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            AlarmCheckReadyWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            IncoimingFrameThread = new Thread(NewFrameReady);
            IncoimingFrameThread.Start();
            AlarmCheckFrameThread = new Thread(CheckAlarmLists);
            AlarmCheckFrameThread.Start();
        }

        public void CheckAlarmLists()
        {
            while (Context.ProgramIsClosing == false)
            {
                AlarmCheckReadyWait.WaitOne();
                DateTime TS = DateTime.Now;
                lock (AlarmCheckFrameListLock)
                {                    
                    foreach (RadioPlusMAC Frame in AlarmCheckFrameList)
                    {
                        if (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ControlCTS)        
                        {
                            foreach (FrameControl.StationAlarms Alarm in Context.StationAlarmsList)
                            {
                                if (Frame.PropDstAddr.ToString() == Alarm.StationAddress)
                                {
                                    if (Alarm.InRange)
                                    {
                                        AlarmInformation AlarmInfo = new AlarmInformation();
                                        AlarmInfo.RequestedStationAddress = Frame.PropDstAddr.ToString();
                                        AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                        AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                        AlarmInfo.TargetStationAddress = Alarm.StationAddress;
                                        AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                        AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.StationInRange;
                                        AlarmInfo.FrameType = Frame.PropFrameType;
                                        AlarmInfo.TimeStamp = TS;
                                        AlarmInfo.LocId = Frame.SourceLoc;
                                        AlarmInfo.Tag = Frame.Tag;
                                        if (Frame.PropProbeRequestSSID != null)
                                        {
                                            AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        }
                                        Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                    }
                                }
                            }
                        }
                        if ((Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse)
                                || (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse))
                        {
                            foreach (FrameControl.StationAlarms Alarm in Context.StationAlarmsList)
                            {
                                if (Frame.PropDstAddr.ToString() == Alarm.StationAddress)
                                {
                                    if (Alarm.InRange)
                                    {
                                        AlarmInformation AlarmInfo = new AlarmInformation();
                                        AlarmInfo.RequestedStationAddress = Frame.PropDstAddr.ToString();                            
                                        AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                        AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                        AlarmInfo.TargetStationAddress = Alarm.StationAddress;
                                        AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                        AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.StationInRange;
                                        AlarmInfo.FrameType = Frame.PropFrameType;
                                        AlarmInfo.TimeStamp = TS;
                                        AlarmInfo.LocId = Frame.SourceLoc;
                                        AlarmInfo.Tag = Frame.Tag;
                                        if (Frame.PropProbeRequestSSID != null)
                                        {
                                            AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        }
                                        Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Context.MaxRSSIAlarm != 0)
                            {
                                if (Frame.PropRadioTapRssi.AntennaSignalDbm > Context.MaxRSSIAlarm)
                                {
                                    AlarmInformation AlarmInfo = new AlarmInformation();
                                    AlarmInfo.FrameType = Frame.PropFrameType;
                                    AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                    AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                    AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                    AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.RadioTapRSSIMax;
                                    if (Frame.PropProbeRequestSSID != null)
                                    {
                                        AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                    }
                                    AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                    AlarmInfo.TimeStamp = TS;
                                    AlarmInfo.LocId = Frame.SourceLoc;
                                    AlarmInfo.Tag = Frame.Tag;
                                    Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                }
                            }
                            foreach (FrameControl.StationAlarms Alarm in Context.StationAlarmsList)
                            {
                                if (Context.MaxRSSIAlarm != 0)
                                {
                                    if (Frame.PropRadioTapRssi.AntennaSignalDbm > Context.MaxRSSIAlarm)
                                    {
                                        AlarmInformation AlarmInfo = new AlarmInformation();
                                        AlarmInfo.FrameType = Frame.PropFrameType;
                                        AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                        AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                        AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                        AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.RadioTapRSSIMax;
                                        if (Frame.PropProbeRequestSSID != null)
                                        {
                                            AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        }
                                        AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                        AlarmInfo.TimeStamp = TS;
                                        AlarmInfo.LocId = Frame.SourceLoc;
                                        AlarmInfo.Tag = Frame.Tag;
                                        Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                    }
                                }

                                if (Frame.PropSrcAddr.ToString() == Alarm.StationAddress)
                                {
                                    if (Alarm.InRange)
                                    {
                                        AlarmInformation AlarmInfo = new AlarmInformation();
                                        AlarmInfo.FrameType = Frame.PropFrameType;
                                        AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                        AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                        AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                        AlarmInfo.TargetStationAddress = Alarm.StationAddress;
                                        AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                        AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.StationInRange;
                                        if (Frame.PropProbeRequestSSID != null)
                                        {
                                            AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        }
                                        AlarmInfo.TimeStamp = TS;
                                        AlarmInfo.LocId = Frame.SourceLoc;
                                        AlarmInfo.Tag = Frame.Tag;
                                        Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                    }
                                    if (Alarm.RadioTapRSSIHigh)
                                    {
                                        if (Alarm.RadioTapRSSIValueHigh >= Frame.PropRadioTapRssi.AntennaSignalDbm)
                                        {
                                            AlarmInformation AlarmInfo = new AlarmInformation();
                                            AlarmInfo.FrameType = Frame.PropFrameType;
                                            AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                            AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                            AlarmInfo.TargetStationAddress = Alarm.StationAddress;
                                            AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                            AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.RadioTapRSSIHigh;
                                            AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                            AlarmInfo.RadioTapRssiDBMThreshold = Alarm.RadioTapRSSIValueHigh;
                                            AlarmInfo.TimeStamp = TS;
                                            AlarmInfo.LocId = Frame.SourceLoc;
                                            AlarmInfo.Tag = Frame.Tag;
                                            if (Frame.PropProbeRequestSSID != null)
                                            {
                                                AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                            }
                                            Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                        }
                                    }
                                    if (Alarm.RadioTapRSSILow)
                                    {
                                        if (Alarm.RadioTapRSSIValueLow <= Frame.PropRadioTapRssi.AntennaSignalDbm)
                                        {
                                            AlarmInformation AlarmInfo = new AlarmInformation();
                                            AlarmInfo.FrameType = Frame.PropFrameType;
                                            AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                            AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                            AlarmInfo.TargetStationAddress = Alarm.StationAddress;
                                            AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.StationAddress;
                                            AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.RadioTapRSSILow;
                                            AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                            AlarmInfo.RadioTapRssiDBMThreshold = Alarm.RadioTapRSSIValueLow;
                                            AlarmInfo.TimeStamp = TS;
                                            AlarmInfo.LocId = Frame.SourceLoc;
                                            AlarmInfo.Tag = Frame.Tag;
                                            if (Frame.PropProbeRequestSSID != null)
                                            {
                                                AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                            }
                                            Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                        }
                                    }
                                }
                            }
                            foreach (FrameControl.SSIDAlarms Alarm in Context.SSIDAlarmsList)
                            {
                                if (Frame.PropProbeRequestSSID == Alarm.SSID)
                                {
                                    if (Alarm.StationRequest)
                                    {
                                        AlarmInformation AlarmInfo = new AlarmInformation();
                                        AlarmInfo.FrameType = Frame.PropFrameType;
                                        AlarmInfo.Channel = Frame.RadioTapChannel.ToString();
                                        AlarmInfo.RadioTapRssiDBM = Frame.PropRadioTapRssi.AntennaSignalDbm;
                                        AlarmInfo.AlarmObject = AlarmInformation.AlarmPropertiesObject.NetworkSSID;
                                        AlarmInfo.AlarmProperties = AlarmInformation.AlarmPropertiesType.SSIDRequested;
                                        AlarmInfo.TargetNetworkSSID = Alarm.SSID;
                                        AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        AlarmInfo.RequestedStationAddress = Frame.PropSrcAddr.ToString();
                                        AlarmInfo.TimeStamp = TS;
                                        AlarmInfo.LocId = Frame.SourceLoc;
                                        if (Frame.PropProbeRequestSSID != null)
                                        {
                                            AlarmInfo.RequestedNetworkSSID = Frame.PropProbeRequestSSID;
                                        }
                                        Context.ActiveAlarms.CurrentAlarms.BeginInvoke(new Alarms.MyDelegate(Context.ActiveAlarms.AddAlarms), AlarmInfo);
                                    }
                                }
                            }
                        } 
                    }
                    AlarmCheckFrameList.Clear();
                }
            }
        }
        public void NewFrameReady()
        {
            while (Context.ProgramIsClosing == false)
            {
                IncomingFrameReadyWait.WaitOne();
                lock(IncoimingFrameListLock)
                {
                    foreach (var Frame in IncomingFrameList)
                    {
                        foreach(var tag in Context.StationTagList)
                        {
                            if (Frame.strSrcAddr == tag.MAC)
                            {
                                Frame.Tag = tag.Tag;
                            }
                            else if (Frame.strDstAddr == tag.MAC)
                            {
                                Frame.Tag = tag.Tag;
                            }

                        }
                        lock (DisplayFrameListLock)
                        {
                            DisplayFrameList.Add(Frame);
                        }
                        lock (AlarmCheckFrameListLock)
                        {
                            AlarmCheckFrameList.Add(Frame);
                        }
                        if (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ControlCTS)
                        {
                            if (Context.WatchList.IsStationOnWatchList(Frame.PropDstAddr.ToString()))
                            {
                                Context.WatchList.AddFrame(Frame);
                            }
                        }
                        else if ((Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse)
                                    || (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse))
                        {
                            if (Context.WatchList.IsStationOnWatchList(Frame.PropDstAddr.ToString()))
                            {
                                Context.WatchList.AddFrame(Frame);
                            }
                        }
                        else
                        {// requests type frames only in here
                            if (Context.WatchList.IsStationOnWatchList(Frame.PropSrcAddr.ToString()))
                            {
                                Context.WatchList.AddFrame(Frame);
                            }
                            if (Frame.PropProbeRequestSSID != null && Frame.PropProbeRequestSSID != string.Empty)
                            {
                                if (Context.WatchList.IsSSIDOnWatchList(Frame.PropProbeRequestSSID))
                                {
                                    Context.WatchList.AddFrameWithSSID(Frame);
                                }
                            }

                            Context.ProbeRequestors.BeginInvoke(new ProbeRequests.MyDelegate(Context.ProbeRequestors.UpdateProbeRequests), Frame);
                            

                        }
                    }
                    IncomingFrameList.Clear();
                }
                if (Context.ProgramIsClosing == false)
                {
                    MainList.BeginInvoke(new MyDelegate(PopulateFrameDisplay));
                    AlarmCheckReadyWait.Set();
                }
            }
        }
        public delegate void MyDelegate();
        public void PopulateFrameDisplay()
        {
            lock (DisplayFrameListLock)
            {
                MainList.BeginUpdate();
                foreach (RadioPlusMAC Frame in DisplayFrameList)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    sb.Append(Frame.PropSrcAddr.ToString() + "\t");
                    sb.Append(Frame.PropDstAddr.ToString() + "\t");
                    sb.Append(Frame.PropBSSID.ToString() + "\t");
                    sb.Append(Frame.PropRadioTapChannel.Channel.ToString() + "\t");
                    sb.Append(Frame.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");               
                    sb.Append(Frame.PropFrameType.ToString() + "\t\t");
                    sb.Append(Frame.TimeStamp.ToString() + "\t");
                    if (Frame.PropProbeRequestSSID != null)
                    { 
                        sb.Append(Frame.PropProbeRequestSSID.ToString());     
                    }
                    if (ScrollingEnabled)
                    {
                        if (MainList.Items.Count > 1000)
                        {
                            MainList.Items.RemoveAt(0); // remove first line
                        }
                    }
                    MainList.Items.Add(sb.ToString());
                    
                }
                if (ScrollingEnabled)
                {
                    MainList.SelectedIndex = MainList.Items.Count - 1;
                    MainList.ClearSelected();
                }
                MainList.EndUpdate();
                DisplayFrameList.Clear();
            }
        }

        private void MainList_MouseClick(object sender, MouseEventArgs e)
        {
            ScrollingEnabled = false;
            if (e.Button == MouseButtons.Right)
            {
                int index = MainList.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    MainList.SelectedItem = index;
                    MainListContextMenu.Show(); 
                    var FrameInfo = MainList.Items[index];
                }
            }            
        }

        private void MainList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = MainList.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var FrameInfo = MainList.Items[index];
            }
        }

        private void Monitor_Load(object sender, EventArgs e)
        {
            MainListContextMenu = new ContextMenuStrip();
            MainListContextMenu.Opening += new CancelEventHandler(MainListContextMenu_Opening);
            MainList.ContextMenuStrip = MainListContextMenu;
            ToolStripMenuItem AddWatchList = new ToolStripMenuItem("Add Address To Watch List");
            AddWatchList.Click += new EventHandler(AddWatchList_Click);
            ToolStripMenuItem ShowStaInfo = new ToolStripMenuItem("Show Station Info");
            ShowStaInfo.Click += new EventHandler(ShowStaInfo_Click);
            ToolStripMenuItem Scrolling = new ToolStripMenuItem("Resume Scrolling");
            Scrolling.Click += new EventHandler(Scrolling_Click);
            MainListContextMenu.Items.AddRange(new ToolStripItem[] { AddWatchList, ShowStaInfo, Scrolling });
           
           
        }
        public void Scrolling_Click(object sender, EventArgs e)
        {
            ScrollingEnabled = true;
            MainList.SelectedIndex = MainList.Items.Count - 1;
            MainList.ClearSelected();
        }
        public void AddWatchList_Click(object sender, EventArgs e)
        {
            AddToWatchList AddAddrBox = new AddToWatchList(Context);
            AddAddrBox.Show();
            AddAddrBox.SelectStationAddress(MainList.SelectedItem);
            ScrollingEnabled = true;
        }
        public void ShowStaInfo_Click(object sender, EventArgs e)
        {
            ScrollingEnabled = true;
        }
        private void MainListContextMenu_Opening(object sender, CancelEventArgs e)
        {
            
        }
        private void WatchListbtn_Click(object sender, EventArgs e)
        {
            Context.WatchList.Show();
            Context.WatchList.Activate();
        }

        private void EditWatchListBtn_Click(object sender, EventArgs e)
        {
            EditWatchList EditList = new EditWatchList(Context);
            EditList.Show();
            EditList.UpdateWatchLists();
        }

        private void EditAlarmsBtn_Click(object sender, EventArgs e)
        {
            AddAlarms AlarmList = new AddAlarms(Context);
            AlarmList.Show();
            AlarmList.UpdateAlarmLists();
        }

        private void Monitor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                e.SuppressKeyPress = true;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context.ActiveAlarms.Show();
            Context.ActiveAlarms.Activate();

        }

        private void ProbeRequestsBtn_Click(object sender, EventArgs e)
        {
            Context.ProbeRequestors.Show();
            Context.ProbeRequestors.Activate();
        }
    }
}
