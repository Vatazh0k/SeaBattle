using SeaBattle.Model;
using SeaBattle.Resource;
using SeaBattle.View.Pages;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace SeaBattle.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Data

        #region Private Data
        private ShipSelectionWindow selectionWindow;
        private RulesWindow RulesWindow = new RulesWindow();
        private ObservableCollection<Ship> _ships = new ObservableCollection<Ship>();
        private Page _CurrentPage;

        private int _oneDeckShip = 4;
        private int _twoDeckShip = 3;
        private int _thireDeckShip = 2;
        private int _fourDeckShip = 1;

        private string cellNumber;

        #endregion

        #region PUblic Data
        public ObservableCollection<Ship> Ships
        {
            get => _ships;
            set => Set(ref _ships, value);
        }
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }

        public int OneDeckShip
        {
            get { return _oneDeckShip; }
            set => Set(ref _oneDeckShip, value);
        }
        public int TwoDeckShip
        {
            get { return _twoDeckShip; }
            set => Set(ref _twoDeckShip, value);
        }
        public int ThrieDeckShip
        {
            get { return _thireDeckShip; }
            set => Set(ref _thireDeckShip, value);
        }
        public int FourDeckShip
        {
            get { return _fourDeckShip; }
            set => Set(ref _fourDeckShip, value);
        }


        public ICommand NewShipAssignmentCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }
        public ICommand CreatingShipsCommand { get; set; }
        public Page LoginPage { get; set; }

        #endregion
        #endregion

        public MainWindowViewModel()
        {
            CreatingShipsCommand = new Command(CreatingShipsCommandAction, CanUseCreatingShipsCommand);
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
            NewShipAssignmentCommand = new Command(NewShipAssignmentCommandAction, CanUseNewShipAssignmentCommand);

            LoginPage = new LoginPage(this);
            CurrentPage = LoginPage;

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

        private bool CanUseNewShipAssignmentCommand(object p) => true;
        private bool CanUseExitCommand(object p) => true;
        private bool CanUseRulesCommand(object p) => true;
        private bool CanUseCreatingShipsCommand(object p)
        {
            if (CurrentPage is FieldCreatingPage)
                return true;
            return false;
        }

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
        private void CreatingShipsCommandAction(object p)
        {
            cellNumber = null;
            for (int i = 1; i < p.ToString().Length; i++)
            {
                cellNumber += p.ToString()[i];
            }

            selectionWindow = new ShipSelectionWindow(this);
            selectionWindow.ShowDialog();
        }

        private void NewShipAssignmentCommandAction(object p)
        {
            int Cell = Convert.ToInt32(cellNumber);

            if (p.ToString() is "s1")
            {
                if (OneDeckShip is 0) return;

                Ships[Cell] = new Ship
                {
                    Content = "O",
                    Color = new SolidColorBrush(Colors.Red),
                    Border = new Thickness(1)
                };
                OneDeckShip--;
            }

            selectionWindow.Close();
        }

        #endregion

        #region Private Methods

        #endregion

    }
}
