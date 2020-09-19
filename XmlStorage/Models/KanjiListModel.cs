using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace XmlStorage.Models
{
    [XmlRoot("kanjiList")]
    public class KanjiListModel:BaseModel
    {
        public KanjiListModel()
        {
            Kanji = new List<KanjiModel>();
        }
        private List<KanjiModel> _Kanji;
        [XmlElement("kanji")]
        public List<KanjiModel> Kanji
        {
            get
            {
                return _Kanji;
            }
            set
            {
                _Kanji = value;
                NotifyPropertyChanged();
            }
        }
    }
}
