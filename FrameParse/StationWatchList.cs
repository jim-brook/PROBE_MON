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
using System.Net.NetworkInformation;

namespace ProbeMon
{
    public partial class StationWatchList : Form
    {
        public ContextMenuStrip AddrWatchListContextMenu;
        public ContextMenuStrip SSIDWatchListContextMenu;
        public FrameControl Context;
        public List<string> SSIDListIndex;
        public List<List<RadioPlusMAC>> SSIDListIndexable;
        public object SSIDListLock = new object();
        public List<PhysicalAddress> StationAddrIndex;
        public List<List<RadioPlusMAC>> StationAddrIndexableData;
        public object StationAddrListLock = new object();
        public EventWaitHandle UpdateUIWait;
        public Thread UpdateUIThread;
        public StationWatchList(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
            StationAddrIndex = new List<PhysicalAddress>();
            StationAddrIndexableData = new List<List<RadioPlusMAC>>();
            SSIDListIndex = new List<string>();
            SSIDListIndexable = new List<List<RadioPlusMAC>>();
            UpdateUIWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            UpdateUIThread = new Thread(UpdateUIBigLoop);
            UpdateUIThread.Start();
        }
        public void UpdateUIBigLoop()
        {
            while (Context.ProgramIsClosing == false)
            {
                UpdateUIWait.WaitOne();
                if (Context.ProgramIsClosing == true) { return; }
                AddrWatchList.BeginInvoke(new MyDelegate(PopulateAddrWatchList));
                SSIDWatchList.BeginInvoke(new MyDelegate2(PopulateSSIDWatchList));
            }

        }
        public delegate void MyDelegate();
        public void PopulateAddrWatchList()
        {
            int Index = 0;
            lock (StationAddrListLock)
            {
                AddrWatchList.BeginUpdate();
                AddrWatchList.Items.Clear();
                foreach (List<RadioPlusMAC> DataAddrList in StationAddrIndexableData)
                {
                    List<RadioPlusMAC> ReverseList = DataAddrList.ToList();
                    ReverseList.Reverse();
                    PhysicalAddress Address = StationAddrIndex.ElementAt(Index);
                    string TargAddress = Address.ToString();
                    RadioPlusMAC LatestFrame = ReverseList.ElementAt(0);


                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    sb.Append(TargAddress.ToString() + "\t");
                    sb.Append(LatestFrame.PropSrcAddr.ToString() + "\t");
                    sb.Append(LatestFrame.PropDstAddr.ToString() + "\t");
                    sb.Append(LatestFrame.PropBSSID.ToString() + "\t");
                    sb.Append(LatestFrame.PropRadioTapChannel.Channel.ToString() + "\t");
                    sb.Append(LatestFrame.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");            
                    sb.Append(ReverseList.Count.ToString() + "\t");
                    sb.Append(LatestFrame.TimeStamp.ToString() + "\t");
                    sb.Append(LatestFrame.PropFrameType.ToString() + "\t\t");
                    if (LatestFrame.Tag != null)
                    {
                        sb.Append(LatestFrame.Tag.ToString() + "\t");
                    }
                    if (LatestFrame.PropProbeRequestSSID != null)
                    {
                        sb.Append(LatestFrame.PropProbeRequestSSID.ToString());
                    }

                    AddrWatchList.Items.Add(sb.ToString());
                    Index++;
                }
                AddrWatchList.EndUpdate(); 
            }
        }
        public delegate void MyDelegate2();
        public void PopulateSSIDWatchList()
        {
            int Index = 0;
            lock (SSIDListLock)
            {
                SSIDWatchList.BeginUpdate();
                SSIDWatchList.Items.Clear();

                foreach (List<RadioPlusMAC> DataSSIDList in SSIDListIndexable)
                {
                    List<RadioPlusMAC> ReverseList = DataSSIDList.ToList();
                    ReverseList.Reverse();
                    string TargSSID = SSIDListIndex.ElementAt(Index);
                    RadioPlusMAC LatestFrame = ReverseList.ElementAt(0);

                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    //sb.Append(TargSSID.ToString() + "\t");
                    sb.Append(LatestFrame.PropSrcAddr.ToString() + "\t");
                    sb.Append(LatestFrame.PropDstAddr.ToString() + "\t");
                    sb.Append(LatestFrame.PropBSSID.ToString() + "\t");
                    sb.Append(LatestFrame.PropRadioTapChannel.Channel.ToString() + "\t");
                    sb.Append(LatestFrame.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");                
                    sb.Append(ReverseList.Count.ToString() + "\t");
                    sb.Append(LatestFrame.TimeStamp.ToString() + "\t");
                    sb.Append(LatestFrame.PropFrameType.ToString() + "\t\t");
                    if (LatestFrame.PropProbeRequestSSID != null)
                    {
                        sb.Append(LatestFrame.PropProbeRequestSSID.ToString());
                    }

                    SSIDWatchList.Items.Add(sb.ToString());
                    Index++;
                }
                SSIDWatchList.EndUpdate(); 

            }

        }
        public void AddStationAddress(string StationAddress)
        {
            PhysicalAddress Addr = System.Net.NetworkInformation.PhysicalAddress.Parse(StationAddress);
            if (!IsStationOnWatchList(StationAddress))
            {
                Context.StationWatchList.Add(StationAddress);
            }
        }
        public void RemoveStationAddress(string StationAddress)
        {
            PhysicalAddress Addr = System.Net.NetworkInformation.PhysicalAddress.Parse(StationAddress);
            Context.StationWatchList.Remove(StationAddress);
   
            int Index = 0;
            Index = StationAddrIndex.IndexOf(Addr);
            if (Index >= 0)
            {                
                StationAddrIndex.Remove(Addr);
                StationAddrIndexableData.RemoveAt(Index);
            }
        }
        public void AddSSID(string SSID)
        {
            if (SSID != null && SSID != string.Empty)
            {
                if (!IsSSIDOnWatchList(SSID))
                {
                    Context.SSIDWatchList.Add(SSID);
                }
            }
        }
        public void RemoveSSID(string SSID)
        {
            int Index = 0;
            Context.SSIDWatchList.Remove(SSID);
            Index = SSIDListIndex.IndexOf(SSID);
            if (Index >= 0)
            {
                SSIDListIndex.Remove(SSID);
                SSIDListIndexable.RemoveAt(Index);               

            }
        }
        public bool IsStationOnWatchList(string StationAddress)
        {
            return Context.StationWatchList.Contains(StationAddress);
        }
        public bool IsSSIDOnWatchList(string SSID)
        {
            return Context.SSIDWatchList.Contains(SSID);
        }
        public void AddFrameWithSSID(RadioPlusMAC Frame)
        {
            if ((Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse)
                    || (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse))
            {
                return;
            }
            else
            {
                lock (SSIDListLock)
                {
                    int index = SSIDListIndex.IndexOf(Frame.PropProbeRequestSSID);
                    if (index >= 0)
                    {
                        List<RadioPlusMAC> SSIDList = SSIDListIndexable.ElementAt(index);
                        if (SSIDList.Count > 1000) { SSIDList.Clear(); }        //1,000 is just arbitrary for now until MAC exclusion
                        try
                        {
                            SSIDList.Add(Frame);
                        }
                        catch (System.OutOfMemoryException Ex)
                        {
                            SSIDList.Clear();
                            Console.WriteLine("SSIDListIndex is full!", Ex.Message.ToString());
                        }

                    }
                    else
                    {
                        SSIDListIndex.Add(Frame.PropProbeRequestSSID);
                        List<RadioPlusMAC> SSIDList = new List<RadioPlusMAC>();
                        SSIDList.Add(Frame);
                        SSIDListIndexable.Add(SSIDList);

                    }
                }
            }
            UpdateUIWait.Set();
        }
        public void AddFrame(RadioPlusMAC Frame)
        {
            //Is this a response? Then Work based on the dest addr instead
            if ((Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementProbeResponse)
                    || (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ManagementAssociationResponse)
                        || (Frame.PropFrameType == PacketDotNet.Ieee80211.FrameControlField.FrameSubTypes.ControlCTS))
            {
                
                lock (StationAddrListLock)
                {
                    int index = StationAddrIndex.IndexOf(Frame.PropDstAddr);
                    if (index >= 0)
                    {
                        List<RadioPlusMAC> StaDataList = StationAddrIndexableData.ElementAt(index);
                        if (StaDataList.Count > 1000) { StaDataList.Clear(); }        //1,000 is just arbitrary for now until MAC exclusion
                        try
                        {
                            StaDataList.Add(Frame);
                        }
                        catch (System.OutOfMemoryException Ex)
                        {
                            StaDataList.Clear();
                            Console.WriteLine("StaDataList is full!", Ex.Message.ToString());
                        }

                    }
                    else
                    {
                        StationAddrIndex.Add(Frame.PropDstAddr);
                        List<RadioPlusMAC> StaDataList = new List<RadioPlusMAC>();
                        StaDataList.Add(Frame);
                        StationAddrIndexableData.Add(StaDataList);

                    }
                }
            }
            else
            { 
                lock (StationAddrListLock)
                {
                    int index = StationAddrIndex.IndexOf(Frame.PropSrcAddr);
                    if (index >= 0)
                    {
                        List<RadioPlusMAC> StaDataList = StationAddrIndexableData.ElementAt(index);
                        if (StaDataList.Count > 1000) { StaDataList.Clear(); }         //1,000 is just arbitrary for now until MAC exclusion
                        try
                        {
                            StaDataList.Add(Frame);
                        }
                        catch (System.OutOfMemoryException Ex)
                        {
                            StaDataList.Clear();
                            Console.WriteLine("StaDataList is full!", Ex.Message.ToString());
                        }

                    }
                    else
                    {
                        StationAddrIndex.Add(Frame.PropSrcAddr);
                        List<RadioPlusMAC> StaDataList = new List<RadioPlusMAC>();
                        StaDataList.Add(Frame);
                        StationAddrIndexableData.Add(StaDataList);

                    }
                }
            }
            UpdateUIWait.Set();
        }

        private void StationWatchList_Load(object sender, EventArgs e)
        {
            AddrWatchListContextMenu = new ContextMenuStrip();
            AddrWatchListContextMenu.Opening += new CancelEventHandler(AddrWatchListContextMenu_Opening);
            AddrWatchList.ContextMenuStrip = AddrWatchListContextMenu;
            ToolStripMenuItem AddrWatchListPktDetails = new ToolStripMenuItem("Show Station Details");
            AddrWatchListPktDetails.Click += new EventHandler(AddrWatchListPktDetail_Click);
            ToolStripMenuItem AddrWatchListAddAlarm = new ToolStripMenuItem("Add Alarms");
            AddrWatchListAddAlarm.Click += new EventHandler(AddrWatchListAddAlarm_Click);
            ToolStripMenuItem AddrWatchListRemove = new ToolStripMenuItem("Remove From List");
            AddrWatchListRemove.Click += new EventHandler(AddrWatchListRemove_Click);
            AddrWatchListContextMenu.Items.AddRange(new ToolStripItem[] { AddrWatchListPktDetails, AddrWatchListAddAlarm, AddrWatchListRemove });

            SSIDWatchListContextMenu = new ContextMenuStrip();
            SSIDWatchListContextMenu.Opening += new CancelEventHandler(SSIDWatchListContextMenu_Opening);
            SSIDWatchList.ContextMenuStrip = SSIDWatchListContextMenu;
            ToolStripMenuItem SSIDWatchListFrameDetails = new ToolStripMenuItem("Show SSID Details");
            SSIDWatchListFrameDetails.Click += new EventHandler(SSIDWatchListFrameDetails_Click);
            ToolStripMenuItem SSIDWatchListAddAlarm = new ToolStripMenuItem("Add Alarms");
            SSIDWatchListAddAlarm.Click += new EventHandler(SSIDWatchListAddAlarm_Click);
            ToolStripMenuItem SSIDWatchListRemove = new ToolStripMenuItem("Remove From List");
            SSIDWatchListRemove.Click += new EventHandler(SSIDWatchListRemove_Click);
            SSIDWatchListContextMenu.Items.AddRange(new ToolStripItem[] { SSIDWatchListFrameDetails, SSIDWatchListAddAlarm, SSIDWatchListRemove });          
        }
        public void SSIDWatchListRemove_Click(object sender, EventArgs e)
        {
            if (SSIDWatchList.SelectedItem == null) { return; }
            RemoveSSID(GetSSIDFromString(SSIDWatchList.SelectedItem.ToString()));
            SSIDWatchList.Items.RemoveAt(SSIDWatchList.SelectedIndex);
            SSIDWatchList.ClearSelected();
        }
        public void SSIDWatchListFrameDetails_Click(object sender, EventArgs e)
        {
            if (SSIDWatchList.SelectedItem == null) { return; }
            DetailsView StationDetails = new DetailsView(Context);
            StationDetails.Show();
            StationDetails.DisplaySSIDDetails(GetSSIDFromString(SSIDWatchList.SelectedItem.ToString()));
            SSIDWatchList.ClearSelected();
        }
        public void SSIDWatchListAddAlarm_Click(object sender, EventArgs e)
        {
            AddAlarms AlarmList = new AddAlarms(Context);
            AlarmList.Show();
            AlarmList.UpdateAlarmLists();
            SSIDWatchList.ClearSelected();
        }
        private void SSIDWatchListContextMenu_Opening(object sender, CancelEventArgs e)
        {
            
        }
        public void AddrWatchListRemove_Click(object sender, EventArgs e)
        {
            if (AddrWatchList.SelectedItem == null) { return; }
            RemoveStationAddress(GetSrcAddr(AddrWatchList.SelectedItem.ToString()));
            AddrWatchList.Items.RemoveAt(AddrWatchList.SelectedIndex);
            AddrWatchList.ClearSelected();
        }
        public void AddrWatchListPktDetail_Click(object sender, EventArgs e)
        {
            if (AddrWatchList.SelectedItem == null) { return; }
            DetailsView StationDetails = new DetailsView(Context);
            StationDetails.Show();
            StationDetails.DisplayStationDetails(GetSrcAddr(AddrWatchList.SelectedItem.ToString()));
            AddrWatchList.ClearSelected();
        }
        public void AddrWatchListAddAlarm_Click(object sender, EventArgs e)
        {
            AddAlarms AlarmList = new AddAlarms(Context);
            AlarmList.Show();
            AlarmList.UpdateAlarmLists();
            AddrWatchList.ClearSelected();
        }
        private void AddrWatchListContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }
        public string GetSrcAddr(string Data)
        {
            int FieldEnd;
            string Item = Data;
            FieldEnd = Item.IndexOf('\t');
            char[] SourceAdd = new char[FieldEnd];
            Item.CopyTo(0, SourceAdd, 0, FieldEnd);
            string SrcAddr = new string(SourceAdd);

            return SrcAddr;

        }
        public string GetSSIDFromString(string Data)
        {
            int FieldEnd;
            string Item = Data;
            if (Item == null || Item == string.Empty) { return ""; }
            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');;
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);

            FieldEnd = Item.IndexOf('\t');
            Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.IndexOf('\t');
            Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.IndexOf('\t');
            Item = Item.Remove(0, FieldEnd + 1);
            FieldEnd = Item.Length;
            char[] ssid = new char[FieldEnd];
            Item.CopyTo(0, ssid, 0, FieldEnd);
            string SSID = new string(ssid);


            return SSID;

        }
        private void AddrWatchList_MouseClick(object sender, MouseEventArgs e)
        {
            lock (StationAddrListLock)
            {
                if (e.Button == MouseButtons.Right)
                {
                    int index = AddrWatchList.IndexFromPoint(e.Location);
                    if (index != ListBox.NoMatches)
                    {
                        AddrWatchList.SelectedItem = index;
                        AddrWatchListContextMenu.Show();
                        var FrameInfo = AddrWatchList.Items[index];
                    }
                }
            }
        }

        private void SSIDWatchList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = SSIDWatchList.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    SSIDWatchList.SelectedItem = index;
                    SSIDWatchListContextMenu.Show();
                    var FrameInfo = SSIDWatchList.Items[index];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void StationWatchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                e.SuppressKeyPress = true;

            }
        }
    }
}
