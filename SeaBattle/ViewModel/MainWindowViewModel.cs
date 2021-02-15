using SeaBattle.BuisnessLogic;
using SeaBattle.Infrastructure.Converters;
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
        private ObservableCollection<Brush> _color;
        private ObservableCollection<Ship> _ships;
        private Page _CurrentPage;
        private string cellNumber;
        private string shipsDecksCount;

        private int _oneDeckShip;
        private int _twoDeckShip;
        private int _thireDeckShip;
        private int _fourDeckShip;

        #endregion

        #region PUblic Data
        public IEnumerable<SolidColorBrush> colors { get; }
        public IEnumerable<Ship> ships { get; }

        public ObservableCollection<Brush> Color
        {
            get => _color;
            set => Set(ref _color, value);
        }
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
        public Page StartMenue { get; set; }


        public ICommand DragCommand { get; set; }
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
            DragCommand = new Command(DragCommandAction, CanUseDragCommand);
            CreatingShipsCommand = new Command(ShipsWindowOpeningAction, CanUseCreatingShipsCommand);
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
            NewShipAssignmentCommand = new Command(ShipAssignmentCommandAction, CanUseNewShipAssignmentCommand);
            NewGameCommand = new Command(NewGameCommandAction, CanUseNewGameCommand);
            #endregion
            StartMenue = new StartMenuePage(this);
            CurrentPage = StartMenue;

            colors = Enumerable.Range(0, 121).Select(i => new SolidColorBrush(Colors.White));
            ships = Enumerable.Range(0, 121).Select(i => new Ship());

            _color = new ObservableCollection<Brush>(colors);
            _ships = new ObservableCollection<Ship>(ships);
           
        }

        #region CanUseCommands
        private bool CanUseDragCommand(object p) => true;
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
        private void DragCommandAction(object p)
        {
            Color = new ObservableCollection<Brush>(colors);
            Label lb = p as Label;
            shipsDecksCount = lb.Name.ToString();
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);

        }
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
        private void ShipsWindowOpeningAction(object p)
        {
            Color = new ObservableCollection<Brush>(colors);
            cellNumber = null;

            for (int i = 1; i < p.ToString().Length; i++)
                cellNumber += p.ToString()[i];

            if (Ships[Convert.ToInt32(cellNumber)].isOnField is true)
            {
                ChangeShipsDirection(Convert.ToInt32(cellNumber));
                return;
            }

            selectionWindow = new ShipSelectionWindow(this);
            selectionWindow.ShowDialog();
        }
        private void ShipAssignmentCommandAction(object p)
        {
            int Cell = Convert.ToInt32(cellNumber);

            SearchShipsType(Cell, p.ToString());

            selectionWindow.Close();
        }

        #endregion

        #region Private Methods
        private void ChangeShipsDirection(int CellNumber)
        {
            Color = new ObservableCollection<Brush>(colors);
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(CellNumber);

            int CurrentDeck = 0;
            string[,] tempArr = new string[11, 11];
            tempArr = CellsAssignment(tempArr, Ships);

            int DecksInShipCount = GameProcess.CountingDecks
            (tempArr, indexes.I_index, indexes.J_index, ref CurrentDeck, Ships[CellNumber].isHorizontal);

            int FirstDeckOfHorizontalShip = indexes.J_index - CurrentDeck;
            int FirstDeckOfVerticalShip = indexes.I_index - CurrentDeck;

            if (Ships[CellNumber].isHorizontal is true)
                for (int i = 0; i < DecksInShipCount; i++)
                    tempArr[indexes.I_index, FirstDeckOfHorizontalShip + i] = null;

            if (Ships[CellNumber].isHorizontal is false)
                for (int i = 0; i < DecksInShipCount; i++)
                    tempArr[FirstDeckOfVerticalShip + i, indexes.J_index] = null;

            bool canPutShip = ShipPositionValidation.PositionValidationLogic
            (indexes.I_index, indexes.J_index, tempArr, DecksInShipCount, !Ships[CellNumber].isHorizontal);

            if (canPutShip is false)
            {
                if(Ships[CellNumber].isHorizontal is true)
                CantPutVerticalShipHint(DecksInShipCount, indexes, tempArr, CellNumber);

                if(Ships[CellNumber].isHorizontal is false)
                CantPutHorizontalShipHint(DecksInShipCount, indexes, tempArr, CellNumber);
                return;
            }
            if (canPutShip is true)
            {
                if (Ships[CellNumber].isHorizontal is true)
                {
                    int Cell = CellNumber - CurrentDeck;

                    for (int i = 0; i < DecksInShipCount; i++)
                        Ships[Cell + i] = new Ship();

                    CreatingVerticalShip(DecksInShipCount, CellNumber);
                    return;
                }
                if (Ships[CellNumber].isHorizontal is false)
                {
                    int Cell = CellNumber;
                    switch (CurrentDeck)
                    {
                        default:
                            break;

                        case 0:
                            Cell -= 11;
                            break;

                        case 1:
                            Cell -= 22;
                            break;
                        case 2:
                            Cell -= 33;
                            break;
                        case 3:
                            Cell -= 44;
                            break;

                    }

                    for (int i = 0; i < DecksInShipCount; i++)
                        Ships[Cell += 11] = new Ship();

                    switch (DecksInShipCount)
                    {
                        default:
                            break;

                        case 1:
                            OneDeckShip++;
                            break;
                        case 2:
                            TwoDeckShip++;
                            break;
                        case 3:
                            ThrieDeckShip++;
                            break;
                        case 4:
                            FourDeckShip++;
                            break;

                    }

                    CreatingHorizontalShip(DecksInShipCount, CellNumber);
                    return;
                }
            }

        }
        private void SearchShipsType(int cell, string ComparableString)
        {

            if (ComparableString is "s1" || ComparableString is "OneDeckShip")
            {
                if (OneDeckShip is 0) return;
                DirectionValidation(cell, 1);
            }
            else if (ComparableString is "s2" || ComparableString is "DoubleDeckShip")
            {
                if (TwoDeckShip is 0) return;
                DirectionValidation(cell, 2);
            }
            else if (ComparableString is "s3" || ComparableString is "ThrieDeckShip")
            {
                if (ThrieDeckShip is 0) return;
                DirectionValidation(cell, 3);
            }
            else if (ComparableString is "s4" || ComparableString is "FourDeckShip")
            {
                if (FourDeckShip is 0) return;
                DirectionValidation(cell, 4);
            }

        }
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
        private void ShipOptions(string Path, int Cell, int left, int top, int right, int bottom, bool Direction = true)
        {
            Ships[Cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isHorizontal = Direction,
                isOnField = true,
                Border = new Thickness(left,top,right,bottom),
            };
        }
        private void DirectionValidation(int Cell, int DeckCount)
        {

            string[,] tempArr = new string[11, 11];

            tempArr = CellsAssignment(tempArr, Ships);
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

            if (!ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, tempArr, DeckCount, Ships[Cell].isHorizontal))
            {
                CantPutHorizontalShipHint(DeckCount, Indexes, tempArr, Cell);
            }
            else
            {
                CreatingHorizontalShip(DeckCount, Cell);
            }
        }
        private void CantPutHorizontalShipHint(int DeckCount, CellIndex Indexes, string[,] tempArr, int Cell)
        {
            try
            {
                for (int i = 0; i < DeckCount; i++)
                {
                    tempArr[Indexes.I_index, Indexes.J_index + i] = "";
                    Color[Cell + i] = new SolidColorBrush(Colors.Red);
                    Color[Cell + i].Opacity = 0.2;
                }

            }
            catch (Exception)
            {
                return;
            }
        }
        private void CantPutVerticalShipHint(int DeckCount, CellIndex Indexes, string[,] tempArr, int Cell)
        {
            try
            {
                for (int i = 0; i < DeckCount; i++)
                {
                    tempArr[Indexes.I_index + i, Indexes.J_index] = "";
                    Color[Cell] = new SolidColorBrush(Colors.Red);
                    Color[Cell].Opacity = 0.2;
                    Cell += 11;
                }

            }
            catch (Exception)
            {
                return;
            }
        }
        private void CreatingHorizontalShip(int Deck, int Cell)
        {
            switch (Deck)
            {

                default:
                    break;

                case 1:
                    ShipOptions(PathToShipContent.OneDeckShip, Cell, 1, 1, 1, 1);
                    OneDeckShip--;
                    break;

                case 2:
                    ShipOptions(PathToShipContent.TwoDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                    ShipOptions(PathToShipContent.TwoDeckShip_SecondDeck, Cell + 1, 0, 1, 1, 1);
                    TwoDeckShip--;
                    break;


                case 3:
                    ShipOptions(PathToShipContent.ThrieDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                    ShipOptions(PathToShipContent.ThrieDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1);
                    ShipOptions(PathToShipContent.ThrieDeckShip_ThirdDeck, Cell + 2, 0, 1, 1, 1);
                    ThrieDeckShip--;
                    break;
                case 4:
                    ShipOptions(PathToShipContent.FourDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                    ShipOptions(PathToShipContent.FourDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1);
                    ShipOptions(PathToShipContent.FourDeckShip_ThirdDeck, Cell + 2, 0, 1, 0, 1);
                    ShipOptions(PathToShipContent.FourDeckShip_FourDeck, Cell + 3, 0, 1, 1, 1);
                    FourDeckShip--;
                    break;

            }
        }
        private void CreatingVerticalShip(int Decks, int CellNumber)
        {
            switch (Decks)
            {

                default:
                    break;

                case 1:
                    ShipOptions(PathToShipContent.Vertical_OneDeckShip, CellNumber, 1, 1, 1, 1, false);
                    break;

                case 2:
                    ShipOptions(PathToShipContent.Vertical_TwoDeckShip_FirstDeck, CellNumber, 1, 1, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_TwoDeckShip_SecondDeck, CellNumber + 11, 1, 0, 1, 1, false);
                    break;


                case 3:
                    ShipOptions(PathToShipContent.Vertical_ThrieDeckShip_FirstDeck, CellNumber, 1, 1, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_ThrieDeckShip_SecondDeck, CellNumber + 11, 1, 0, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_ThrieDeckShip_ThirdDeck, CellNumber + 22, 1, 0, 1, 1, false);
                    break;
                case 4:
                    ShipOptions(PathToShipContent.Vertical_FourDeckShip_FirstDeck, CellNumber, 1, 1, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_FourDeckShip_SecondDeck, CellNumber + 11, 1, 0, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_FourDeckShip_ThirdDeck, CellNumber + 22, 1, 0, 1, 0, false);
                    ShipOptions(PathToShipContent.Vertical_FourDeckShip_FourDeck, CellNumber + 33, 1, 0, 1, 1, false);
                    break;

            }
        }
        public void DropAction(object sender, DragEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            string CellsNumber = "";
            int cell = 0;

            for (int i = 1; i < feSource.Name.Length; i++)
                CellsNumber += feSource.Name[i];

            bool isParsed = int.TryParse(CellsNumber, out cell);
            if (isParsed is false) return;

            SearchShipsType(cell, shipsDecksCount);
        }
        #endregion

    }   
}   
   