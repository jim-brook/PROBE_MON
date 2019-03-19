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
    public partial class AddAlarms : Form
    {
        public FrameControl Context;
        public AddAlarms(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }
        public void UpdateAlarmLists()
        {
            StationAddressWatchList.BeginUpdate();
            StationAddressWatchList.Items.Clear();
            foreach (string StaAddr in Context.StationWatchList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(StaAddr);
                StationAddressWatchList.Items.Add(sb.ToString());
            }
            StationAddressWatchList.EndUpdate();

            SSIDWatchList.BeginUpdate();
            SSIDWatchList.Items.Clear();
            foreach (string SSID in Context.SSIDWatchList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(SSID);
                SSIDWatchList.Items.Add(sb.ToString());
            }
            SSIDWatchList.EndUpdate();
            AlarmAddressList.BeginUpdate();
            AlarmAddressList.Items.Clear();
            foreach (FrameControl.StationAlarms Alarm in Context.StationAlarmsList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(Alarm.StationAddress);
                AlarmAddressList.Items.Add(sb.ToString());
            }
            AlarmAddressList.EndUpdate();

            AlarmSSIDList.BeginUpdate();
            AlarmSSIDList.Items.Clear();
            foreach (FrameControl.SSIDAlarms Alarm in Context.SSIDAlarmsList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(Alarm.SSID);
                AlarmSSIDList.Items.Add(sb.ToString());
            }
            AlarmSSIDList.EndUpdate();
        } 

        private void ManualAddToStaWatchList_Click(object sender, EventArgs e)
        {
            string Item = "";
            if ((StaAddrToAdd.Text == null) || (StaAddrToAdd.Text == string.Empty))
            {
                if (StationAddressWatchList.SelectedItem != null)
                {
                    Item = StationAddressWatchList.SelectedItem as string;
                }
                if (Item.Length != 12) { return; }
            }
            else
            {
                Item = StaAddrToAdd.Text.ToString().ToUpper();
                if (Item.Length != 12) { return; }
            }
            AddNewStationAddrAlarm AddAlarm = new AddNewStationAddrAlarm(Context);
            AddAlarm.Show();
            AddAlarm.SetStationAddress(Item, this);           
        }

        private void ManualAddToSSIDWatchList_Click(object sender, EventArgs e)
        {
            string Item = "";
            if ((SSIDToAdd.Text == null) || (SSIDToAdd.Text == string.Empty))
            {
                if (SSIDWatchList.SelectedItem != null)
                {
                    Item = SSIDWatchList.SelectedItem as string;
                }
                else
                {
                    return;
                }
            }
            else
            {
                Item = SSIDToAdd.Text;
            }
            AddNewSSIDAlarm AddAlarm = new AddNewSSIDAlarm(Context);
            AddAlarm.Show();
            AddAlarm.SetSSID(Item, this);
        }

        private void RemoveStationAlarmBtn_Click(object sender, EventArgs e)
        {
            bool AlarmFound = false;
            string Item = AlarmAddressList.SelectedItem as string;
            FrameControl.StationAlarms TargetAlarm = new FrameControl.StationAlarms();
            if (AlarmAddressList.SelectedItem != null)
            {
                AlarmAddressList.Items.RemoveAt(AlarmAddressList.SelectedIndex);
                AlarmAddressList.ClearSelected();
                foreach(FrameControl.StationAlarms Alarm in Context.StationAlarmsList)
                {
                    if(Alarm.StationAddress == Item)
                    {
                        TargetAlarm = Alarm;
                        AlarmFound = true;
                        break;
                    }
                }
                if (AlarmFound)
                {
                    Context.StationAlarmsList.Remove(TargetAlarm);
                }
            }
        }

        private void RemoveSSIDAlarm_Click(object sender, EventArgs e)
        {
            bool AlarmFound = false;
            string Item = AlarmSSIDList.SelectedItem as string;
            FrameControl.SSIDAlarms TargetAlarm = new FrameControl.SSIDAlarms();
            if (AlarmSSIDList.SelectedItem != null)
            {
                AlarmSSIDList.Items.RemoveAt(AlarmSSIDList.SelectedIndex);
                AlarmSSIDList.ClearSelected();
                foreach (FrameControl.SSIDAlarms Alarm in Context.SSIDAlarmsList)
                {
                    if (Alarm.SSID == Item)
                    {
                        TargetAlarm = Alarm;
                        AlarmFound = true;
                        break;
                    }
                }
                if (AlarmFound)
                {
                    Context.SSIDAlarmsList.Remove(TargetAlarm);
                }
            }
        }

        private void AddMaxRSSI_Click(object sender, EventArgs e)
        {
            if(MaxRssi.SelectedIndex != -1)
            {
                string Item = MaxRssi.SelectedItem as string;
                sbyte rssi = 0;
                if (sbyte.TryParse(Item, out rssi))
                {
                    Context.MaxRSSIAlarm = rssi;
                }
            }
        }
    }
}
