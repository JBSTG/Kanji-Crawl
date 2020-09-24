using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using XmlStorage.Commands;
using XmlStorage.Models;
using XmlStorage.Utility;

namespace XmlStorage.ViewModels
{
    class GameViewModel:BaseModel
    {
        public GameViewModel(string initialSet)
        {
            SetName = initialSet;
            KanjiList = DataAccess.LoadKanjiFromSet(initialSet);
            Cells = GameViewLogic.ZeroOutGrid(20,10);
            ShortList = GameViewLogic.CreateKanjiShortList(KanjiList,5);
            TargetCharacter = GameViewLogic.SelectTargetKanji(ShortList);
            MeaningListDisplay = GameViewLogic.MeaningListToString(TargetCharacter);
            PlayerLocation = GameViewLogic.SetPlayer(Cells);
            GameViewLogic.DisperseKanji(Cells,ShortList);

            //Command Initialization
            ArrowCommand = new BaseCommand(Move);
            SpaceOrEnterCommand = new BaseCommand(HideCorrectionScreen);
        }



        private string _SetName;
        private string _MeaningListDisplay;
        private KanjiModel _TargetCharacter;
        private string _SelectedCharacter;
        private KanjiListModel _ShortList;
        private ObservableCollection<ObservableCollection<string>> _Cells;
        private KanjiListModel _KanjiList;
        private int[] _PlayerLocation;
        private bool _CorrectionScreenVisible = false;
        public string SetName
        {
            get
            {
                return _SetName;
            }
            set
            {
                _SetName = value;
            }
        }
        public string MeaningListDisplay {
            get
            {
                return _MeaningListDisplay;
            }
            set
            {
                _MeaningListDisplay = value;
                NotifyPropertyChanged();
            } 
        }
        public KanjiModel TargetCharacter { get; set; }
        public string SelectedCharacter
        {
            get
            {
                return _SelectedCharacter;
            }
            set
            {
                _SelectedCharacter = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<ObservableCollection<string>> Cells 
        {
            get
            {
                return _Cells;
            }
            set
            {
                _Cells = value;
                Debug.WriteLine("Cells Changed");
                NotifyPropertyChanged();
            } 
        }
        public KanjiListModel KanjiList { get; set; }
        public KanjiListModel ShortList { get; set; }
        public int[] PlayerLocation
        { 
            get
            {
                return _PlayerLocation;
            }
            set
            {
                _PlayerLocation = value;
                NotifyPropertyChanged();
            }
        }

        public bool CorrectionScreenVisible
        {
            get
            {
                return _CorrectionScreenVisible;
            }
            set
            {
                _CorrectionScreenVisible = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ArrowCommand { get; set; }
        public ICommand SpaceOrEnterCommand { get; set; }
        public void Move(object parameter)
        {
            string direction = parameter.ToString();
            int x = PlayerLocation[0];
            int y = PlayerLocation[1];
            if (direction=="UP")
            {
                if (y>0)
                {
                    Cells[x][y] = String.Empty;
                    y = (--PlayerLocation[1]);

                    if (Cells[x][y] != String.Empty)
                    {
                        SelectedCharacter = Cells[x][y];
                        CheckAnswer();
                    }
                    else
                    {
                        Cells[x][y] = "👺";
                    }
                }
            }
            if (direction == "LEFT")
            {
                if (x > 0)
                {
                    Cells[x][y] = String.Empty;
                    x = (--PlayerLocation[0]);

                    if (Cells[x][y] != String.Empty)
                    {
                        SelectedCharacter = Cells[x][y];
                        CheckAnswer();
                    }
                    else
                    {
                        Cells[x][y] = "👺";
                    }
                }
            }
            if (direction == "RIGHT")
            {
                if (x < Cells.Count-1)
                {
                    Cells[x][y] = String.Empty;
                    x = (++PlayerLocation[0]);

                    if (Cells[x][y] != String.Empty)
                    {
                        SelectedCharacter = Cells[x][y];
                        CheckAnswer();
                    }
                    else
                    {
                        Cells[x][y] = "👺";
                    }
                }
            }
            if (direction == "DOWN")
            {
                if (y < Cells[0].Count-1)
                {
                    Cells[x][y] = String.Empty;
                    y = (++PlayerLocation[1]);

                    if (Cells[x][y] != String.Empty)
                    {
                        SelectedCharacter = Cells[x][y];
                        CheckAnswer();
                    }
                    else
                    {
                        Cells[x][y] = "👺";
                    }
                }
            }
        }

        private void HideCorrectionScreen(object obj)
        {
            CorrectionScreenVisible = false;
        }

        public void ResetGame()
        {
            Cells = GameViewLogic.ZeroOutGrid(20, 10);
            ShortList = GameViewLogic.CreateKanjiShortList(KanjiList, 5);
            TargetCharacter = GameViewLogic.SelectTargetKanji(ShortList);
            MeaningListDisplay = GameViewLogic.MeaningListToString(TargetCharacter);
            PlayerLocation = GameViewLogic.SetPlayer(Cells);
            GameViewLogic.DisperseKanji(Cells, ShortList);
        }

        public void CheckAnswer()
        {
            int x = PlayerLocation[0];
            int y = PlayerLocation[1];

            if (Cells[x][y] == TargetCharacter.Character)
            {
                MessageBox.Show("Good Job!");
            }
            else
            {
                CorrectionScreenVisible = true;
            }
            ResetGame();
        }
    }
}
