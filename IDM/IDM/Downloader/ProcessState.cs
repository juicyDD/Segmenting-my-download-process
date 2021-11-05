using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDM.Downloader
{
    public enum ProcessState
    {
        Downloading,
        Paused,
        Cancelled,
        Completed
    }
}
