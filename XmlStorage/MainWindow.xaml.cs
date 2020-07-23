using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using XmlStorage.Classes;
namespace XmlStorage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


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
            Kanji k = new Kanji(obj["kanji"].ToString(), meanings, kun, on);
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

        public static bool CheckSetAvailability(string set,Label status) {
            //First, check if kanji.xml is empty.
            KanjiList list = StaticKanjiOperations.GetKanjiListFromXML(set);
            bool needToDownload = false;
            
            if (!(list.kanji.Count > 0))
            {
                needToDownload = true;
                status.Content = "Downloading "+set+" Kanji data.";
                //Then, download data for each Kanji for our category
                JArray ja = Task.Run(async () => await QueueKanjiSet(set)).Result;
                list = Task.Run(async () => await DownloadKanjiSet(ja)).Result;
                StaticKanjiOperations.UpdateKanjiFile(set, list);
                MessageBox.Show("File Updated.");
                //https://kanjiapi.dev/v1/kanji/grade-1
                //Take each Kanji from this list, and download the data for it.
                //Then, write that data to our XML file.
            }
            if (needToDownload) {
                status.Content = "Downloaded "+list.kanji.Count+" new "+set+" Kanji.";
            }
            
            return (list.kanji.Count > 0) ? true : false;
        }

        public MainWindow()
        {
            InitializeComponent();
            Keyboard.Focus(this);
        }
        public void LoadSet(object sender, RoutedEventArgs e)
        {
            string senderName = ((Button)sender).Name;
            ((Button)sender).IsEnabled = false;
            switch (senderName) {
                case ("grade1Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-1", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-1");
                    break;
                case ("grade2Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-2", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-2");
                    break;
                case ("grade3Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-3", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-3");
                    break;
                case ("grade4Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-4", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-4");
                    break;
                case ("grade5Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-5", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-5");
                    break;
                case ("grade6Button"):
                    ((Button)sender).IsEnabled = CheckSetAvailability("grade-6", label);
                    MenuContainer.Visibility = Visibility.Collapsed;
                    DataContext = new GameView("grade-6");
                    break;
                default:
                    break;
            }
        }
    }
}