using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data.SQLite;
using XmlStorage.Utility;
using System.Diagnostics;
using XmlStorage.Classes;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace XmlStorage.ViewModels
{
    //ROLE:
    //Check for database
    //Build database if necessary.
    //Create an object for each Kanji set
    public class MenuViewModel
    {

        public MenuViewModel()
        {

            KanjiList k = FileUtility.GetKanjiListFromXML("grade-4");
            //Check to see if DB exists
            if (!FileUtility.DoesDataFileExist("Kanji"))
            {
                //Create Database File
                FileUtility.CreateDataFile("Kanji");
                //Create Tables
                DataAccess.AddTables("Kanji");
                //Reset View
            }
            else
            {
                //DataAccess.ListTables("Kanji");
                Debug.WriteLine("Good to Go!");
                DataAccess.AddKanji("X",1);
                DataAccess.AddMeanings("X",new List<string> { "One", "Two" });
                DataAccess.CreateSet("grade-1","Kanji");
                DataAccess.AddKanjiToGradeSet(1);
            }
        }
    }
}
