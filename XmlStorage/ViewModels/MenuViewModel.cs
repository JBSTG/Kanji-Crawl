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
using XmlStorage.Models;
using System.Xml.Serialization;

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
            //Check to see if DB exists
            if (!FileUtility.DoesDataFileExist("Kanji"))
            {
                //DatabaseBuilder.BuildDatabase();
                Debug.WriteLine("db needed");
            }
            //DatabaseBuilder.ListAllKanji("Kanji");
        }
    }
}
