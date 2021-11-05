using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDM.Downloader
{
    public class BlockInfo 
    {

        private int _startIndex;
        public int StartIndex
        {
            get
            {
                return _startIndex;

            }
            private set { }
        }
        private int _endIndex;
        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
            private set { }
        }
        public BlockInfo(int start, int end)
        {
            this._startIndex = start;
            this._endIndex = end;
        }
        
        
    }
}
