using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            GameViewLogic.SetPlayer(Cells);
            Debug.WriteLine(ShortList.Kanji.Count);
            GameViewLogic.DisperseKanji(Cells,ShortList);
        }
        private string _SetName;
        private string _MeaningListDisplay;
        private KanjiModel _TargetCharacter;
        private KanjiListModel _ShortList;
        private List<List<string>> _Cells;
        private KanjiListModel _KanjiList;
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
        public string MeaningList { get; set; }
        public KanjiModel TargetCharacter { get; set; }
        public List<List<string>> Cells { get; set; }
        public KanjiListModel KanjiList { get; set; }
        public KanjiListModel ShortList { get; set; }
    }
}
