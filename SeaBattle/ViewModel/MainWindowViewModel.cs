﻿using SeaBattle.BuisnessLogic;
using SeaBattle.Model;
using SeaBattle.Resource;
using SeaBattle.View.Pages;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaBattle.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Data

        #region Private Data
        private ShipSelectionWindow selectionWindow;
        private RulesWindow RulesWindow = new RulesWindow();
        private ObservableCollection<Ship> _ships;
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
            #region Commands
            CreatingShipsCommand = new Command(CreatingShipsCommandAction, CanUseCreatingShipsCommand);
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
            NewShipAssignmentCommand = new Command(NewShipAssignmentCommandAction, CanUseNewShipAssignmentCommand);
            #endregion

            LoginPage = new LoginPage(this);
            CurrentPage = LoginPage;

            var ships = Enumerable.Range(0, 121)
            .Select(i => new Ship
            {
                Content = new Image(),
                isOnField = false,
                Border = new Thickness(0.5)
            });

            _ships = new ObservableCollection<Ship>(ships);

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
            switch (p.ToString())
            {
                default:
                    break;

                case "s1":
                    ShipsCountValidation(ref _oneDeckShip, Cell, 1);
                    break;

                case "s2":
                    ShipsCountValidation(ref _twoDeckShip, Cell, 2);
                    break;

                case "s3":
                    ShipsCountValidation(ref _thireDeckShip, Cell, 3);
                    break;

                case "s4":
                    ShipsCountValidation(ref _fourDeckShip, Cell, 4);
                    break;


            }


            selectionWindow.Close();
        }

        #endregion

        #region Private Methods
        private string[,] CellsAssignment(string[,] tempArr, ObservableCollection<Ship> Ships)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    tempArr[i, j] = null;
                    if (Ships[i * 11 + j].isOnField == true)
                    {
                        tempArr[i, j] = "O";
                    }
                }
            }
            return tempArr;
        }
        private CellIndex SearchCellIndexes(int cell)
        {
            CellIndex indexes = new CellIndex();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i * 11 + j == cell)
                    {
                        indexes.I_index = i;
                        indexes.J_index = j;
                    }
                }
            }
            
            return indexes;
        }
        private void ShipOptions(string Path, int Cell, int left, int top, int right, int bottom)
        {
            Ships[Cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isOnField = true,
                Border = new Thickness(left,top,right,bottom),
            };
        }
        private void ShipsCountValidation(ref int Ship, int Cell, int DeckCount)
        {
            if (Ship is 0) return;

            string[,] tempArr = new string[11, 11];

            tempArr = CellsAssignment(tempArr, Ships);
            CellIndex Indexes = SearchCellIndexes(Cell);

            if (!ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, tempArr, DeckCount))
            {
                MessageBox.Show("You can create ship here!", "Error",
                  MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                switch (DeckCount)
                {
                   
                    default:
                        break;
                    #region ShipsCreating
                    case 1:
                        ShipOptions(PathToShipContent.OneDeckShip, Cell, 1,1,1,1);
                        break;

                    case 2:
                        ShipOptions(PathToShipContent.TwoDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.TwoDeckShip_SecondDeck, Cell+1, 0,1,1,1);
                        break;


                    case 3:
                        ShipOptions(PathToShipContent.ThrieDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.ThrieDeckShip_SecondDeck, Cell+1, 0,1,0,1);
                        ShipOptions(PathToShipContent.ThrieDeckShip_ThirdDeck, Cell+2, 0,1,1,1);
                        break;
                    case 4:
                        ShipOptions(PathToShipContent.FourDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_SecondDeck, Cell+1, 0,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_ThirdDeck, Cell+2, 0,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_FourDeck, Cell+3, 0,1,1,1);
                        break;
                        #endregion
                }

                Ship--;
            }
        }
        #endregion

    }
} 
   