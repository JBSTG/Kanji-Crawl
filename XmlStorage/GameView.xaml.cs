using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XmlStorage.Classes;



namespace XmlStorage
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        KanjiList list;
        Kanji targetKanji;
        List<List<KanjiCell>> cells;
        List<KanjiCell> cellsInUse;
        int rows;
        int columns;
        int kanjiToPickFrom = 5;
        string subject = "";
        PlayerControl p;
        Random r;
        public void SetUpGameBoard(int rows, int columns) {
            this.rows = rows;
            this.columns = columns;
            cells = new List<List<KanjiCell>>();
            for (int i = 0;i<rows;i++) {
                Board.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < columns; j++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i  = 0;i<rows;i++)
            {
                cells.Add(new List<KanjiCell>());
                for (int j=0;j<columns;j++)
                {
                    KanjiCell k = new KanjiCell("  ");
                    Board.Children.Add(k);
                    Grid.SetRow(k,i);
                    Grid.SetColumn(k,j);
                    cells[i].Add(k);
                }
            }
            p = new PlayerControl();
            Board.Children.Add(p);
            Grid.SetRow(p, rows/2);
            Grid.SetColumn(p, columns/2);
        }

        public KanjiList SelectRandomKanjiSubset(KanjiList full, int amt) {
            KanjiList shortList = new KanjiList();
            for (int i = 0; i<amt;i++){
                //TODO: Search for repeats.
                int index = r.Next(0,full.kanji.Count);
                shortList.kanji.Add(full.kanji[index]);
            }
            return shortList;
        }

        public void DisperseKanji(KanjiList kl,int kanjiToDisperse) {
            //Center player
            Grid.SetColumn(p, columns / 2);
            Grid.SetRow(p, rows / 2);
            //Get Kanji to test on
            KanjiList shortList = SelectRandomKanjiSubset(kl,kanjiToDisperse);
            int co = r.Next(0, columns);
            int ro = r.Next(0, rows);
            for (int i = 0; i < kanjiToPickFrom; i++)
            {
                while (
                    (cells[ro][co].value.Text!="  ")||
                    (co==Grid.GetColumn(p)&&ro==Grid.GetRow(p))) {
                    co = r.Next(0, columns);
                    ro = r.Next(0, rows);
                }
                cells[ro][co].value.Text = shortList.kanji[i].character;
                cellsInUse.Add(cells[ro][co]);
            }
            //Set the target answer.
            int targetIndex = r.Next(0,shortList.kanji.Count);
            targetKanji = shortList.kanji[targetIndex];
            for (int i = 0;i<targetKanji.meanings.Count;i++) {
                target.Text += targetKanji.meanings[i];
                if (i<targetKanji.meanings.Count-1) {
                    target.Text += ",";
                }
                target.Text += " ";
            }

            //Set Accuracy
            if (targetKanji.correct == 0 && targetKanji.incorrect == 0)
            {
                pastAccuracy.Text = "--";
            }
            else if (targetKanji.incorrect == 0)
            {
                pastAccuracy.Text = "100%";
            }
            else {
                double percent = (targetKanji.correct) / (targetKanji.correct+targetKanji.incorrect);
                pastAccuracy.Text = Math.Floor(percent).ToString();
            }
        }
        public GameView(string kanjiSet)
        {
            r = new Random();
            InitializeComponent();
            //Create Kanji Set Object
            subject = kanjiSet;
            list = StaticKanjiOperations.GetKanjiListFromXML(kanjiSet);
            cellsInUse = new List<KanjiCell>();

            SetUpGameBoard(5, 10);
            DisperseKanji(list,5);
        }

        public void SetFocus(Object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
            this.IsEnabled = true;
            this.Visibility = Visibility.Visible;
            Keyboard.Focus(this);
            this.Focus();
            e.Handled = true;
        }
        public void KeyTest(Object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString());

            if (e.Key == Key.W && Grid.GetRow(p) > 0)
            {
                Grid.SetRow(p, Grid.GetRow(p) - 1);
            }
            if (e.Key == Key.S && Grid.GetRow(p) < rows)
            {
                Grid.SetRow(p, Grid.GetRow(p) + 1);
            }
            if (e.Key == Key.A && Grid.GetColumn(p) > 0)
            {
                Grid.SetColumn(p, Grid.GetColumn(p) - 1);
            }
            if (e.Key == Key.D && Grid.GetColumn(p) < columns)
            {
                Grid.SetColumn(p, Grid.GetColumn(p) + 1);
            }

            string cellValue = cells[Grid.GetRow(p)][Grid.GetColumn(p)].value.Text;
            if (cellValue!="  ") {
                if (cellValue == targetKanji.character)
                {
                    correctOrNot.Text = "CORRECT";
                    targetKanji.correct++;
                }
                else {
                    correctOrNot.Text = "INCORRECT";
                    targetKanji.incorrect++;
                }
                //Reset cells in use.
                for (int i = 0;i<cellsInUse.Count;i++) {
                    cellsInUse[i].value.Text = "  ";
                }
                cellsInUse = new List<KanjiCell>();
                target.Text = "";
                DisperseKanji(list, kanjiToPickFrom);
            }
            e.Handled = true;
        }
        public void ReturnToMainMenu(Object sender, RoutedEventArgs e) {
            StaticKanjiOperations.UpdateKanjiFile(subject, list);
            this.Visibility = Visibility.Collapsed;
            Visibility = Visibility.Visible;
            (Window.GetWindow(this).FindName("MenuContainer") as Grid).Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
