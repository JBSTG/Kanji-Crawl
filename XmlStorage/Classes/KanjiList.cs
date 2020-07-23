using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XmlStorage.Classes
{
    public class Kanji
    {
        [XmlElement("character")]
        public string character { get; set; }
        [XmlElement("meaning")]
        public List<string> meanings { get; set; }
        [XmlElement("kun")]
        public List<string> kun { get; set; }
        [XmlElement("on")]
        public List<string> on { get; set; }
        [XmlElement("correct")]
        public int correct { get; set; }

        [XmlElement("incorrect")]
        public int incorrect { get; set; }

        public Kanji()
        {

        }
        public Kanji(string c, List<string> m, List<string> k, List<string> o)
        {
            character = c;
            meanings = m;
            kun = k;
            on = o;
        }
    }
    [XmlRoot("kanjiList")]
    public class KanjiList
    {
        [XmlElement("kanji")]
        public List<Kanji> kanji { get; set; }
        public KanjiList()
        {
            kanji = new List<Kanji>();
        }
    }
}
