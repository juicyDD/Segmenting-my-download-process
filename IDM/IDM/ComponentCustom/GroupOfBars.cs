using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IDM.ComponentCustom
{
    class GroupOfBars
    {
        public int numberOfSegments;
        public SegmentInfo[] segments;
        public MyProgressBar[] progressBars ;
        private int initialX = 62;
        private int initialY = 141;
        private int fixedWidth = 381;
        private int fixedHeight = 23;
        private int commonWidth;
        private int commonHeight;
        private int commonY;
        public GroupOfBars(int segment)
        {
            this.numberOfSegments = segment;
            
            this.commonWidth = this.fixedWidth / segment;
            this.commonHeight = this.fixedHeight;
            this.commonY = this.initialY;
            this.segments = new SegmentInfo[this.numberOfSegments];

            this.progressBars = new MyProgressBar[this.numberOfSegments];
            for(int i=0;i<numberOfSegments;i++)
            {
                int x = Convert.ToInt32(this.initialX + i * this.commonWidth);
                
                progressBars[i] = new MyProgressBar(this.commonWidth, this.commonHeight, x, this.commonY);
            }

            
        }
        public void NhiIsAssigningSegmentInfo(int segmentidx, SegmentInfo info)
        {
            
            this.segments[segmentidx] = info;
        }
    }
}
