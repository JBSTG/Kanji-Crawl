using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using XmlStorage.Classes;

namespace XmlStorage.Utility
{
    class FileUtility
    {
        public static bool DoesDataFileExist(string dbname)
        {
            return File.Exists("" + dbname + ".db");
        }
        public static void CreateDataFile(string dbname)
        {
            File.Create(""+dbname+".db").Close();
        }

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
        public static async Task<JArray> QueueKanjiSet(string category)
        {
            List<string> kanjiToDownload = new List<string>();
            //Call API, get list in JSON
            string url = "http://kanjiapi.dev/v1/kanji/" + category;
            HttpClient http = new HttpClient();
            HttpResponseMessage res = await http.GetAsync(url);
            HttpContent content = res.Content;
            var data = await content.ReadAsStringAsync();
            JArray chars = JArray.Parse(data);
            return chars;
        }

        public static List<string> JArrayToStringList(JArray j)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < j.Count; i++)
            {
                res.Add(j[i].ToString());
            }
            return res;
        }

        public static async Task<Kanji> DownloadKanji(HttpClient http, string character)
        {
            string url = "http://kanjiapi.dev/v1/kanji/" + character;
            //Query API
            HttpResponseMessage res = await http.GetAsync(url);
            HttpContent content = res.Content;
            var data = await content.ReadAsStringAsync();
            JObject obj = JObject.Parse(data);

            //Meanings
            List<string> meanings = JArrayToStringList(JArray.Parse(obj["meanings"].ToString()));
            //Kun
            List<string> kun = JArrayToStringList(JArray.Parse(obj["kun_readings"].ToString()));
            //On
            List<string> on = JArrayToStringList(JArray.Parse(obj["on_readings"].ToString()));

            //Name
            List<string> names = JArrayToStringList(JArray.Parse(obj["name_readings"].ToString()));
            //Grade
            int grade = Int32.Parse(obj["grade"].ToString());
            //JLPT
            int jlpt = Int32.Parse(obj["jlpt"].ToString());
            //Unicode
            string unicode = obj["unicode"].ToString();
            //Heisig
            string heisig = obj["heisig_en"].ToString();
            //Strokes
            int strokes = Int32.Parse(obj["stroke_count"].ToString());

            Kanji k = new Kanji(
                obj["kanji"].ToString(),
                meanings,
                kun,
                on,
                names,
                grade,
                jlpt,
                strokes,
                unicode,
                heisig);
            return k;
        }

        public static async Task<KanjiList> DownloadKanjiSet(JArray kanjiToDownload)
        {
            //Create Object for each object, download the data.
            KanjiList newSet = new KanjiList();
            HttpClient http = new HttpClient();
            for (int i = 0; i < kanjiToDownload.Count; i++)
            {
                var k = Task.Run(async () => await DownloadKanji(http, kanjiToDownload[i].ToString())).Result;
                newSet.kanji.Add(k);
            }
            return newSet;
        }

        public static bool CheckSetAvailability(string set)
        {
            //First, check if kanji.xml is empty.
            KanjiList list = GetKanjiListFromXML(set);
            bool needToDownload = false;

            if (!(list.kanji.Count > 0))
            {
                needToDownload = true;
                //Then, download data for each Kanji for our category
                JArray ja = Task.Run(async () => await QueueKanjiSet(set)).Result;
                list = Task.Run(async () => await DownloadKanjiSet(ja)).Result;
                UpdateKanjiFile(set, list);
                //https://kanjiapi.dev/v1/kanji/grade-1
                //Take each Kanji from this list, and download the data for it.
                //Then, write that data to our XML file.
            }
            if (needToDownload)
            {
            }

            return (list.kanji.Count > 0) ? true : false;
        }
    }
}
