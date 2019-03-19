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
    public partial class AddNewSSIDAlarm : Form
    {
        public FrameControl Context;
        public AddAlarms ListBoxContext;
        public AddNewSSIDAlarm(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }

        public void SetSSID(string SSID, AddAlarms RefreshListBoxContext)
        {
            NetworkSSID.Text = SSID;
            ListBoxContext = RefreshListBoxContext;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            FrameControl.SSIDAlarms Alarm = new FrameControl.SSIDAlarms();
            Alarm.SSID = NetworkSSID.Text;
            Alarm.StationRequest = SetSSIDRequestsAlarm.Checked;
            Context.SSIDAlarmsList.Add(Alarm);
            ListBoxContext.UpdateAlarmLists();
            this.Close();
        }
    }
}
