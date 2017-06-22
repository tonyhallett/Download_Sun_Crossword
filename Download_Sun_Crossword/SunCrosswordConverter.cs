using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Download_Sun_Crossword
{
    public static class SunCrosswordConverter
    {
        private static CrosswordModelClue[] MapClues(SuncrosswordClue[] clues)
        {
            return clues.Select(c =>
            {
                return new CrosswordModelClue { format = c.format, text = c.clue, number = c.number, wordId = c.word };
            }).ToArray();
        }
        public static CrosswordModelJson Convert(SunCrosswordJson cw)
        {
            var data = cw.data;
            var grid = data.grid;
            var copy = data.copy;
            var clues = copy.clues;
            var words = copy.words;
            var convertedCrossword = new CrosswordModelJson();
            convertedCrossword.datePublished = DateTime.Parse(copy.datepublish);
            convertedCrossword.dateStarted = null;
            convertedCrossword.duration = 0;
            convertedCrossword.title = "Sun " + copy.title;
            convertedCrossword.id = "Sun" + copy.id;
            convertedCrossword.grid = grid.Select(row =>
            {
                return row.Select(square =>
                {
                    return new CrosswordModelSquare { autoSolved = false, guess = "", letter = square.Letter, selected = false, wordSelected = false, solvingMode = 0, number = square.Number };
                }).ToArray();
            }).ToArray();

            var crypticClueProvider = new CrosswordModelClueProvider { name = "Cryptic", acrossClues = MapClues(clues[0].clues), downClues = MapClues(clues[1].clues) };
            var coffeeTimeClueProvider = new CrosswordModelClueProvider { name = "Coffee Time", acrossClues = MapClues(clues[2].clues), downClues = MapClues(clues[3].clues) };
            convertedCrossword.clueProviders = new CrosswordModelClueProvider[] { crypticClueProvider, coffeeTimeClueProvider };

            convertedCrossword.words = words.Select(w =>
            {
                var isAcross = true;
                int lengthEnd;
                //"2-12", means starts on 2 finishes on 12 - length = 12-1+1
                var xParts = w.x.Split(new string[] { "-" }, StringSplitOptions.None);//*************** X is columns
                var yParts = w.y.Split(new string[] { "-" }, StringSplitOptions.None);//*************** Y is rows

                var x = int.Parse(xParts[0]);//lowest is 1
                var start = x;
                var y = int.Parse(yParts[0]);
                var lengthParts = xParts;
                if (yParts.Length == 2)
                {
                    lengthParts = yParts;
                    isAcross = false;
                    start = y;
                }
                lengthEnd = int.Parse(lengthParts[1]);
                var length = lengthEnd - start + 1;
                return new CrosswordModelWord { id = w.id, isAcross = isAcross, x = x, y = y, length = length };
            }).ToArray();
            return convertedCrossword;
        }
    }
}
