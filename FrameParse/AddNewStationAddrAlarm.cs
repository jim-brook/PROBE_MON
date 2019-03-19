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
    public partial class AddNewStationAddrAlarm : Form
    {
        public FrameControl Context;
        public AddAlarms ListBoxContext;
        public AddNewStationAddrAlarm(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;

        }  

        private void AddBtn_Click(object sender, EventArgs e)
        {
            FrameControl.StationAlarms Alarm = new FrameControl.StationAlarms();
            Alarm.InRange = SetInRangeAlarm.Checked;
            Alarm.StationAddress = StaAddr.Text;
            Alarm.RadioTapRSSIHigh = SetRssiMax.Checked;
            if (Alarm.RadioTapRSSIHigh)
            {
                if (MaxRssi.Text.Length > 0)
                {
                    bool Sts = sbyte.TryParse(MaxRssi.Text, out Alarm.RadioTapRSSIValueHigh);    //would fail if not setting rssi alarm, doesnt matter 
                    if (!Sts) { return; }
                }
            }
            Alarm.RadioTapRSSILow = SetRssiMin.Checked;
            if (Alarm.RadioTapRSSILow)
            {
                if (MinRssi.Text.Length > 0)
                {
                    bool Sts = sbyte.TryParse(MinRssi.Text, out Alarm.RadioTapRSSIValueLow);
                    if (!Sts) { return; }
                }
            }
            Context.StationAlarmsList.Add(Alarm);
            ListBoxContext.UpdateAlarmLists();
            this.Close();

        }
        public void SetStationAddress(string StationAddress, AddAlarms RefreshListBoxContext)
        {
            StaAddr.Text = StationAddress;
            ListBoxContext = RefreshListBoxContext;
        }
    }
}
