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
        private Field UsersField;
        private Field ComputerField;

        private const string ShipsMark = "O";
        private string cellNumber;
        private int shipsDecksCount;
        private bool shipsDirection = true;
        private bool isDroped = true;
        private int FirstDecksIndex;


        private int _oneDeckShip;
        private int _twoDeckShip;
        private int _thireDeckShip;
        private int _fourDeckShip;
        #endregion

        #region PUblic Data
        public Page StartMenue { get; set; }
        public ComputerFieldPage ComputerFieldPage { get; set; }
        public IEnumerable<SolidColorBrush> colors { get; }
        public IEnumerable<Ship> ships { get; }
        #region Commands
        public ICommand ShipsAutoGeneration { get; set; }
        public ICommand ReadyCommand { get; set; }
        public ICommand CleanFieldCommand { get; set; }
        public ICommand DragCommand { get; set; }
        public ICommand NewShipAssignmentCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }
        public ICommand CreatingShipsCommand { get; set; }
        public ICommand NewGameCommand { get; set; }
        #endregion
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

            ShipsAutoGeneration = new Command(ShipsAutoGenerationAction, CanUseCommands);
            ReadyCommand = new Command(ReadyCommandAction, CanUseCommands);
            CleanFieldCommand = new Command(CleanFieldCommandAction, CanUseCommands);
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
        private bool CanUseCommands(object p) => true;
        #endregion

        #region Commands Actions
        private void ShipsAutoGenerationAction(object p)
        {
            CleanField();
            UsersField.FieldAutoGeneration(UsersField.field);
            Ships = ShipsAssignment(UsersField.field);
            CleanShips();
        }
        private void CleanFieldCommandAction(object p)
        {
            CleanField();
        }
        private void ReadyCommandAction(object p)
        {

            if (OneDeckShip is 0 && TwoDeckShip is 0 &&
              ThrieDeckShip is 0 && FourDeckShip is 0)
            {
                ShipsReplenishment();
                ComputerFieldPage = new ComputerFieldPage(UsersField, ComputerField, this);
                CurrentPage = ComputerFieldPage;
            }
            else
                MessageBox.Show("Please input all ships", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void NewGameCommandAction(object p)
        {
            ComputerField = new Field();
            UsersField = new Field();
            CleanField();
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
            int Cell = SearchCell(p.ToString());
            cellNumber = Cell.ToString();

            if (Ships[Cell].isOnField is true)
            {
                if (UsersField.ChangeShipsDirection(UsersField.field, Cell))
                    ChangeShipsDirection(Cell);
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
                default: break;
                case "FourDeckShip":
                    shipsDecksCount = 4;
                    if (FourDeckShip is 0) return; break;
                case "ThrieDeckShip":
                    shipsDecksCount = 3;
                    if (ThrieDeckShip is 0) return; break;
                case "DoubleDeckShip":
                    shipsDecksCount = 2;
                    if (TwoDeckShip is 0) return; break;
                case "OneDeckShip":
                    shipsDecksCount = 1;
                    if (OneDeckShip is 0) return; break;
            }
            shipsDirection = true;
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);

        }
        #endregion

        #region Private Methods
        private int GetCell(int i, int j)
        {
            return i * 11 + j;
        }
        private void CleanField()
        {
            ShipsReplenishment();
            UsersField.field = UsersField.CleanField(UsersField.field);

            Color = new ObservableCollection<Brush>(_color);

            Ships = new ObservableCollection<Ship>(ships);
            CurrentPage = new FieldCreatingPage(this);
        }
        private ObservableCollection<Ship> ShipsAssignment(string[,] Field)
        {
            int DecksCount;
            bool Direction;
            CellIndex indexes = new CellIndex();
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Ships[GetCell(i, j)].isOnField is true) continue;
                    if (Field[i, j] is ShipsMark)
                    {
                        int FirstDeckInde = 0;
                        indexes.I_index = i;
                        indexes.J_index = j;
                        Direction = UsersField.DeterminingTheDirection(i, j, UsersField.field);
                        DecksCount = UsersField.CountingDecks(UsersField.field, indexes, ref FirstDeckInde, Direction);
                        if (Direction)
                        {
                            ShowHorizontalShips(DecksCount, GetCell(i, j));
                        }
                        if (!Direction)
                        {
                            ShowVerticalShips(DecksCount, GetCell(i, j));
                        }
                    }
                }
            }

            return Ships;
        }
        private void ShipsReplenishment()
        {
            FourDeckShip = 1;
            ThrieDeckShip = 2;
            TwoDeckShip = 3;
            OneDeckShip = 4;
        }
        private void CleanShips()
        {
            FourDeckShip = 0;
            ThrieDeckShip = 0;
            TwoDeckShip = 0;
            OneDeckShip = 0;
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
        private void ReduceShipsCount(int DeckCount)
        {
            switch (DeckCount)
            {
                default: break;

                case 1: OneDeckShip--; break;
                case 2: TwoDeckShip--; break;
                case 3: ThrieDeckShip--; break;
                case 4: FourDeckShip--; break;
            }
        }
        private bool CanPutShip(int Cell, int DecksCount, int FirstDeckIndex = 0, bool Direction = true)
        {
            if (Direction is true)
                Cell -= FirstDeckIndex;
            else
                Cell -= FirstDeckIndex * 11;

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
            int DecksCount = UsersField.CountingDecks(UsersField.field, Indexes, ref FirstDecksIndex, Ships[Cell].isHorizontal);



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
        private void ChangeShipsDirection(int CellNumber)
        {
            int firstDecksIndex = 0;
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(CellNumber);
            int DecksCount = UsersField.CountingDecks(UsersField.field, Indexes, ref firstDecksIndex, Ships[CellNumber].isHorizontal);

            if (Ships[CellNumber].isHorizontal)
            {
                int Cell = CellNumber - firstDecksIndex;
                firstDecksIndex = Indexes.J_index - firstDecksIndex;
                for (int i = 0; i < DecksCount; i++)
                {
                    UsersField.field[Indexes.I_index, firstDecksIndex + i] = null;
                    Ships[Cell + i] = new Ship();
                }
                for (int i = 0; i < DecksCount; i++)
                {
                    UsersField.field[Indexes.I_index + i, Indexes.J_index] = ShipsMark;
                }
                ShowVerticalShips(DecksCount, CellNumber);
                return;
            }

            if (!Ships[CellNumber].isHorizontal)
            {
                int Cell = CellNumber - firstDecksIndex * 11;
                firstDecksIndex = Indexes.I_index - firstDecksIndex;
                for (int i = 0; i < DecksCount; i++)
                {
                    UsersField.field[firstDecksIndex + i, Indexes.J_index] = null;
                    Ships[Cell + GetCell(i, 0)] = new Ship();
                }
                for (int i = 0; i < DecksCount; i++)
                {
                    UsersField.field[Indexes.I_index, Indexes.J_index + i] = ShipsMark;
                }
                ShowHorizontalShips(DecksCount, CellNumber);
                return;
            }


        }

        #region ColorEffects
        private void ShowGrinHint(int cell, int i_index, int Direction = 1)
        {
            Color[cell + i_index * Direction] = new SolidColorBrush(Colors.Green);
            Color[cell + i_index * Direction].Opacity = 0.4;
        }
        private void ShowRedHint(int cell, int index, int Direction = 1)
        {
            for (int j = 0; j < shipsDecksCount; j++)
            {
                if (index + j == 11) break;
                Color[cell + j * Direction] = new SolidColorBrush(Colors.Red);
                Color[cell + j * Direction].Opacity = 0.4;
            }
        }
        private void ReduceColor(int Cell, bool direction = true)
        {
            if (direction is true)
                for (int i = 0; i < shipsDecksCount; i++)
                {
                    if (Cell + i >= 121) return;
                    Color[Cell + i] = new SolidColorBrush(Colors.White);
                    Color[Cell + i].Opacity = 1;
                }

            if (direction is false)
                for (int i = 0; i < shipsDecksCount; i++)
                {
                    if (Cell + GetCell(i, 0) > 121) return;
                    Color[Cell + GetCell(i, 0)] = new SolidColorBrush(Colors.White);
                    Color[Cell + GetCell(i, 0)].Opacity = 1;
                }

        }
        #endregion

        #region ShowShips
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
                ContentPath = Path,
                isOnField = true,
                Border = new Thickness(1),
            };
        }
        private ObservableCollection<Ship> ShowVerticalShips(int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(cell, PathToShipContent.VerticalShips[1], false);
                    break;
                case 2:
                    ShipsOptions(cell, PathToShipContent.VerticalShips[2], false);
                    ShipsOptions(cell + 11, PathToShipContent.VerticalShips[3], false);
                    break;
                case 3:
                    ShipsOptions(cell, PathToShipContent.VerticalShips[4], false);
                    ShipsOptions(cell + 11, PathToShipContent.VerticalShips[5], false);
                    ShipsOptions(cell + 22, PathToShipContent.VerticalShips[6], false);
                    break;
                case 4:
                    ShipsOptions(cell, PathToShipContent.VerticalShips[7], false);
                    ShipsOptions(cell + 11, PathToShipContent.VerticalShips[8], false);
                    ShipsOptions(cell + 22, PathToShipContent.VerticalShips[9], false);
                    ShipsOptions(cell + 33, PathToShipContent.VerticalShips[10], false);
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
                    ShipsOptions(cell, PathToShipContent.HorizontalShips[1]);
                    break;
                case 2:
                    ShipsOptions(cell, PathToShipContent.HorizontalShips[2]);
                    ShipsOptions(cell + 1, PathToShipContent.HorizontalShips[3]);
                    break;
                case 3:
                    ShipsOptions(cell, PathToShipContent.HorizontalShips[4]);
                    ShipsOptions(cell + 1, PathToShipContent.HorizontalShips[5]);
                    ShipsOptions(cell + 2, PathToShipContent.HorizontalShips[6]);
                    break;
                case 4:
                    ShipsOptions(cell, PathToShipContent.HorizontalShips[7]);
                    ShipsOptions(cell + 1, PathToShipContent.HorizontalShips[8]);
                    ShipsOptions(cell + 2, PathToShipContent.HorizontalShips[9]);
                    ShipsOptions(cell + 3, PathToShipContent.HorizontalShips[10]);
                    break;

            }


            return Ships;
        }
        #endregion

        #region DragDrop
        public void DragTheShipOnTheFieldAction(object sender, MouseButtonEventArgs e)
        {
            Color = new ObservableCollection<Brush>(colors);
            Button lb = sender as Button;
            shipsDecksCount = CountingDecks(lb.Name);
            if (lb.Content is null) return;
            isDroped = false;
            shipsDirection = Ships[Convert.ToInt32(cellNumber)].isHorizontal;
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);
            if (isDroped is false)
                switch (shipsDecksCount)
                {
                    default: break;
                    case 4: FourDeckShip++; break;
                    case 3: ThrieDeckShip++; break;
                    case 2: TwoDeckShip++; break;
                    case 1: OneDeckShip++; break;
                }
        }
        public void DropAction(object sender, DragEventArgs e)
        {
            isDroped = true;
            FrameworkElement feSource = e.Source as FrameworkElement;
            int PreviousCell = Convert.ToInt32(cellNumber);
            CellIndex PreviousCellIndexes = CellsConverter.ConverCellsToIndexes(Convert.ToInt32(PreviousCell));
            Color = new ObservableCollection<Brush>(colors);

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

                ReduceColor(NewCell, shipsDirection);
                cellNumber = null;
                return;
            }

            bool canPutShip = CanPutShip(NewCell, shipsDecksCount, 0, shipsDirection);
            if (canPutShip is true)
            {
                ReduceShipsCount(shipsDecksCount);
                ReduceColor(NewCell, shipsDirection);
            }

        }
        public void DragLeave(object sender, DragEventArgs e)
        {
            if (cellNumber != null)
            {
                int Cell = Convert.ToInt32(cellNumber);
                CellIndex _cellIndexes = CellsConverter.ConverCellsToIndexes(Cell);

                if (UsersField.field[_cellIndexes.I_index, _cellIndexes.J_index] is null)
                    goto PositionValidation;
                int DecksCount = UsersField.CountingDecks
                (UsersField.field, _cellIndexes, ref FirstDecksIndex, Ships[Cell].isHorizontal);
                DeleteShip(Convert.ToInt32(cellNumber), _cellIndexes, FirstDecksIndex, DecksCount);
            }
        PositionValidation:
            FrameworkElement feSource = e.Source as FrameworkElement;

            int cell = SearchCell(feSource.Name);
            if (cell is -1) return;

            CellIndex cellIndexes = CellsConverter.ConverCellsToIndexes(cell);

            ReduceColor(cell, shipsDirection);
        }
        public void DragEnter(object sender, DragEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            int cell = SearchCell(feSource.Name);
            if (cell is -1) return;
            CellIndex cellIndexes = CellsConverter.ConverCellsToIndexes(cell);

            if (shipsDirection is true)
            for (int i = shipsDecksCount - 1; i >= 0; i--)
            {
                if (!UsersField.CanPutShip(UsersField.field, cellIndexes.I_index, cellIndexes.J_index, shipsDecksCount))
                {
                    ShowRedHint(cell, cellIndexes.J_index);
                    return;
                }
                ShowGrinHint(cell, i);
            }
            if (shipsDirection is false)
            for (int i = shipsDecksCount - 1; i >= 0; i--)
            {
                if (!UsersField.CanPutShip(UsersField.field, cellIndexes.I_index, cellIndexes.J_index, shipsDecksCount, false))
                {
                    ShowRedHint(cell, cellIndexes.I_index, 11);
                    return;
                }
                ShowGrinHint(cell, i, 11);
            }

        }
        #endregion
        #endregion

    }
}        