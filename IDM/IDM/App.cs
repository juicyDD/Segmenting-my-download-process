using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDM
{
    class App
    {
        static MyDB db;
        public static MyDB DB
        {
            get
            {
                if (db == null) db= new MyDB();
                return db;
            }
        }
    }
}
