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
    public partial class ProbeRequests : Form
    {
        public ContextMenuStrip MainListContextMenu;
        public FrameControl Context;
        public List<RadioPlusMAC> ProbeRequestsList = new List<RadioPlusMAC>();
        public object ProbeRequestsListLock = new object();
        public System.Windows.Forms.Timer CleanUpTimer = new System.Windows.Forms.Timer();
        public bool PuaseDisplay;
        public ProbeRequests(FrameControl FC)
        {
            InitializeComponent();
            Context = FC; 
            CleanUpTimer.Interval = 5000;
            CleanUpTimer.Tick += TimerTicFunc;
            PuaseDisplay = false;
        }
        public void TimerTicFunc(object sender, EventArgs e)
        {
            if (Context.ProgramIsClosing == true) { return; }
            List<RadioPlusMAC> WorkingList = null;
            lock (ProbeRequestsListLock)
            {
                WorkingList = ProbeRequestsList.ToList();
            }
            
            foreach (RadioPlusMAC Frame in WorkingList)
            {
                TimeSpan TimeDiff = DateTime.Now - Frame.TimeStamp;
                if (TimeDiff.TotalMinutes >= 3)
                {
                    lock (ProbeRequestsListLock)
                    {
                        ProbeRequestsList.Remove(Frame);
                    }
                }
            }
        }
    
        public delegate void MyDelegate(RadioPlusMAC RequestFrame);
        public void UpdateProbeRequests(RadioPlusMAC RequestFrame)
        {
            bool RecordFound = false;
            RadioPlusMAC FoundFrame = null;
            if (Context.ProgramIsClosing == true) { return; }
            lock (ProbeRequestsListLock)
            {
                foreach (RadioPlusMAC Frame in ProbeRequestsList)
                {
                    if (Frame.strSrcAddr == RequestFrame.strSrcAddr)
                    {
                        FoundFrame = Frame;
                        RecordFound = true;
                        break;
                    }
                }
            }
            if (RecordFound)
            {
                ProbeRequestsList.Remove(FoundFrame);
                RequestFrame.Count = FoundFrame.Count;
                RequestFrame.Count++;
                ProbeRequestsList.Add(RequestFrame);
            }
            else
            {
                RequestFrame.Count++;
                ProbeRequestsList.Add(RequestFrame);
            }
            PopulateRequestFramesList();
        }
        public void PopulateRequestFramesList()
        {
            if (PuaseDisplay == true)
            {
                return;
            }
            List<RadioPlusMAC> ReverseList = null;
            lock (ProbeRequestsListLock)
            {
                ReverseList = ProbeRequestsList.ToList();
            }
            RequestFramesList.BeginUpdate();
            RequestFramesList.Items.Clear();
            ReverseList.Reverse();

            foreach (RadioPlusMAC Request in ReverseList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(Request.Count.ToString() + "\t");
                sb.Append(Request.PropSrcAddr.ToString() + "\t");
                sb.Append(Request.PropDstAddr.ToString() + "\t");
                sb.Append(Request.PropBSSID.ToString() + "\t");
                sb.Append(Request.PropRadioTapChannel.Channel.ToString() + "\t");
                sb.Append(Request.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");
                sb.Append(Request.PropFrameType.ToString() + "\t\t");
                sb.Append(Request.TimeStamp.ToString() + "\t");
                if (Request.PropProbeRequestSSID != null)
                {
                    sb.Append(Request.PropProbeRequestSSID.ToString());
                }
                RequestFramesList.Items.Add(sb.ToString());
            }
            RequestFramesList.EndUpdate();

        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ProbeRequests_Load(object sender, EventArgs e)
        {
            MainListContextMenu = new ContextMenuStrip();
            RequestFramesList.ContextMenuStrip = MainListContextMenu;
            ToolStripMenuItem AddWatchList = new ToolStripMenuItem("Add Address To Watch List");
            AddWatchList.Click += new EventHandler(AddWatchList_Click);        
            MainListContextMenu.Items.AddRange(new ToolStripItem[] { AddWatchList });
        }
        public void AddWatchList_Click(object sender, EventArgs e)
        {
            string Item = null;
            if (RequestFramesList.SelectedItem != null)
            {
                int FieldEnd;
                Item = RequestFramesList.SelectedItem as string;
                if (Item == null || Item == string.Empty) { return; }
                FieldEnd = Item.IndexOf('\t');
                Item = Item.Remove(0, FieldEnd + 1);

            }            
            AddToWatchList AddAddrBox = new AddToWatchList(Context);
            AddAddrBox.Show();
            AddAddrBox.SelectStationAddress(Item);


        }

        private void ProbeRequests_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                e.SuppressKeyPress = true;

            }
        }

        private void PauseResumeBtn_Click(object sender, EventArgs e)
        {
            if (PuaseDisplay == false)
            {
                PauseResumeBtn.Text = "Resume";
                PuaseDisplay = true;
            }
            else
            {
                PauseResumeBtn.Text = "Pause";
                PuaseDisplay = false;
            }

        }
    }
}
