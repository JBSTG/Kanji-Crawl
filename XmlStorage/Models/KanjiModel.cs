using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace XmlStorage.Models
{
    public class KanjiModel : BaseModel
    {
        private string _Character = String.Empty;
        private int _Grade;
        private List<string> _Meanings;
        private List<string> _NameReadings;
        private List<string> _KunReadings;
        private List<string> _OnReadings;
        private string _HeisigEnglish;
        private string _Unicode;
        private int _JLPT;
        private int _StrokeCount;

        public KanjiModel()
        {
            Meanings = new List<string>();
        }
        public KanjiModel(string c,int g, int s,int j,string u, string h)
        {
            Character = c;
            Grade = g;
            StrokeCount = s;
            JLPT = j;
            Unicode = u;
            HeisigEnglish = h;
            Meanings = new List<string>();
        }
        [XmlElement("character")]
        public string Character { 
            get
            {
                return _Character;
            }
            set
            {
                _Character = value;
                NotifyPropertyChanged();
            }
        }
        [XmlElement("meanings")]
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

        [XmlElement("kunReadings")]
        public List<string> KunReadings
        {
            get
            {
                return _KunReadings;
            }
            set
            {
                _KunReadings = value;
                NotifyPropertyChanged();
            }
        }
        [XmlElement("onReadings")]
        public List<string> OnReadings
        {
            get
            {
                return _OnReadings;
            }
            set
            {
                _OnReadings = value;
                NotifyPropertyChanged();
            }
        }
        [XmlElement("nameReadings")]
        public List<string> NameReadings
        {
            get
            {
                return _NameReadings;
            }
            set
            {
                _NameReadings = value;
                NotifyPropertyChanged();
            }
        }
        [XmlElement("grade")]
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

        [XmlElement("jlpt")]
        public int JLPT
        {
            get
            {
                return _JLPT;
            }
            set
            {
                _JLPT = value;
                NotifyPropertyChanged();
            }
        }

        [XmlElement("strokeCount")]
        public int StrokeCount
        {
            get
            {
                return _StrokeCount;
            }
            set
            {
                _StrokeCount = value;
                NotifyPropertyChanged();
            }
        }

        [XmlElement("unicode")]
        public string Unicode
        {
            get
            {
                return _Unicode;
            }
            set
            {
                _Unicode = value;
                NotifyPropertyChanged();
            }
        }

        [XmlElement("heisigEnglish")]
        public string HeisigEnglish
        {
            get
            {
                return _HeisigEnglish;
            }
            set
            {
                _HeisigEnglish = value;
                NotifyPropertyChanged();
            }
        }

    }
}
