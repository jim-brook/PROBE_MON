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
    public partial class EditWatchList : Form
    {
        public volatile FrameControl Context;
        public EditWatchList(FrameControl FC)
        {
            InitializeComponent();
            Context = FC;
        }
        public void UpdateWatchLists()
        {
            StaAddrList.BeginUpdate();
            StaAddrList.Items.Clear();
            foreach (string StaAddr in Context.StationWatchList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(StaAddr);
                StaAddrList.Items.Add(sb.ToString());
            }
            StaAddrList.EndUpdate();

            SSIDList.BeginUpdate();
            SSIDList.Items.Clear();
            foreach (string SSID in Context.SSIDWatchList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(SSID);
                SSIDList.Items.Add(sb.ToString());
            }
            SSIDList.EndUpdate();
            UpdateTagLists();
        }

        private void RemoveFromWatchList_Click(object sender, EventArgs e)
        {
            if (StaAddrList.SelectedItem == null) { return; }
            string Item = StaAddrList.SelectedItem as string;
            StaAddrList.Items.RemoveAt(StaAddrList.SelectedIndex);
            StaAddrList.ClearSelected();
            Context.StationWatchList.Remove(Item);
            StaAddrRemoved.Items.Add(Item);
           
        }

        private void AddToWatchList_Click(object sender, EventArgs e)
        {
            if (StaAddrRemoved.SelectedItem == null) { return; }
            string Item = StaAddrRemoved.SelectedItem as string;
            StaAddrRemoved.Items.RemoveAt(StaAddrRemoved.SelectedIndex);
            StaAddrRemoved.ClearSelected();
            Context.StationWatchList.Add(Item);
            StaAddrList.Items.Add(Item);
        }

        private void RemoveFromSSIDList_Click(object sender, EventArgs e)
        {
            if (SSIDList.SelectedItem == null) { return; }
            string Item = SSIDList.SelectedItem as string;
            SSIDList.Items.RemoveAt(SSIDList.SelectedIndex);
            SSIDList.ClearSelected();
            Context.SSIDWatchList.Remove(Item);
            SSIDListRemoved.Items.Add(Item);
        }

        private void AddToSSIDList_Click(object sender, EventArgs e)
        {
            if (SSIDListRemoved.SelectedItem == null) { return; }
            string Item = SSIDListRemoved.SelectedItem as string;
            SSIDListRemoved.Items.RemoveAt(SSIDListRemoved.SelectedIndex);
            SSIDListRemoved.ClearSelected();
            Context.SSIDWatchList.Add(Item);
            SSIDList.Items.Add(Item);
        }

        private void ManualAddToStaWatchList_Click(object sender, EventArgs e)
        {
            if ((StaAddrToAdd.Text == null) || (StaAddrToAdd.Text == string.Empty))
            {
                return;
            }
            string Item = StaAddrToAdd.Text.ToString().ToUpper();
            if (Item.Length != 12) { return; }
            Context.StationWatchList.Add(Item);
            UpdateWatchLists();
        }

        private void ManualAddToSSIDWatchList_Click(object sender, EventArgs e)
        {
            if ((SSIDToAdd.Text == null) || (SSIDToAdd.Text == string.Empty))
            {
                return;
            }
            string Item = SSIDToAdd.Text;
            Context.SSIDWatchList.Add(Item);
            UpdateWatchLists();
        }

        private void AddTagInfo_Click(object sender, EventArgs e)
        {
            if ((TagAddr.Text != null) && (TagInfo.Text != null))
            {
                if ((TagAddr.Text != string.Empty) && (TagInfo.Text != string.Empty))
                {
                    FrameControl.StationTags Tag = new FrameControl.StationTags();
                    Tag.MAC = TagAddr.Text.ToUpper();
                    Tag.Tag = TagInfo.Text;
                    Context.StationTagList.Add(Tag);
                }
            }
            UpdateTagLists();
        }
        public void UpdateTagLists()
        {
            TagListBox.BeginUpdate();
            TagListBox.Items.Clear();
            foreach (FrameControl.StationTags Tag in Context.StationTagList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append(Tag.MAC.ToString() + " " + " " + Tag.Tag.ToString());
                TagListBox.Items.Add(sb.ToString());
            }
            TagListBox.EndUpdate();
        }

        private void RemoveTagBtn_Click(object sender, EventArgs e)
        {
            if (TagListBox.SelectedItem == null) { return; }
            FrameControl.StationTags Tag = new FrameControl.StationTags();
            string Item = TagListBox.SelectedItem as string;
            TagListBox.Items.RemoveAt(TagListBox.SelectedIndex);
            TagListBox.ClearSelected();

            int FieldEnd;  
            if (Item == null || Item == string.Empty) { return; }
            FieldEnd = Item.IndexOf(' ');
            char[] Addr = new char[FieldEnd];
            Item.CopyTo(0, Addr, 0, FieldEnd);
            string TagAddr = new string(Addr);
            //Console.WriteLine(SrcAddr);
            Item = Item.Remove(0, FieldEnd + 1);
            Item = Item.Remove(0, 1);
            FieldEnd = Item.Length;
            char[] tag = new char[FieldEnd];
            Item.CopyTo(0, tag, 0, FieldEnd);
            string ItemTag = new string(tag);
            Tag.MAC = TagAddr;
            Tag.Tag = ItemTag;
            Context.StationTagList.Remove(Tag);
            Context.StationWatchList.Remove(Item);
       
        }
    }
}
