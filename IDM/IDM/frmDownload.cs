using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDM.Downloader;
using IDM.ComponentCustom;
using System.Diagnostics;

namespace IDM
{
    public partial class frmDownload : Form
    {
        public Timer NhiIsTiming;
        public Stopwatch myStopWatch;
        private GroupOfBars myGroup;
        Dictionary<int, string> ComboBoxItem = new Dictionary<int, string>
        {
            {1, "1 segment" },
            {2, "2 segments" },
            {3, "3 segments" },
            {4, "4 segments" },
            {5, "5 segments" }
        };
        public frmDownload(frmMain frm)
        {
            InitializeComponent();
            this.tbAddress.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            btnResume.Enabled = false;
            x = Downloader.MyDownloader.GetInstance();
            x.updatebuttons += UpdateButtons;
            x.msgdel += showMessage;
            _frmMain = frm;
            this.cbSegments.DataSource = new BindingSource(ComboBoxItem, null);
            this.cbSegments.DisplayMember = "Value";
            this.cbSegments.ValueMember = "Key";
            this.cbSegments.SelectedItem = 0;
            
            
        }
        WebClient client;
        public string Url { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public decimal Percentage { get; set; }
        private frmMain _frmMain;
        public Downloader.MyDownloader x;
        private void UpdateDownloadTime(object sender, EventArgs e)
        {
            this.lbStopWatch.Text = this.myStopWatch.Elapsed.ToString(@"hh\:mm\:ss\.f");
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.myStopWatch = new Stopwatch();
            this.NhiIsTiming = new Timer();
            this.NhiIsTiming.Interval = 1000;
            NhiIsTiming.Tick += UpdateDownloadTime;
            this.NhiIsTiming.Start();
            this.myStopWatch.Start();



            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnBrowse.Enabled = false;
            KeyValuePair<int, string> bono = (KeyValuePair<int, string>)cbSegments.SelectedItem;
            int nhi = bono.Key;

            x.DownloadState = ProcessState.Downloading;
            //-----------------------
            x.nhiIsModifying += this.UIAdjustingWhenEverthingEndsUp;
            //------------------------------
            Uri uri = new Uri(this.Url);
            FileName = System.IO.Path.GetFileName(uri.AbsolutePath);

            MessageBox.Show(nhi.ToString());
            var downloadTask = Task.Run(async () => await x.DownloadFile(this.Url, Properties.Settings.Default.Path + "/" + FileName, nhi));
            while (x.segs.Count < nhi) { };
            foreach(SegmentInfo i in x.segs)
            {
                this.myGroup.NhiIsAssigningSegmentInfo(i.SegmentIndex, i);
                this.myGroup.progressBars[i.SegmentIndex].Minimum = 0;
                this.myGroup.progressBars[i.SegmentIndex].Maximum = Convert.ToInt32(i.SegmentSize);
            }
            foreach (SegmentInfo i in this.myGroup.segments)
            {
                //add delegate update progress bar
                i.UpdateProgressBar += NhiIsUpdatingProgressBarForeachSegment;
            }
           // var task = Task.Run(async () => await x.DownloadFile(this.Url, Properties.Settings.Default.Path + "/" + FileName));
            /*Uri uri = new Uri(this.Url);
            FileName = System.IO.Path.GetFileName(uri.AbsolutePath);
            client.DownloadFileAsync(uri, Properties.Settings.Default.Path + "/" + FileName);*/
            
           
        }
        delegate void SetValueCallback(SegmentInfo i);

        private void SetValue(SegmentInfo i)
        {
            if (this.myGroup.progressBars[i.SegmentIndex].InvokeRequired)
            {
                try
                {
                    SetValueCallback d = new SetValueCallback(SetValue);
                    this.Invoke(d, new object[] { i });
                }
                catch(Exception nhi)
                {

                }
            }
            else
            {
                this.myGroup.progressBars[i.SegmentIndex].Value = i.CurrentByte;
            }
        }
        public void NhiIsUpdatingProgressBarForeachSegment(SegmentInfo seg)
        {
            int segmentIndex = seg.SegmentIndex;
            //this.myGroup.progressBars[segmentIndex].Value = seg.CurrentByte;
            SetValue(seg);
            
        }
        public delegate bool UIAdjustingWhenEverythingEndsUpCallback();
        public bool UIAdjustingWhenEverthingEndsUp()
        {
            if(this.btnStop.InvokeRequired|| btnStart.InvokeRequired || btnResume.InvokeRequired || btnPause.InvokeRequired )
            {
                try
                {
                    UIAdjustingWhenEverythingEndsUpCallback d = new UIAdjustingWhenEverythingEndsUpCallback(UIAdjustingWhenEverthingEndsUp);
                    this.Invoke(d);
                }
                catch (Exception nhi) { }
            }
            else
            {
                this.btnStop.Enabled = false;
                this.btnStart.Enabled = false;
                this.btnResume.Enabled = false;
                this.btnPause.Enabled = false;
                this.myStopWatch.Stop();
                this.AddNewDownloadInfo();
            }
            return true;
        }
        public bool AddNewDownloadInfo()
        {
            MyDB.FILESRow row = App.DB.FILES.NewFILESRow();
            row.URL = Url;
            row.FileName = FileName;
            row.FileSize = (string.Format("{0:0.##} KB", Convert.ToInt32(this.x.fileSize) / 1024));
            row.DateTime = DateTime.Now;
            row.DownloadTime = this.lbStopWatch.Text.ToString();
            App.DB.FILES.AddFILESRow(row);
            App.DB.AcceptChanges();
            App.DB.WriteXml(string.Format("{0}/data.dat", Application.StartupPath));
            return true;
        }
        public void UpdateButtons(ProcessState state)
        {
            if (state==ProcessState.Downloading)
            {
                btnStart.Enabled = false;
                btnResume.Enabled = false;
                btnStop.Enabled = true;
                btnPause.Enabled = true;
            }
            if(state==ProcessState.Paused)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnResume.Enabled = true;
                btnPause.Enabled = false;
            }
            if(state == ProcessState.Cancelled)
            {
                btnStart.Enabled = false;
                btnResume.Enabled = false;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
            }
           /*if (state == ProcessState.Completed)
            {
                
                
                btnStop.Enabled = false;
                btnStart.Enabled = false;
                btnResume.Enabled = false;
                
                this.btnPause.Enabled = false;
                
                //this.myStopWatch.Stop();
                //this.NhiIsTiming.Stop();
                
                MessageBox.Show("Xong roi");

            }
           */

        }
        private void showMessage(string t)
        {
            MessageBox.Show(t);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.myStopWatch.Stop();
            DialogResult dialogResult = MessageBox.Show("Are you sure you wanta cancel this download process", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //do something
                x.CancelDownload("cancel");
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog fbd= new FolderBrowserDialog() { Description="Select your path."})
            {
                if(fbd.ShowDialog() == DialogResult.OK)
                {
                    tbPath.Text = fbd.SelectedPath;
                    Properties.Settings.Default.Path = tbPath.Text;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {
            client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            tbAddress.Text = Url;
        }

        public void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MyDB.FILESRow row = App.DB.FILES.NewFILESRow();
            row.URL = Url;
            row.FileName = FileName;
            row.FileSize = (string.Format("{0:0.##} KB", FileSize / 1024));
            row.DateTime = DateTime.Now;
            App.DB.FILES.AddFILESRow(row);
            App.DB.AcceptChanges();
            App.DB.WriteXml(string.Format("{0}/data.dat", Application.StartupPath));
            ListViewItem item = new ListViewItem(row.ID.ToString());
            item.SubItems.Add(row.URL);
            item.SubItems.Add(row.FileName);
            item.SubItems.Add(row.FileSize);
            item.SubItems.Add(row.DateTime.ToLongDateString());
            _frmMain.listView1.Items.Add(item);
            this.Close();
        }
        
        public void InsertProcessInfo(object sender, EventArgs e)
        {
            //MyDB.FILESRow row = App.DB.FILES.NewFILESRow();
            
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
           
        }

        private void btnPause_Click(object sender, EventArgs e)
        {

            x.CancelDownload("pause");
            this.myStopWatch.Stop();
        }

        private void cbSegments_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<int, string> x =(KeyValuePair<int,string>) cbSegments.SelectedItem;
            if(this.myGroup!=null) foreach(MyProgressBar bar in this.myGroup.progressBars)
            {
                    this.Controls.Remove(bar);
            }
            this.myGroup = new GroupOfBars(x.Key);
            //MessageBox.Show(myGroup.progressBars[0].Location.X.ToString()+" "+ myGroup.progressBars[0].Location.Y.ToString()+" "+
             //   myGroup.progressBars[0].Width.ToString()+ " "+ myGroup.progressBars[0].Height.ToString());
            
            for (int i = 0; i<myGroup.numberOfSegments;i++)
            {
                this.Controls.Add(myGroup.progressBars[i]);
                
                myGroup.progressBars[i].Visible = true;
                
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            this.myStopWatch.Start();
            KeyValuePair<int, string> bono = (KeyValuePair<int, string>)cbSegments.SelectedItem;
            int nhi = bono.Key;
            x.DownloadState = ProcessState.Downloading;

            var resumeTask = Task.Run(async () => await x.ResumeDownload(this.Url, Properties.Settings.Default.Path + "/" + FileName, nhi));
        }
        //static int time = 0;
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (x.DownloadState==ProcessState.Paused)
            {
                DialogResult dialogResult = MessageBox.Show("The process is temporarily paused, if you quit atm it will be cancelled pye pye", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                    this.Close();
                }
            }
            
            
        }

        private void frmDownload_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.NhiIsUpdatingFormMain();
        }
        public delegate void UpdateFormMain();
        public UpdateFormMain NhiIsUpdatingFormMain;
    }
}
