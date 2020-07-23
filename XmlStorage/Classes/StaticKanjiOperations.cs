using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XmlStorage.Classes
{
    class StaticKanjiOperations
    {
        public static KanjiList GetKanjiListFromXML(string category)
        {
            XmlReader x = XmlReader.Create("XML/" + category + ".xml");
            XmlSerializer serializer = new XmlSerializer(typeof(KanjiList));
            string text = File.ReadAllText("XML/" + category + ".xml");
            KanjiList list = (KanjiList)serializer.Deserialize(x);
            x.Close();
            return list;
        }
        public static void UpdateKanjiFile(string category, KanjiList list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(KanjiList));
            var file = System.IO.File.Create("XML/" + category + ".xml");
            serializer.Serialize(file, list);
            file.Close();
        }
    }
}
