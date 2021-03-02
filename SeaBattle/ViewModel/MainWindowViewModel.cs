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
        private const string ShipsMark = "O";

        private string cellNumber;
        private int shipsDecksCount;
        private bool shipsDirection = true;
        private int FirstDecksIndex;

        Field UsersField;
        Field ComputerField;


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

            UsersField = new Field();
            ComputerField = new Field();

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

            cellNumber = null;

            SearchShipsType(Cell, p.ToString());

            selectionWindow.Close();
        }
        private void DragCommandAction(object p)
        {
            Color = new ObservableCollection<Brush>(colors);
            Label lb = p as Label;
            string ShipsName = lb.Name.ToString();
            cellNumber = null;
            switch (ShipsName)
            {
                default:
                    break;
                case "FourDeckShip":
                    shipsDecksCount = 4;
                    if (FourDeckShip is 0) return;
                    break;
                case "ThrieDeckShip":
                    shipsDecksCount = 3;
                    if (ThrieDeckShip is 0) return;
                    break;
                case "DoubleDeckShip":
                    shipsDecksCount = 2;
                    if (TwoDeckShip is 0) return;
                    break;
                case "OneDeckShip":
                    shipsDecksCount = 1;
                    if (OneDeckShip is 0) return;
                    break;
            }
            shipsDirection = true;
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);

        }
        #endregion

        #region Private Methods
        private void ChangeShipsDirection(int CellNumber)
        {
            Color = new ObservableCollection<Brush>(colors);
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(CellNumber);
            int CurrentDeck = 0;

            int DecksInShipCount = GameProcess.CountingDecks
            (UsersField.field, indexes.I_index, indexes.J_index, ref CurrentDeck, Ships[CellNumber].isHorizontal);

            int FirstDeckOfHorizontalShip = indexes.J_index - CurrentDeck;
            int FirstDeckOfVerticalShip = indexes.I_index - CurrentDeck;

            if (Ships[CellNumber].isHorizontal is true)
                for (int i = 0; i < DecksInShipCount; i++)
                    UsersField.field[indexes.I_index, FirstDeckOfHorizontalShip + i] = null;

            if (Ships[CellNumber].isHorizontal is false)
                for (int i = 0; i < DecksInShipCount; i++)
                    UsersField.field[FirstDeckOfVerticalShip + i, indexes.J_index] = null;

            bool canPutShip = UsersField.CanPutShip
            (UsersField.field, indexes.I_index, indexes.J_index, DecksInShipCount, !Ships[CellNumber].isHorizontal);

            if (canPutShip is false)
            {
                if (Ships[CellNumber].isHorizontal is true)
                    for (int i = 0; i < DecksInShipCount; i++)
                        UsersField.field[indexes.I_index, FirstDeckOfHorizontalShip + i] = ShipsMark;

                if (Ships[CellNumber].isHorizontal is false)
                    for (int i = 0; i < DecksInShipCount; i++)
                        UsersField.field[FirstDeckOfVerticalShip + i, indexes.J_index] = ShipsMark;//МЕТОДОМ!!! переставлять верт корабли
                //if (Ships[CellNumber].isHorizontal is true)
                //    CantPutVerticalShipHint(DecksInShipCount, indexes, UsersField.field, CellNumber);

                //if (Ships[CellNumber].isHorizontal is false)
                //    CantPutHorizontalShipHint(DecksInShipCount, indexes, UsersField.field, CellNumber);
                return;
            }
            if (canPutShip is true)
            {
                if (Ships[CellNumber].isHorizontal is true)
                {
                    int Cell = CellNumber - CurrentDeck;

                    for (int i = 0; i < DecksInShipCount; i++)
                    {
                        Ships[Cell + i] = new Ship();
                    }

                    ShowVerticalShips(DecksInShipCount, CellNumber);
                    for (int i = 0; i < DecksInShipCount; i++)
                    {
                        UsersField.field[indexes.I_index + i, indexes.J_index ] = ShipsMark;
                    }
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
                    {
                        Ships[Cell += 11] = new Ship();
                    }

                    ShowHorizontalShips(DecksInShipCount, CellNumber);
                    for (int i = 0; i < DecksInShipCount; i++)
                    {
                        UsersField.field[indexes.I_index, indexes.J_index + i] = ShipsMark;
                    }
                    return;
                }
            }

        }
        private void SearchShipsType(int cell, string ComparableString)
        {

            if (ComparableString is "s1" || ComparableString is "OneDeckShip")
            {
                if (OneDeckShip is 0) return;
                if (CanPutShip(cell, 1)) ReduceShipsCount(1);
            }
            else if (ComparableString is "s2" || ComparableString is "DoubleDeckShip")
            {
                if (TwoDeckShip is 0) return;
                if (CanPutShip(cell, 2)) ReduceShipsCount(2);
            }
            else if (ComparableString is "s3" || ComparableString is "ThrieDeckShip")
            {
                if (ThrieDeckShip is 0) return;
                if (CanPutShip(cell, 3)) ReduceShipsCount(3);
            }
            else if (ComparableString is "s4" || ComparableString is "FourDeckShip")
            {
                if (FourDeckShip is 0) return;
                if (CanPutShip(cell, 4)) ReduceShipsCount(4);
            }

        }
        private void ShipsOptions(int cell, string Path, bool Direction = true)
        {
            Ships[cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isHorizontal = Direction,
                isOnField = true,
                Border = new Thickness(1),
            };
        }
        private void ReduceShipsCount(int DeckCount)
        {
            switch (DeckCount)
            {
                default:
                    break;

                case 1:
                    OneDeckShip--;
                    break;
                case 2:
                    TwoDeckShip--;
                    break;
                case 3:
                    ThrieDeckShip--;
                    break;
                case 4:
                    FourDeckShip--;
                    break;
            }
        }
        private void ReduceColor(int Cell, bool direction = true)
        {
            if(direction is true)
            for (int i = 0; i < shipsDecksCount; i++)
            {
                Color[Cell + i] = new SolidColorBrush(Colors.White);
                Color[Cell + i].Opacity = 1;
            }

            if (direction is false)
            for (int i = 0; i < shipsDecksCount; i++)
            {
                Color[Cell + i*11] = new SolidColorBrush(Colors.White);
                Color[Cell + i*11].Opacity = 1;
            }

        }
        private ObservableCollection<Ship> ShowVerticalShips(int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(cell, PathToShipContent.Vertical_OneDeckShip, false);
                    break;
                case 2:
                    ShipsOptions(cell, PathToShipContent.Vertical_TwoDeckShip_FirstDeck, false);
                    ShipsOptions(cell + 11, PathToShipContent.Vertical_TwoDeckShip_SecondDeck, false);
                    break;
                case 3:
                    ShipsOptions(cell, PathToShipContent.Vertical_ThrieDeckShip_FirstDeck, false);
                    ShipsOptions(cell + 11, PathToShipContent.Vertical_ThrieDeckShip_SecondDeck, false);
                    ShipsOptions(cell + 22, PathToShipContent.Vertical_ThrieDeckShip_ThirdDeck, false);
                    break;
                case 4:
                    ShipsOptions(cell, PathToShipContent.Vertical_FourDeckShip_FirstDeck, false);
                    ShipsOptions(cell + 11, PathToShipContent.Vertical_FourDeckShip_SecondDeck, false);
                    ShipsOptions(cell + 22, PathToShipContent.Vertical_FourDeckShip_ThirdDeck, false);
                    ShipsOptions(cell + 33, PathToShipContent.Vertical_FourDeckShip_FourDeck, false);
                    break;

            }

            return Ships;
        }
        private ObservableCollection<Ship> ShowHorizontalShips(int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(cell, PathToShipContent.OneDeckShip);
                    break;
                case 2:
                    ShipsOptions(cell, PathToShipContent.TwoDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.TwoDeckShip_SecondDeck);
                    break;
                case 3:
                    ShipsOptions(cell, PathToShipContent.ThrieDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.ThrieDeckShip_SecondDeck);
                    ShipsOptions(cell + 2, PathToShipContent.ThrieDeckShip_ThirdDeck);
                    break;
                case 4:
                    ShipsOptions(cell, PathToShipContent.FourDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.FourDeckShip_SecondDeck);
                    ShipsOptions(cell + 2, PathToShipContent.FourDeckShip_ThirdDeck);
                    ShipsOptions(cell + 3, PathToShipContent.FourDeckShip_FourDeck);
                    break;

            }


            return Ships;
        } 
        private bool CanPutShip(int Cell, int DecksCount, int FirstDeckIndex = 0, bool Direction = true)
        {
            Cell -= FirstDeckIndex;
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);
            bool CanPutShip = UsersField.CanPutShip(UsersField.field, Indexes.I_index, Indexes.J_index, DecksCount, Direction);

            if (CanPutShip is true)
            {
                if (Direction is true)
                {
                    for (int i = 0; i < DecksCount; i++)
                        UsersField.field[Indexes.I_index, Indexes.J_index + i] = ShipsMark;

                    ShowHorizontalShips(DecksCount, Cell);
                }
                if (Direction is false)
                {
                    for (int i = 0; i < DecksCount; i++)
                        UsersField.field[Indexes.I_index + i, Indexes.J_index] = ShipsMark;

                    ShowVerticalShips(DecksCount, Cell);
                }
                return true;

            }
            return false;
        }
        private int CountingDecks(string CellNumber)
        {
            cellNumber = null;

            for (int i = 1; i < CellNumber.Length; i++)
                cellNumber += CellNumber[i];

            int Cell = Convert.ToInt32(cellNumber);

            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

            int FirstDecksIndex = 0;
            int DecksCount = GameProcess.CountingDecks(UsersField.field, Indexes.I_index, Indexes.J_index, ref FirstDecksIndex, Ships[Cell].isHorizontal);



            return DecksCount;
        }
        private void DeleteShip(int cell, CellIndex indexes, int firstDeckIndex, int DecksCount)
        {
            if (Ships[cell].isHorizontal is false)
            {
                indexes.I_index -= firstDeckIndex;
                cell = cell - firstDeckIndex * 11;

                for (int i = 0; i < DecksCount; i++)
                {
                    Ships[cell + i * 11] = new Ship();
                    UsersField.field[indexes.I_index + i, indexes.J_index] = null;
                }
            }
            else
            {
                indexes.J_index -= firstDeckIndex;
                cell = cell - firstDeckIndex;

                for (int i = 0; i < DecksCount; i++)
                {
                    Ships[cell + i] = new Ship();
                    UsersField.field[indexes.I_index, indexes.J_index + i] = null;
                }
            }
        }
        public void DragTheShipOnTheFieldAction(object sender, MouseButtonEventArgs e)
        {
            Color = new ObservableCollection<Brush>(colors);
            Button lb = sender as Button;
            shipsDecksCount = CountingDecks(lb.Name);
            if (lb.Content is null) return;
            shipsDirection = Ships[Convert.ToInt32(cellNumber)].isHorizontal;
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);

        }
        private int SearchCell(string CellName)
        {
            string CellsNumber = "";
            int NewCell = 0;

            for (int i = 1; i < CellName.Length; i++)
                CellsNumber += CellName[i];

            bool isParsed = int.TryParse(CellsNumber, out NewCell);
            if (isParsed is false) return -1;

            return NewCell;
        }
        public void DropAction(object sender, DragEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            int PreviousCell = Convert.ToInt32(cellNumber);
            CellIndex PreviousCellIndexes = CellsConverter.ConverCellsToIndexes(Convert.ToInt32(PreviousCell));

            int NewCell = SearchCell(feSource.Name);
            if (NewCell is -1)
            {
                CanPutShip(PreviousCell, shipsDecksCount, FirstDecksIndex, shipsDirection);
                return;
            }

            if (cellNumber != null)
            {
                bool _canPutShip = CanPutShip(NewCell, shipsDecksCount, 0, shipsDirection);
               
                if (_canPutShip is false)
                {
                    CanPutShip(PreviousCell, shipsDecksCount, FirstDecksIndex, shipsDirection);
                }

                ReduceColor(NewCell, Ships[PreviousCell].isHorizontal);
                cellNumber = null;
                return;
            }

            bool canPutShip = CanPutShip(NewCell, shipsDecksCount, 0, Ships[PreviousCell].isHorizontal);
            if (canPutShip is true)
            {
                ReduceShipsCount(shipsDecksCount);
                ReduceColor(NewCell, shipsDirection);
            }
            if (canPutShip is false)
            {
                //redline
            }
        }
        public void DragLeave(object sender, DragEventArgs e)
        {   
            if (cellNumber != null)
            {
                CellIndex _cellIndexes = CellsConverter.ConverCellsToIndexes(Convert.ToInt32(cellNumber));

                if (UsersField.field[_cellIndexes.I_index, _cellIndexes.J_index] is null)
                    goto CheckThePositionValidation;
                int DecksCount = GameProcess.CountingDecks
                (UsersField.field, _cellIndexes.I_index, _cellIndexes.J_index, ref FirstDecksIndex, Ships[Convert.ToInt32(cellNumber)].isHorizontal);
                DeleteShip(Convert.ToInt32(cellNumber), _cellIndexes, FirstDecksIndex, DecksCount);
            }
            CheckThePositionValidation:
            FrameworkElement feSource = e.Source as FrameworkElement;

            int cell = SearchCell(feSource.Name);
            if (cell is -1) return;

            CellIndex cellIndexes = CellsConverter.ConverCellsToIndexes(cell);

            if (shipsDirection is true)
            {
                for (int i = 0; i < shipsDecksCount; i++)
                {
                    if (cellIndexes.J_index + i == 11) break;
                    Color[cell + i] = new SolidColorBrush(Colors.White);
                    Color[cell + i].Opacity = 1;

                }
            }
            if (shipsDirection is false)
            {
                for (int i = 0; i < shipsDecksCount; i++)
                {
                    if (cellIndexes.I_index + i == 11) break;
                    Color[cell + i*11] = new SolidColorBrush(Colors.White);
                    Color[cell + i*11].Opacity = 1;

                }
            }
        }
        public void DragEnter(object sender, DragEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;

            int cell = SearchCell(feSource.Name);
            if (cell is -1) return;

            CellIndex cellIndexes = CellsConverter.ConverCellsToIndexes(cell);


            if (shipsDirection is true)
            {
                for (int i = shipsDecksCount - 1; i >= 0; i--)
                {
                    if (!UsersField.CanPutShip(UsersField.field, cellIndexes.I_index, cellIndexes.J_index, shipsDecksCount))
                    {

                        for (int j = 0; j < shipsDecksCount; j++)
                        {
                            if (cellIndexes.J_index + j == 11) break;
                            Color[cell + j] = new SolidColorBrush(Colors.Red);
                            Color[cell + j].Opacity = 0.4;
                        }
                        return;
                    }
                    Color[cell + i] = new SolidColorBrush(Colors.Green);
                    Color[cell + i].Opacity = 0.4;
                }
            }
            if (shipsDirection is false)
            {
                for (int i = shipsDecksCount - 1; i >= 0; i--)
                {
                    if (!UsersField.CanPutShip(UsersField.field, cellIndexes.I_index, cellIndexes.J_index, shipsDecksCount, false))
                    {

                        for (int j = 0; j < shipsDecksCount; j++)
                        {
                            if (cellIndexes.I_index + j == 11) break;
                            Color[cell + j*11] = new SolidColorBrush(Colors.Red);
                            Color[cell + j*11].Opacity = 0.4;
                        }
                        return;
                    }
                    Color[cell + i*11] = new SolidColorBrush(Colors.Green);
                    Color[cell + i*11].Opacity = 0.4;
                }
            }
        }
        #endregion

    }
}  
  