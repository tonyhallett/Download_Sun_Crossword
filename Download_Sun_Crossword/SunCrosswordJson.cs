using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Download_Sun_Crossword
{
    public class SunCrosswordJson
    {
        public SunCrosswordData data { get; set; }
    }
    public class SunCrosswordData
    {
        public string headline { get; set; }
        public string type { get; set; }
        public SunCrosswordMeta meta { get; set; }
        public SunCrosswordCopy copy { get; set; }
        //options is an empty array ....
        public string competitioncrossword { get; set; }
        public SuncrosswordSquare[][] grid { get; set; }
        public string created { get; set; }
    }
    public class SuncrosswordSquare
    {
        public int SquareID { get; set; }
        public string Number { get; set; }
        public string Letter { get; set; }
        public string Blank { get; set; }
        //these can be number or ""
        public string WordAcrossID { get; set; }
        public string WordDownID { get; set; }
    }
    public class SunCrosswordMeta
    {
        public string pdf { get; set; }
        public string print_index { get; set; }
    }
    
    public class SunCrosswordCopy
    {
        public string title { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string publisher { get; set; }
        public string setter { get; set; }
        public string byline { get; set; }
        //different property names
        [JsonProperty(PropertyName = "date-publish")]
        public string datepublish { get; set; }
        [JsonProperty(PropertyName = "date-publish-email")]
        public string datepublishemail { get; set; }
        [JsonProperty(PropertyName = "date-publish-analytics")]
        public string datepublishanalytics { get; set; }
        [JsonProperty(PropertyName = "date-release")]
        public string daterelease { get; set; }
        [JsonProperty(PropertyName = "date-solution")]
        public string datesolution { get; set; }
        public string crosswordtype { get; set; }
        public string correctsolutionmessagetext { get; set; }
        public string previoussolutiontext { get; set; }
        public string previoussolutionlink { get; set; }
        public string type { get; set; }
        public SuncrosswordGridSize gridsize { get; set; }
        public SuncrosswordSettings settings { get; set; }
        public SuncrosswordHints hints { get; set; }
        public SuncrosswordClueProvider[] clues { get; set; }
        public SuncrosswordWord[] words { get; set; }
    }
    public class SuncrosswordWord
    {
        public int id { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string solution { get; set; }
    }
    public class SuncrosswordClueProvider
    {
        public string name { get; set; }
        public string title { get; set; }
        public SuncrosswordClue[] clues { get; set; }

    }
    public class SuncrosswordClue
    {
        public int word { get; set; }
        public string number { get; set; }
        public string clue { get; set; }
        public string format { get; set; }
        public int length { get; set; }
        public string answer { get; set; }
    }
    public class SuncrosswordGridSize
    {
        public string type { get; set; }
        public string cols { get; set; }
        public string rows { get; set; }
    }
    public class SuncrosswordSettings
    {
        public string solution_hashed { get; set; }
        public string solution { get; set; }
    }
    public class SuncrosswordHints
    {
        [JsonProperty(PropertyName = "Mark Errors")]
        public string MarkErrors { get; set; }
        [JsonProperty(PropertyName = "Solve Letter")]
        public string SolveLetter { get; set; }
        [JsonProperty(PropertyName = "Solve Word")]
        public string SolveWord { get; set; }
        [JsonProperty(PropertyName = "Ask A Friend")]
        public string AskAFriend { get; set; }
    }
}
