using SeaBattle.Resource;
using SeaBattle.View.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModel
{
    class StartMenuePageViewModel
    {
        #region Data
        private MainWindowViewModel mainWindowViewModel;
        private Page FieldCreatingPage;

        public ICommand HelpCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand StartCommand { get; set; }
        #endregion

        public StartMenuePageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            #region Commands
            HelpCommand = new Command(HelpCommandAction, CanUseCommand);
            SettingsCommand = new Command(SettingsCommandAction, CanUseCommand);
            StartCommand = new Command(StartCommandAction, CanUseCommand);
            #endregion
        }

        #region Commands
        private bool CanUseCommand(object p) => true;


        private void HelpCommandAction(object p)
        {//TODO:
        
        }

        private void SettingsCommandAction(object p)
        {//TODO:

        }

        private void StartCommandAction(object p)
        {
            FieldCreatingPage = new FieldCreatingPage(mainWindowViewModel);
            mainWindowViewModel.OneDeckShip = 4;
            mainWindowViewModel.TwoDeckShip = 3;
            mainWindowViewModel.ThrieDeckShip = 2;
            mainWindowViewModel.FourDeckShip = 1;
            mainWindowViewModel.CurrentPage = FieldCreatingPage;
        }
        #endregion
    }
} 
 