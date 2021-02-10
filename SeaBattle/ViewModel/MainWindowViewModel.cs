using SeaBattle.BuisnessLogic;
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
        private string cellNumber;

        private int _oneDeckShip;
        private int _twoDeckShip;
        private int _thireDeckShip;
        private int _fourDeckShip;

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
        public Page LoginPage { get; set; }


        public ICommand NewShipAssignmentCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }
        public ICommand CreatingShipsCommand { get; set; }
        public ICommand NewGameCommand { get; set; }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Commands
            CreatingShipsCommand = new Command(CreatingShipsCommandAction, CanUseCreatingShipsCommand);
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
            NewShipAssignmentCommand = new Command(NewShipAssignmentCommandAction, CanUseNewShipAssignmentCommand);
            NewGameCommand = new Command(NewGameCommandAction, CanUseNewGameCommand);
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
        private bool CanUseNewGameCommand(object p) => true;
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
        private void NewGameCommandAction(object p)
        {
            OneDeckShip = 4;
            TwoDeckShip = 3;
            ThrieDeckShip = 2;
            FourDeckShip = 1;

            for (int i = 0; i < 121; i++)
            {
                Ships[i] = new Ship
                {
                    Content = new Image(),
                    isOnField = false,
                    Border = new Thickness(0.5)
                };
            }
            CurrentPage = new FieldCreatingPage(this);
        }
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
                    if (OneDeckShip is 0) break;
                    ShipsCountValidation(Cell, 1);
                    break;

                case "s2":
                    if (TwoDeckShip is 0) break;
                    ShipsCountValidation(Cell, 2);
                    break;

                case "s3":
                    if (ThrieDeckShip is 0) break;
                    ShipsCountValidation(Cell, 3);
                    break;

                case "s4":
                    if (FourDeckShip is 0) break;
                    ShipsCountValidation(Cell, 4);
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
        private void ShipsCountValidation(int Cell, int DeckCount)
        {

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
                        OneDeckShip--;
                        break;

                    case 2:
                        ShipOptions(PathToShipContent.TwoDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.TwoDeckShip_SecondDeck, Cell+1, 0,1,1,1);
                        TwoDeckShip--;
                        break;


                    case 3:
                        ShipOptions(PathToShipContent.ThrieDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.ThrieDeckShip_SecondDeck, Cell+1, 0,1,0,1);
                        ShipOptions(PathToShipContent.ThrieDeckShip_ThirdDeck, Cell+2, 0,1,1,1);
                        ThrieDeckShip--;
                        break;
                    case 4:
                        ShipOptions(PathToShipContent.FourDeckShip_FirstDeck, Cell, 1,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_SecondDeck, Cell+1, 0,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_ThirdDeck, Cell+2, 0,1,0,1);
                        ShipOptions(PathToShipContent.FourDeckShip_FourDeck, Cell+3, 0,1,1,1);
                        FourDeckShip--;
                        break;
                        #endregion
                }
            }
        }
        #endregion

    } 
}  
   