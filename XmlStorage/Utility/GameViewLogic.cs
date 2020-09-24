using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using XmlStorage.Models;

namespace XmlStorage.Utility
{
    class GameViewLogic
    {
        public static ObservableCollection<ObservableCollection<string>> ZeroOutGrid(int X,int Y)
        {
            ObservableCollection<ObservableCollection<string>> EmptyGrid = new ObservableCollection<ObservableCollection<string>>();
            for (int i = 0;i< X; i++)
            {
                EmptyGrid.Add(new ObservableCollection<string>());
                for (int j = 0;j<Y;j++)
                {
                    EmptyGrid[i].Add(String.Empty);
                }
            }
            return EmptyGrid;
        }

        public static string MeaningListToString(KanjiModel target)
        {
            string list = String.Empty;
            for (int i = 0;i<target.Meanings.Count;i++)
            {
                list += target.Meanings[i];
                if (i<target.Meanings.Count-1)
                {
                    list += ", ";
                }
            }
            return list;
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

        public static int[] SetPlayer(ObservableCollection<ObservableCollection<string>> cellValues)
        {
            int xLimit = cellValues.Count;
            int yLimit = cellValues[0].Count;
            int x = xLimit / 2;
            int y = yLimit / 2;
            int[] playerLocation = new int[] {x,y};
            cellValues[x][y] = "👺";
            return playerLocation;
        }

        public static void DisperseKanji(ObservableCollection<ObservableCollection<string>> cellValues, KanjiListModel shortList)
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
