using IDM.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDM.ComponentCustom;

namespace IDM
{
    public class SegmentInfo 
    {
        private static MyDownloader myDownloader = MyDownloader.GetInstance();
        protected double _segmentSize;
        protected int _segmentIndex;
        protected  int _startByte;
        protected int _endByte;
        private int _currentByte=0;
        public delegate void NhiIsUpdatingProgressBar(SegmentInfo seg);
        public NhiIsUpdatingProgressBar UpdateProgressBar;
        public int CurrentByte
        {
            get
            {
                return this._currentByte;
            }
            set
            {
                _currentByte = value;
                Task.Run(() => this.UpdateProgressBar(this));
            }
        }
        
        public double SegmentSize
        {
            get
            {
                return _segmentSize;
            }
            set 
            {
                _segmentSize = value;
            }
        }
        public int SegmentIndex
        {
            get
            {
                return _segmentIndex;
            }
            private set { }
        }
        public int StartByte
        {
            get
            {
                return _startByte;
            }
            set 
            {
                _startByte = value;
            }
        }
        public int EndByte
        {
            get
            {
                return _endByte;
            }
            private set { }
        }

        public SegmentInfo(double segsize, int segidx, int start, int end)
        {
            this._segmentSize = segsize;
            this._segmentIndex = segidx;
            this._startByte = start;
            this._endByte = end;
        }
        public static List<SegmentInfo> nhiIsDividingAFile(double contentLength, int segs)
        {
            int segsize = Convert.ToInt32(contentLength / segs + 1);
            List<SegmentInfo> nhi = new List<SegmentInfo>();
            int i = 0;
            while(i<segs)
            {
                if (i==0)
                {
                    int startidx = 0;
                    int endidx = (int)segsize - 1;
                    double size = segsize;
                    int segidx = i;
                    SegmentInfo bono = new SegmentInfo(size, segidx, startidx, endidx);
                    nhi.Add(bono);
                }
                else if(i<segs-1)
                {
                    int startidx = nhi[i - 1].EndByte + 1;
                    double size = segsize;
                    int segidx = i;
                    int endidx = Convert.ToInt32((i + 1) * size - 1);
                    SegmentInfo bono = new SegmentInfo(size, segidx, startidx, endidx);
                    nhi.Add(bono);
                }
                else if (i== segs-1)
                {
                    int startidx = nhi[i - 1].EndByte + 1;
                    int endidx = Convert.ToInt32(Math.Ceiling(contentLength) - 1) ;
                    int segidx = i;
                    //double size = contentLength - segsize * i - 1;
                    double size = contentLength - segsize * i;
                    SegmentInfo bono = new SegmentInfo(size, segidx, startidx, endidx);
                    nhi.Add(bono);
                }
                i++;
            }
            
            return nhi;
        }

    }
}
