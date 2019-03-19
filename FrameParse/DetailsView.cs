using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace ProbeMon
{
    public partial class DetailsView : Form
    {
        public volatile FrameControl Context;
        public DetailsView(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }
        public void DisplayStationDetails(string StationAddress)
        {
            List<RadioPlusMAC> StaDetailsList = null;
            PhysicalAddress Addr = System.Net.NetworkInformation.PhysicalAddress.Parse(StationAddress);
            bool StationPresent = false;
            lock (Context.WatchList.StationAddrListLock)
            {
                int index = Context.WatchList.StationAddrIndex.IndexOf(Addr);
                if (index >= 0)
                {
                    StaDetailsList = Context.WatchList.StationAddrIndexableData.ElementAt(index);
                    StationPresent = true;
                    
                }
            }
            if (StationPresent)
            {
                ShowStationDetails(StaDetailsList, StationAddress);
            }
        }
        public void DisplaySSIDDetails(string SSID)
        {
            List<RadioPlusMAC> SSIDList = null;
            bool StationPresent = false;
            lock (Context.WatchList.SSIDListLock)
            {
                int index = Context.WatchList.SSIDListIndex.IndexOf(SSID);
                if (index >= 0)
                {
                    SSIDList = Context.WatchList.SSIDListIndexable.ElementAt(index);
                    StationPresent = true;
                }
            }
            if (StationPresent)
            {
                ShowSSIDDetails(SSIDList);
            }
        }
        delegate void SetTextCallback2(List<RadioPlusMAC> SSIDList);
        public void ShowSSIDDetails(List<RadioPlusMAC> SSIDList)
        {
            if (DetailedView.InvokeRequired)
            {
                SetTextCallback2 d = new SetTextCallback2(ShowSSIDDetails);
                DetailedView.Invoke(d, new object[] { SSIDList });
            }
            else
            {
                DetailedView.BeginUpdate();
                DetailedView.Items.Clear();
                lock (Context.WatchList.StationAddrListLock)
                {
                    if (SSIDList == null) { return; }
                    List<RadioPlusMAC> ReverseList = SSIDList.ToList();
                    ReverseList.Reverse();  
                    foreach (RadioPlusMAC Frame in ReverseList)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Clear();     
                        sb.Append(Frame.PropSrcAddr.ToString() + "\t");
                        sb.Append(Frame.PropDstAddr.ToString() + "\t");
                        sb.Append(Frame.PropBSSID.ToString() + "\t");
                        sb.Append(Frame.PropRadioTapChannel.Channel.ToString() + "\t");
                        sb.Append(Frame.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");
                        sb.Append(Frame.TimeStamp.ToString() + "\t");
                        sb.Append(Frame.PropFrameType.ToString() + "\t\t");
                        if (Frame.PropProbeRequestSSID != null)
                        {
                            sb.Append(Frame.PropProbeRequestSSID.ToString());
                        }
                        DetailedView.Items.Add(sb.ToString());
                    }                    
                }
                DetailedView.EndUpdate();
            }
        }
        delegate void SetTextCallback(List<RadioPlusMAC> DetailsList, string StationAddress);
        public void ShowStationDetails(List<RadioPlusMAC> DetailsList, string StationAddress)
        {
            if (DetailedView.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ShowStationDetails);
                DetailedView.Invoke(d, new object[] { DetailsList });
            }
            else
            {
                DetailedView.BeginUpdate();
                DetailedView.Items.Clear();
                lock (Context.WatchList.StationAddrListLock)
                {
                    if (DetailsList == null) { return; }
                    List<RadioPlusMAC> ReverseList = DetailsList.ToList();
                    ReverseList.Reverse();
                    foreach (RadioPlusMAC Frame in ReverseList)
                    {        
                        StringBuilder sb = new StringBuilder();
                        sb.Clear();
                        sb.Append(StationAddress.ToString() + "\t");
                        sb.Append(Frame.PropSrcAddr.ToString() + "\t");
                        sb.Append(Frame.PropDstAddr.ToString() + "\t");
                        sb.Append(Frame.PropBSSID.ToString() + "\t");
                        sb.Append(Frame.PropRadioTapChannel.Channel.ToString() + "\t");
                        sb.Append(Frame.PropRadioTapRssi.AntennaSignalDbm.ToString() + "\t");                        
                        sb.Append(Frame.TimeStamp.ToString() + "\t");
                        if (Frame.Tag != null)
                        {
                            sb.Append(Frame.Tag.ToString() + "\t");
                        }
                        sb.Append(Frame.PropFrameType.ToString() + "\t\t");
                        if (Frame.PropProbeRequestSSID != null)
                        {
                            sb.Append(Frame.PropProbeRequestSSID.ToString());
                        }

                        DetailedView.Items.Add(sb.ToString());                        
                    } 
                }
                DetailedView.EndUpdate();
            }
        }
    }
}
