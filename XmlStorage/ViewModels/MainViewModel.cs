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
            LoadSettingsCommand = new BaseCommand(LoadSettings);
            ReturnToMenuCommand = new BaseCommand(ReturnToMenu);
            ChangeThemeCommand = new BaseCommand(ChangeTheme);
            ChangeGridLinesCommand = new BaseCommand(ChangeGridLines);
        }

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

        private int _Theme = Main.Default.SelectedTheme;
        public int Theme {
            get
            {
                return _Theme;
            }
            set
            {
                _Theme = value;
                NotifyPropertyChanged();
            }
        }
        private bool _GridLinesVisible = Main.Default.GridLinesVisible;
        public bool GridLinesVisible {
            get
            {
                return _GridLinesVisible;
            }
            set
            {
                _GridLinesVisible = value;
                NotifyPropertyChanged();
            }
        }
        public void LoadGame(object parameter)
        {
            GameViewModel gm = new GameViewModel(parameter.ToString());
            CurrentViewModel = gm;
        }

        public void LoadSettings(object parameter)
        {
            SettingsViewModel sm = new SettingsViewModel();
            CurrentViewModel = sm;
        }

        public void ReturnToMenu(object parameter)
        {
            //TODO: Add database updates.
            MenuViewModel mm = new MenuViewModel();
            CurrentViewModel = mm;
        }

        public void ChangeTheme(object parameter)
        {
            Main.Default.SelectedTheme = Int32.Parse(parameter.ToString());
            Main.Default.Save();
            Theme = Int32.Parse(parameter.ToString());
        }

        public void ChangeGridLines(object parameter)
        {
            Main.Default.GridLinesVisible = bool.Parse(parameter.ToString());
            Main.Default.Save();
            GridLinesVisible = Main.Default.GridLinesVisible;
        }

        public ICommand LoadGameCommand { get; set; }
        public ICommand LoadSettingsCommand { get; set; }
        public ICommand ReturnToMenuCommand { get; set; }
        public ICommand ChangeThemeCommand { get; set; }
        public ICommand ChangeGridLinesCommand { get; set; }

    }
}
