using System;
namespace Download_Sun_Crossword
{
    //will decide what else need such as title, date, id
    public class CrosswordModelJson
    {
        public CrosswordModelSquare[][] grid { get; set; }
        public CrosswordModelWord[] words { get; set; }
        public CrosswordModelClueProvider[] clueProviders;
        public string id;
        public string title;
        public DateTime datePublished;
        public DateTime? dateStarted;
        //will have to decide upon correct datatype
        //if ever read from firebase - for upload this will always be 0
        public int duration;
        public int solvingMode;
        public CrosswordModelJson()
        {
        }
    }
    public class CrosswordModelClueProvider
    {
        public string name { get; set; }
        public CrosswordModelClue[] acrossClues { get; set; }
        public CrosswordModelClue[] downClues { get; set; }
    }
    public class CrosswordModelClue
    {
        public string format { get; set; }
        //the clue
        public string text { get; set; }
        public string number { get; set; }
        public int wordId { get; set; }
    }
    public class CrosswordModelWord
    {
        public bool isAcross { get; set; }
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int length { get; set; }
        public int solvingMode { get; set; }
        public bool selected { get; set; }
    }
    public class CrosswordModelSquare
    {
        public string guess { get; set; }
        public string letter { get; set; }
        public string number { get; set; }
        public bool selected { get; set; }
        public bool wordSelected { get; set; }
        //think guessing is 0
        public int solvingMode { get; set; }
        public bool autoSolved { get; set; }

    }
    public class CrosswordModelLookupJson
    {
        public string id;
        public string title;
        public DateTime datePublished;
        public DateTime? dateStarted;
        //will have to decide upon correct datatype
        //if ever read from firebase - for upload this will always be 0
        public int duration;
    }
    public class CrosswordFirebaseRoot
    {
        public CrosswordModelJson[] crosswords { get; set; }
        public CrosswordModelLookupJson[] lookups { get; set; }
    }

}