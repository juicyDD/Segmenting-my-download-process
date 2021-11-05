using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDM.Downloader;

namespace IDM.ComponentCustom
{
    public class MyProgressBar : ProgressBar
    {
        public SegmentInfo segment;
        public MyProgressBar(int width, int height, int x, int y)
        {

            //this.segment = i;
            
            //this.Maximum = Convert.ToInt32(this.segment.SegmentSize);
            
            this.Width = width;
            this.Height = height;
            this.Location = new System.Drawing.Point(x, y);
            this.Size = new System.Drawing.Size(width, height);


        }
        public void UpdateProgressBarValue()
        {
            //this.Value = this.segment.CurrentByte;
        }
    }
}
