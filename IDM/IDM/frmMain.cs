using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDM
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            
            InitializeComponent();
        }

        private void tsSetting_Click(object sender, EventArgs e)
        {
            using(frmSetting frm = new frmSetting())
            {
                frm.ShowDialog();
            }
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            using(frmAddUrl frm= new frmAddUrl())
            {
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    frmDownload frmDownload = new frmDownload(this);
                    frmDownload.Url = frm.Url;
                    frmDownload.NhiIsUpdatingFormMain = this.ReloadProcessInfo;
                    frmDownload.Show();
                }
            }
        }

        private void tsRemove_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this record?","Message", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                for(int i=listView1.SelectedItems.Count;i>0;i--)
                {
                    ListViewItem item = listView1.SelectedItems[i - 1];
                    App.DB.FILES.Rows[item.Index].Delete();
                    listView1.Items[item.Index].Remove();
                }
                App.DB.AcceptChanges();
                App.DB.WriteXml(string.Format("{0}/data.dat", Application.StartupPath));
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.ReloadProcessInfo();
        }
        public void ReloadProcessInfo()
        {
            string fileName = string.Format("{0}/data.dat", Application.StartupPath);
            if (File.Exists(fileName))
            {
                listView1.Items.Clear();
                App.DB.Clear();
                App.DB.ReadXml(fileName);

            }
            foreach (MyDB.FILESRow row in App.DB.FILES)
            {
                ListViewItem item = new ListViewItem(row.ID.ToString());
                item.SubItems.Add(row.URL);
                item.SubItems.Add(row.FileName);
                item.SubItems.Add(row.FileSize);
                item.SubItems.Add(row.DateTime.ToLongDateString());
                if (row.DownloadTime!=null)item.SubItems.Add(row.DownloadTime.ToString());
                listView1.Items.Add(item);
            }
        }
    }
}
