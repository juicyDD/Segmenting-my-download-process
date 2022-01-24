using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace IDM.Downloader
{
    public class MyDownloader
    {
       // public EventHandler DownloadCompletedEvent;

        private CancellationTokenSource source;
        private CancellationToken token;
        private MyDownloader() {
            source = new CancellationTokenSource();
            token = source.Token;
        }
        private static MyDownloader _instance;
        //private MyDownloader _instance;
        private HttpWebRequest request = null;
        IAsyncResult responseResult = null;
        private volatile HttpWebResponse response = null;
        private string filePath = null;
        private volatile Stream fileStream = null; //******
        private List<Stream> streams = new List<Stream>();
        private string[] bufferFilePath;
        public double fileSize = -1;
        private int state = 0;
        private string urlAddress;
        public List<SegmentInfo> segs = new List<SegmentInfo>();
        private int segments;
        private ProcessState _downloadState;
        private long[] fixedLengthsOfEachSegs=new long[100];
        public ProcessState DownloadState
        {
            get 
            {
                return _downloadState;
            }
            set {
                _downloadState = value;
                this.updatebuttons(this._downloadState);
            }
        }

        public static MyDownloader GetInstance()
        {
            /*if(_instance == null) {
                _instance = new MyDownloader();
            }*/
            _instance = new MyDownloader();
            return _instance;
        }
        //public static readonly MyDownloader Instance;

        public delegate void ShowMessage(string t);
        public ShowMessage msgdel;

        public delegate void UpdateUIButtons(ProcessState state);
        public UpdateUIButtons updatebuttons;
        public delegate bool ModifyUIWhenDownloadCompleted();
        public ModifyUIWhenDownloadCompleted nhiIsModifying;
        //Overload
        public async Task<HttpWebResponse> BeginGettingTheLength(string urlAddress)
        {
            
            this.request = (HttpWebRequest)HttpWebRequest.Create(urlAddress);
            this.request.AllowReadStreamBuffering = false; //if this one is not set, an exception: not allow concurrent io read write would be through idkw :D dm
            this.request.AllowWriteStreamBuffering = false;
            
            this.request.Timeout = 30000;
            this.responseResult = request.BeginGetResponse(AndThenEndUp, null);
            return this.response;
            
        }
        public async void AndThenEndUp(object state)
        {
            this.response = request.EndGetResponse(responseResult) as HttpWebResponse;
            this.fileSize = response.ContentLength;
            
            return;
            //return response.ContentLength;
        }
        //Overload
        public async Task DownloadFile(string urlAddress, string filePath, int Segments)
        {
            this.segments = Segments;
            this.bufferFilePath = new string[Segments];
            int i = 0;
            this.urlAddress = urlAddress;
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;

            while (File.Exists(filePath))
            {
                //File.Delete(filePath);
                // filePath += "(" + (++i) + ")";
                string tem;
                tem = filePath.Insert(filePath.LastIndexOf(".") , "(" + (++i) + ")");
                if (File.Exists(tem)==false)
                {
                    filePath = tem;
                    break;
                }
            }
            this.filePath = filePath;
           // this.msgdel(filePath);
            
            //
            //
            //this.BeginGettingTheLength(urlAddress);
            
            await Task.Run(() => this.BeginGettingTheLength(urlAddress));
            while (this.fileSize == -1) { } //ko await function asynccallback dc ulatr
            //msgdel(this.fileSize.ToString());
            //List<SegmentInfo> segs = SegmentInfo.nhiIsDividingAFile(this.fileSize, Segments);
            //msgdel(segs[0].EndByte.ToString());
            if (this.segs.Count ==0)
            this.segs = await Task.Run(()=> SegmentInfo.nhiIsDividingAFile(this.fileSize, Segments));
            
            string tt = "";
            for(int t = 0;t<Segments;t++)
            {
                tt += segs[t].StartByte.ToString() + "-" + segs[t].EndByte.ToString() +"-"+segs[t].SegmentSize.ToString()+ "\n";
                this.fixedLengthsOfEachSegs[t] =Convert.ToInt32(segs[t].SegmentSize);
            }
            this.msgdel(tt);
            
            while (this.response == null)
            {

            }
            
            try
            {
                var segmentingMyDownload = segs.Select(s => Task.Factory.StartNew(() => _1SegmentDownload(s))).ToArray();
                Task.WaitAll(segmentingMyDownload);
            }
            catch (TaskCanceledException Nhi)
            {
                source.Dispose();
                
            }

            


            while (this.state < Segments) { }
            //msgdel(this.DownloadState.ToString() );
            if (this.DownloadState == ProcessState.Cancelled || this.DownloadState == ProcessState.Paused) return;
            this.fileStream = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.Write);
           foreach(string bufferpath in this.bufferFilePath)
            {
                Stream x = new FileStream(bufferpath, FileMode.Open, FileAccess.Read);
                x.CopyTo(fileStream);
                x.Close();
            }
            this.DeleteTemporaryFiles();
            this.fileStream.Close();

            bool myvar = await Task.Run(()=>this.nhiIsModifying());

            msgdel("XONG");

            //this.DownloadState = new ProcessState();
            this.DownloadState = ProcessState.Completed;
            
            
            

        }
        
        public static int i =0;
        public string generateTemporaryPath()
        {
            string filePath = this.filePath;
            while (true)
            {
                string tem;
                tem = filePath.Insert(filePath.LastIndexOf("."), "(buffer_" + (++i) + ")");
                if (File.Exists(tem) == false)
                {
                    return tem;
                }
            }
        }

        public async Task _1SegmentDownload(SegmentInfo i)
        {
            //msgdel(i.SegmentIndex.ToString());
                await this.DownloadFile(this.urlAddress, this.filePath, i);
                this.state++;
                //msgdel(this.state.ToString()+" ne nhi");
            
        }
        public void SetMyTemporaryFilesInvisible(string path)
        {
            
               File.SetAttributes(path, FileAttributes.Hidden);
            
        }

        public void DeleteTemporaryFiles()
        {
            foreach (string bufferpath in this.bufferFilePath)
            {
                if (File.Exists(bufferpath))
                File.Delete(bufferpath);
            }
        }
        public async Task DownloadFile(string urlAddress, string filePath, SegmentInfo info)
        {
            await DownloadFile(urlAddress, filePath, info, this.token);
        }


        public async Task DownloadFile(string urlAddress, string filePath, SegmentInfo info, CancellationToken token)
        {
            try
            {
                
               // int i = 0;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                request = (HttpWebRequest)HttpWebRequest.Create(urlAddress);
                request.AddRange(info.StartByte, info.EndByte);
                
                
                request.Timeout = 30000;
                response = (HttpWebResponse)request.GetResponse();
                
                Stream s = response.GetResponseStream();
                while (true)
                {
                    string tem;
                    tem = filePath.Insert(filePath.LastIndexOf("."), "(buffer_" + ++i + ")");
                    
                    if (File.Exists(tem) == false)
                    {
                        filePath = tem;
                        this.bufferFilePath[info.SegmentIndex] = tem;
                        break;
                    }
                }
                FileStream os = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                this.SetMyTemporaryFilesInvisible(filePath);
                byte[] buff = new byte[4096];
                int c = 0;
                int byteRemain =Convert.ToInt32(info.SegmentSize);
                while ((c = await s.ReadAsync(buff, 0, 4096)) > 0)
                {
                    if(token.IsCancellationRequested)
                    {
                        os.Close();
                        s.Close();
                        os.Dispose();
                        
                        s.Dispose();
                        throw new OperationCanceledException(token);
                    }
                    os.Write(buff, 0, c);
                   
                    os.Flush(true);
                    info.CurrentByte += Math.Min(4096, byteRemain);
                    byteRemain -= Math.Min(4096, byteRemain);
                }
                os.Close();
                
                s.Close();

            }
            catch(Exception e)
            {
                //return;
               // this.msgdel(e.ToString());
                
            }
            finally
            {
                
            }

        }
        public bool CheckIfTempFileDeleted()
        {
            foreach(string path in this.bufferFilePath)
            {
                if (File.Exists(path)) return false;
            }
            return true;
        }
        public void CancelDownload(string t)
        {
            
            try
            {
                this.source.Cancel();
                
                

            }
            catch(Exception e)
            {
                
            }
            finally
            {
                if (t == "cancel")
                {
                    this.DownloadState = ProcessState.Cancelled;
                    msgdel("Cancelled");
                    while (!this.CheckIfTempFileDeleted())
                    {
                        this.DeleteTemporaryFiles();
                    }

                }
                else if (t == "pause")
                {
                    this.DownloadState = ProcessState.Paused;
                    msgdel("Download process is temporarily paused");
                }
            }
        }
        //----------------------------
        //#CODE REGION: RESUME download process 

        public async Task ResumeDownload(string urlAddress, string filePath, int Segments)
        {
            this.source = new CancellationTokenSource();
            this.token = this.source.Token;

            int count = this.segs.Count;
            this.state = 0;
            this.DownloadState = ProcessState.Downloading;
            string ttt = "";
            foreach(SegmentInfo seg in this.segs)
            {
                Boolean x = await Task.Run(()=>this.ResetSegmentInfo(seg));
                //ttt += seg.CurrentByte.ToString() +"-"+ seg.SegmentSize.ToString()+"\n";

            }
           //msgdel(ttt);

            string tt = "";
            /*for (int t = 0; t < this.segs.Count; t++)
            {
                tt += segs[t].StartByte.ToString() + "-" + segs[t].EndByte.ToString() + "-" + segs[t].SegmentSize.ToString() + "\n";
            }
            msgdel(tt);*/
            try
            {
                var segmentingMyResume = segs.Select(s => Task.Factory.StartNew(() => _1SegmentResume(s))).ToArray();
                Task.WaitAll(segmentingMyResume);
            }
            catch (TaskCanceledException Nhi)
            {
                source.Dispose();

            }
            while (this.state < Segments) { }
            //msgdel(this.DownloadState.ToString() );
            if (this.DownloadState == ProcessState.Cancelled || this.DownloadState == ProcessState.Paused) return;
            this.fileStream = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.Write);
            foreach (string bufferpath in this.bufferFilePath)
            {
                Stream x = new FileStream(bufferpath, FileMode.Open, FileAccess.Read);
                x.CopyTo(fileStream);
                x.Close();
            }
            bool myvar = await Task.Run(() => this.nhiIsModifying());
            msgdel("XONG");
            this.DownloadState = ProcessState.Completed;
            
            this.DeleteTemporaryFiles();
            this.fileStream.Close();
            string aa = "";
            /*for(int t = 0;t<Segments;t++)
            {
                aa += segs[t].CurrentByte.ToString() + "\n";
            }
            msgdel(aa);*/
            //while (this.DownloadState != ProcessState.Completed) { }
            
            
            

        }
        public async Task _1SegmentResume(SegmentInfo i)
        {
            //msgdel(i.SegmentIndex.ToString());
            await this.ResumeFile(this.urlAddress, this.filePath, i, this.token);
            this.state++;

        }
        public async Task ResumeFile(string urlAddress, string filePath, SegmentInfo info, CancellationToken token)
        {
            try
            {

                // int i = 0;
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                request = (HttpWebRequest)HttpWebRequest.Create(urlAddress);
                //+Convert.ToInt32(this.fileSize / this.segments * info.SegmentIndex)
                request.AddRange(info.StartByte, info.EndByte );
                
                request.Timeout = 30000;
                response = (HttpWebResponse)request.GetResponse();

                Stream s = response.GetResponseStream();
                
                /* while (true)
                 {
                     string tem;
                     tem = filePath.Insert(filePath.LastIndexOf("."), "(buffer_" + ++i + ")");

                     if (File.Exists(tem) == false)
                     {
                         filePath = tem;
                         this.bufferFilePath[info.SegmentIndex] = tem;
                         break;
                     }
                 }*/
            filePath = this.bufferFilePath[info.SegmentIndex];
                FileStream os = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                os.Flush(true);
                await Task.Run(()=>os.Seek(info.CurrentByte, SeekOrigin.Begin));
                this.SetMyTemporaryFilesInvisible(filePath);
                byte[] buff = new byte[4096];
                int c = 0;
                int byteRemain = Convert.ToInt32(info.SegmentSize);
                
                while ((c = await s.ReadAsync(buff, 0, 4096)) > 0)
                {
                    os.Flush(true);
                    if (token.IsCancellationRequested)
                    {
                        os.Close();
                        s.Close();
                        throw new OperationCanceledException(token);
                    }
                    os.Write(buff, 0, c);

                    
                    //info.CurrentByte += Math.Min(4096, byteRemain);
                    int tam = info.CurrentByte + Math.Min(4096, byteRemain);
                    info.CurrentByte = tam;
                    byteRemain -= Math.Min(4096, byteRemain);
                }
                
                os.Close();
                s.Close();

            }
            catch (Exception e)
            {
                //return;
                // this.msgdel(e.ToString());

            }
            finally
            {

            }
        }
        //private long[] lengths;
        public bool ResetSegmentInfo(SegmentInfo i)
        {
            long len = 0;
            
            len= new System.IO.FileInfo(this.bufferFilePath[i.SegmentIndex]).Length;
           
            while (len == 0) { }
            int start = Convert.ToInt32(len);
            i.SegmentSize = this.fixedLengthsOfEachSegs[i.SegmentIndex] - len;
            i.StartByte = Convert.ToInt32(i.EndByte - i.SegmentSize + 1);
           // msgdel(i.SegmentSize.ToString() + "//" + (i.EndByte-i.StartByte).ToString());

            i.CurrentByte = start;
            return true;
            
        }
        //#END RESUME REGION
    }
}
