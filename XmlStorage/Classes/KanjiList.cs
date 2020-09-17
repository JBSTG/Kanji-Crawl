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


        [XmlElement("meanings")]
        public List<string> meanings { get; set; }


        [XmlElement("kunReadings")]
        public List<string> kunReadings { get; set; }


        [XmlElement("onReadings")]
        public List<string> onReadings { get; set; }


        [XmlElement("nameReadings")]
        public List<string> nameReadings { get; set; }

        [XmlElement("grade")]
        public int grade { get; set; }

        [XmlElement("jlpt")]
        public int jlpt { get; set; }

        [XmlElement("strokeCount")]
        public int strokeCount { get; set; }

        [XmlElement("unicode")]
        public string unicode { get; set; }

        [XmlElement("heisigEnglish")]
        public string heisigEnglish { get; set; }

        public Kanji()
        {

        }
        public Kanji(
            string c,
            List<string> m,
            List<string> k, 
            List<string> o,
            List<string> n,
            int g,
            int j,
            int s,
            string u,
            string h)
        {
            character = c;
            meanings = m;
            kunReadings = k;
            onReadings = o;
            nameReadings = n;
            grade = g;
            jlpt = j;
            strokeCount = s;
            unicode = u;
            heisigEnglish = h;
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
