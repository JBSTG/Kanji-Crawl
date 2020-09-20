using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using XmlStorage.Commands;
using XmlStorage.Models;

namespace XmlStorage.ViewModels
{
    class MainViewModel:BaseModel
    {
        public MainViewModel()
        {
            CurrentViewModel = new MenuViewModel();
            LoadGameCommand = new BaseCommand(LoadGame);
        }


        public void LoadGame(object parameter)
        {
            GameViewModel gm = new GameViewModel(parameter.ToString());
            CurrentViewModel = gm;
        }

        public ICommand LoadGameCommand { get; set; }

        private BaseModel _CurrentViewModel;
        public BaseModel CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set
            {
                _CurrentViewModel = value;
                Debug.WriteLine("VM Changed");
                NotifyPropertyChanged();
            }
        }
    }
}
