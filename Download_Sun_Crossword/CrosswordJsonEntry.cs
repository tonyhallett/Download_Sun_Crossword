using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Download_Sun_Crossword
{
    public class CrosswordJson
    {
        public CrosswordJsonEntry[] data { get; set; }
    }
    public class CrosswordJsonEntry
    {
        public string id { get; set; }
        public string name { get; set; }
        public string date { get; set; }//change to date ?
        public string url { get; set; }
        public string mobile { get; set; }
        public string tablet { get; set; }
        public string web { get; set; }
        public string lastupdated { get; set; }
        public string puzzleicon { get; set; }
        public string categories { get; set; }
    }
}
