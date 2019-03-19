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
    public partial class SiteSelection : Form
    {
        Launcher Context;
        public SiteSelection(Launcher FC)
        {
            InitializeComponent();
            Context = FC;
            SelectedAlarmSite.SelectedIndex = 0;
        }

        private void SiteSelectedBtn_Click(object sender, EventArgs e)
        {
            if (SelectedAlarmSite.SelectedIndex != -1)
            {
                string Item = SelectedAlarmSite.SelectedItem as string;
                uint Site = 0;
                if (UInt32.TryParse(Item, out Site))
                {
                    AddAlarms AlarmList = new AddAlarms(Context, Site);
                    AlarmList.Show();
                    AlarmList.UpdateAlarmLists();
                    this.Close();
                } 
            }
        }
    }
}
