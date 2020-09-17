using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XmlStorage.Models
{
    class KanjiModel : BaseModel
    {
        public KanjiModel()
        {
            Meanings = new List<string>();
        }
        private string _Character = String.Empty;
        public string Character { 
            get
            {
                return _Character;
            }
            set
            {
                Character = value;
                NotifyPropertyChanged();
            }
        }
        private int _Grade;
        public int Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
                NotifyPropertyChanged();
            }
        }
        private List<string> _Meanings;
        public List<string> Meanings
        {
            get
            {
                return _Meanings;
            }
            set
            {
                _Meanings = value;
                NotifyPropertyChanged();
            }
        }

    }
}
