namespace ProbeMon
{
    partial class EditWatchList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditWatchList));
            this.StaAddrList = new System.Windows.Forms.ListBox();
            this.SSIDList = new System.Windows.Forms.ListBox();
            this.StaAddrRemoved = new System.Windows.Forms.ListBox();
            this.SSIDListRemoved = new System.Windows.Forms.ListBox();
            this.RemoveFromWatchList = new System.Windows.Forms.Button();
            this.AddToWatchList = new System.Windows.Forms.Button();
            this.AddToSSIDList = new System.Windows.Forms.Button();
            this.RemoveFromSSIDList = new System.Windows.Forms.Button();
            this.StaAddrToAdd = new System.Windows.Forms.TextBox();
            this.SSIDToAdd = new System.Windows.Forms.TextBox();
            this.ManualAddToStaWatchList = new System.Windows.Forms.Button();
            this.ManualAddToSSIDWatchList = new System.Windows.Forms.Button();
            this.TagInfo = new System.Windows.Forms.TextBox();
            this.AddTagInfo = new System.Windows.Forms.Button();
            this.TagAddr = new System.Windows.Forms.TextBox();
            this.TagListBox = new System.Windows.Forms.ListBox();
            this.RemoveTagBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StaAddrList
            // 
            this.StaAddrList.FormattingEnabled = true;
            this.StaAddrList.Location = new System.Drawing.Point(3, 1);
            this.StaAddrList.Name = "StaAddrList";
            this.StaAddrList.Size = new System.Drawing.Size(190, 264);
            this.StaAddrList.TabIndex = 0;
            // 
            // SSIDList
            // 
            this.SSIDList.FormattingEnabled = true;
            this.SSIDList.Location = new System.Drawing.Point(3, 276);
            this.SSIDList.Name = "SSIDList";
            this.SSIDList.Size = new System.Drawing.Size(190, 290);
            this.SSIDList.TabIndex = 1;
            // 
            // StaAddrRemoved
            // 
            this.StaAddrRemoved.FormattingEnabled = true;
            this.StaAddrRemoved.Location = new System.Drawing.Point(398, 1);
            this.StaAddrRemoved.Name = "StaAddrRemoved";
            this.StaAddrRemoved.Size = new System.Drawing.Size(190, 264);
            this.StaAddrRemoved.TabIndex = 2;
            // 
            // SSIDListRemoved
            // 
            this.SSIDListRemoved.FormattingEnabled = true;
            this.SSIDListRemoved.Location = new System.Drawing.Point(398, 276);
            this.SSIDListRemoved.Name = "SSIDListRemoved";
            this.SSIDListRemoved.Size = new System.Drawing.Size(190, 290);
            this.SSIDListRemoved.TabIndex = 3;
            // 
            // RemoveFromWatchList
            // 
            this.RemoveFromWatchList.Location = new System.Drawing.Point(254, 88);
            this.RemoveFromWatchList.Name = "RemoveFromWatchList";
            this.RemoveFromWatchList.Size = new System.Drawing.Size(75, 23);
            this.RemoveFromWatchList.TabIndex = 4;
            this.RemoveFromWatchList.Text = "Remove -->";
            this.RemoveFromWatchList.UseVisualStyleBackColor = true;
            this.RemoveFromWatchList.Click += new System.EventHandler(this.RemoveFromWatchList_Click);
            // 
            // AddToWatchList
            // 
            this.AddToWatchList.Location = new System.Drawing.Point(254, 128);
            this.AddToWatchList.Name = "AddToWatchList";
            this.AddToWatchList.Size = new System.Drawing.Size(75, 23);
            this.AddToWatchList.TabIndex = 5;
            this.AddToWatchList.Text = "<-- Readd";
            this.AddToWatchList.UseVisualStyleBackColor = true;
            this.AddToWatchList.Click += new System.EventHandler(this.AddToWatchList_Click);
            // 
            // AddToSSIDList
            // 
            this.AddToSSIDList.Location = new System.Drawing.Point(254, 418);
            this.AddToSSIDList.Name = "AddToSSIDList";
            this.AddToSSIDList.Size = new System.Drawing.Size(75, 23);
            this.AddToSSIDList.TabIndex = 6;
            this.AddToSSIDList.Text = "<-- Readd";
            this.AddToSSIDList.UseVisualStyleBackColor = true;
            this.AddToSSIDList.Click += new System.EventHandler(this.AddToSSIDList_Click);
            // 
            // RemoveFromSSIDList
            // 
            this.RemoveFromSSIDList.Location = new System.Drawing.Point(254, 376);
            this.RemoveFromSSIDList.Name = "RemoveFromSSIDList";
            this.RemoveFromSSIDList.Size = new System.Drawing.Size(75, 23);
            this.RemoveFromSSIDList.TabIndex = 7;
            this.RemoveFromSSIDList.Text = "Remove -->";
            this.RemoveFromSSIDList.UseVisualStyleBackColor = true;
            this.RemoveFromSSIDList.Click += new System.EventHandler(this.RemoveFromSSIDList_Click);
            // 
            // StaAddrToAdd
            // 
            this.StaAddrToAdd.Location = new System.Drawing.Point(242, 169);
            this.StaAddrToAdd.Name = "StaAddrToAdd";
            this.StaAddrToAdd.Size = new System.Drawing.Size(111, 20);
            this.StaAddrToAdd.TabIndex = 8;
            // 
            // SSIDToAdd
            // 
            this.SSIDToAdd.Location = new System.Drawing.Point(242, 459);
            this.SSIDToAdd.Name = "SSIDToAdd";
            this.SSIDToAdd.Size = new System.Drawing.Size(111, 20);
            this.SSIDToAdd.TabIndex = 9;
            // 
            // ManualAddToStaWatchList
            // 
            this.ManualAddToStaWatchList.Location = new System.Drawing.Point(254, 195);
            this.ManualAddToStaWatchList.Name = "ManualAddToStaWatchList";
            this.ManualAddToStaWatchList.Size = new System.Drawing.Size(75, 23);
            this.ManualAddToStaWatchList.TabIndex = 10;
            this.ManualAddToStaWatchList.Text = "Add";
            this.ManualAddToStaWatchList.UseVisualStyleBackColor = true;
            this.ManualAddToStaWatchList.Click += new System.EventHandler(this.ManualAddToStaWatchList_Click);
            // 
            // ManualAddToSSIDWatchList
            // 
            this.ManualAddToSSIDWatchList.Location = new System.Drawing.Point(254, 485);
            this.ManualAddToSSIDWatchList.Name = "ManualAddToSSIDWatchList";
            this.ManualAddToSSIDWatchList.Size = new System.Drawing.Size(75, 23);
            this.ManualAddToSSIDWatchList.TabIndex = 11;
            this.ManualAddToSSIDWatchList.Text = "Add";
            this.ManualAddToSSIDWatchList.UseVisualStyleBackColor = true;
            this.ManualAddToSSIDWatchList.Click += new System.EventHandler(this.ManualAddToSSIDWatchList_Click);
            // 
            // TagInfo
            // 
            this.TagInfo.Location = new System.Drawing.Point(640, 301);
            this.TagInfo.Name = "TagInfo";
            this.TagInfo.Size = new System.Drawing.Size(111, 20);
            this.TagInfo.TabIndex = 12;
            // 
            // AddTagInfo
            // 
            this.AddTagInfo.Location = new System.Drawing.Point(657, 328);
            this.AddTagInfo.Name = "AddTagInfo";
            this.AddTagInfo.Size = new System.Drawing.Size(75, 23);
            this.AddTagInfo.TabIndex = 13;
            this.AddTagInfo.Text = "Add Tag";
            this.AddTagInfo.UseVisualStyleBackColor = true;
            this.AddTagInfo.Click += new System.EventHandler(this.AddTagInfo_Click);
            // 
            // TagAddr
            // 
            this.TagAddr.Location = new System.Drawing.Point(640, 277);
            this.TagAddr.Name = "TagAddr";
            this.TagAddr.Size = new System.Drawing.Size(111, 20);
            this.TagAddr.TabIndex = 14;
            // 
            // TagListBox
            // 
            this.TagListBox.FormattingEnabled = true;
            this.TagListBox.Location = new System.Drawing.Point(608, 1);
            this.TagListBox.Name = "TagListBox";
            this.TagListBox.Size = new System.Drawing.Size(190, 264);
            this.TagListBox.TabIndex = 15;
            // 
            // RemoveTagBtn
            // 
            this.RemoveTagBtn.Location = new System.Drawing.Point(657, 357);
            this.RemoveTagBtn.Name = "RemoveTagBtn";
            this.RemoveTagBtn.Size = new System.Drawing.Size(75, 23);
            this.RemoveTagBtn.TabIndex = 16;
            this.RemoveTagBtn.Text = "Remove Tag";
            this.RemoveTagBtn.UseVisualStyleBackColor = true;
            this.RemoveTagBtn.Click += new System.EventHandler(this.RemoveTagBtn_Click);
            // 
            // EditWatchList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 580);
            this.Controls.Add(this.RemoveTagBtn);
            this.Controls.Add(this.TagListBox);
            this.Controls.Add(this.TagAddr);
            this.Controls.Add(this.AddTagInfo);
            this.Controls.Add(this.TagInfo);
            this.Controls.Add(this.ManualAddToSSIDWatchList);
            this.Controls.Add(this.ManualAddToStaWatchList);
            this.Controls.Add(this.SSIDToAdd);
            this.Controls.Add(this.StaAddrToAdd);
            this.Controls.Add(this.RemoveFromSSIDList);
            this.Controls.Add(this.AddToSSIDList);
            this.Controls.Add(this.AddToWatchList);
            this.Controls.Add(this.RemoveFromWatchList);
            this.Controls.Add(this.SSIDListRemoved);
            this.Controls.Add(this.StaAddrRemoved);
            this.Controls.Add(this.SSIDList);
            this.Controls.Add(this.StaAddrList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditWatchList";
            this.Text = "Edit Watch List";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox StaAddrList;
        private System.Windows.Forms.ListBox SSIDList;
        private System.Windows.Forms.ListBox StaAddrRemoved;
        private System.Windows.Forms.ListBox SSIDListRemoved;
        private System.Windows.Forms.Button RemoveFromWatchList;
        private System.Windows.Forms.Button AddToWatchList;
        private System.Windows.Forms.Button AddToSSIDList;
        private System.Windows.Forms.Button RemoveFromSSIDList;
        private System.Windows.Forms.TextBox StaAddrToAdd;
        private System.Windows.Forms.TextBox SSIDToAdd;
        private System.Windows.Forms.Button ManualAddToStaWatchList;
        private System.Windows.Forms.Button ManualAddToSSIDWatchList;
        private System.Windows.Forms.TextBox TagInfo;
        private System.Windows.Forms.Button AddTagInfo;
        private System.Windows.Forms.TextBox TagAddr;
        private System.Windows.Forms.ListBox TagListBox;
        private System.Windows.Forms.Button RemoveTagBtn;
    }
}