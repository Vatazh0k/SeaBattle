using SeaBattle.Model;
using SeaBattle.Resource;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace SeaBattle.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {

        #region Data

        #region Private Data
        private RulesWindow RulesWindow = new RulesWindow();
        private ObservableCollection<Ship> _ships = new ObservableCollection<Ship>();

        #endregion

        #region PUblic Data
        public ObservableCollection<Ship> Ships
        {
            get => _ships;
            set => Set(ref _ships, value);
        }

        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }

        #endregion
        #endregion

        public MainWindowViewModel()
        {
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);


            for (int i = 0; i < 121; i++)
            {
                Ships.Add(new Ship()
                {
                    Content = "",
                    Color = new SolidColorBrush(Colors.White),
                    Border = new Thickness(0.5)
                });
            }
        }

        #region CanUseCommands

        private bool CanUseExitCommand(object p) => true;
        private bool CanUseRulesCommand(object p) => true;

        #endregion

        #region Commands Actions
        private void ExitCommandAction(object p)
        {
            Environment.Exit(0);
        }
        private void RulesCommandAction(object p)
        {
            RulesWindow.ShowDialog();
        }

        #endregion

        #region Private Methods

        #endregion

    }
}
