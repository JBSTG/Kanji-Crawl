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
using System.ComponentModel;
using XmlStorage.Models;
using XmlStorage.Commands;
using System.Windows.Input;

namespace XmlStorage.ViewModels
{
    //ROLE:
    //Check for database
    //Build database if necessary.
    //Create an object for each Kanji set
    public class MenuViewModel : BaseModel
    {
        public MenuViewModel()
        {
            SetNames = DataAccess.GetSetNames();
            Test = "ONE";
        }
        private string _Test;
        public string Test
        {
            get
            {
                return _Test;
            }
            set
            {
                _Test = value;
                NotifyPropertyChanged();
            }
        }
        private List<string> _SetNames;
        public List<string> SetNames
        {
            get
            {
                return _SetNames;
            }
            set
            {
                _SetNames = value;
                NotifyPropertyChanged();
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }
    }
}
