using System;
using System.Collections.Generic;
using System.Text;
using XmlStorage.Models;

namespace XmlStorage.Utility
{
    class GameViewLogic
    {
        public static List<List<string>> ZeroOutGrid(int X,int Y)
        {
            List<List<string>> EmptyGrid = new List<List<string>>();
            for (int i = 0;i< X; i++)
            {
                EmptyGrid.Add(new List<string>());
                for (int j = 0;j<Y;j++)
                {
                    EmptyGrid[i].Add(String.Empty);
                }
            }
            return EmptyGrid;
        }
        public static KanjiListModel CreateKanjiShortList(KanjiListModel fullList,int numberSelected)
        {
            KanjiListModel shortList = new KanjiListModel();
            int max = fullList.Kanji.Count;
            Random r = new Random();
            for (int i = 0;i<numberSelected;i++)
            {
                shortList.Kanji.Add(fullList.Kanji[r.Next(0,max)]);
            }
            return shortList;
        }
        public static KanjiModel SelectTargetKanji(KanjiListModel shortList)
        {
            Random r = new Random();
            KanjiModel target = shortList.Kanji[r.Next(shortList.Kanji.Count)];
            return target;
        }

        public static void SetPlayer(List<List<string>> cellValues)
        {
            int xLimit = cellValues.Count;
            int yLimit = cellValues[0].Count;

            cellValues[xLimit / 2][yLimit / 2] = "👺";
        }

        public static void DisperseKanji(List<List<string>> cellValues, KanjiListModel shortList)
        {
            Random r = new Random();
            int xLimit = cellValues.Count;
            int yLimit = cellValues[0].Count;

            int x = r.Next(0,xLimit);
            int y = r.Next(0,yLimit);

            for (int i = 0;i<shortList.Kanji.Count;i++)
            {
                while ((cellValues[x][y] != String.Empty))
                {
                    x = r.Next(0, xLimit);
                    y = r.Next(0, yLimit);
                }
                cellValues[x][y] = shortList.Kanji[i].Character;
            }
        }
    }


}
