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
        private void DragCommandAction(object p)
        {
            Color = new ObservableCollection<Brush>(colors);
            Label lb = p as Label;
            string ShipsName = lb.Name.ToString();
            switch (ShipsName)
            {
                default:
                    break;
                case "FourDeckShip":
                    shipsDecksCount = 4;
                    break;
                case "ThrieDeckShip":
                    shipsDecksCount = 3;
                    break;
                case "DoubleDeckShip":
                    shipsDecksCount = 2;
                    break;
                case "OneDeckShip":
                    shipsDecksCount = 1;
                    break;
            }
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
                // ChangeShipsDirection(Convert.ToInt32(cellNumber));
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

        #endregion

        #region Private Methods
        private void SearchShipsType(int cell, string ComparableString)
        {

            if (ComparableString is "s1" || ComparableString is "OneDeckShip")
            {
                if (OneDeckShip is 0) return;
                CanPutShip(cell, 1);
            }
            else if (ComparableString is "s2" || ComparableString is "DoubleDeckShip")
            {
                if (TwoDeckShip is 0) return;
                CanPutShip(cell, 2);
            }
            else if (ComparableString is "s3" || ComparableString is "ThrieDeckShip")
            {
                if (ThrieDeckShip is 0) return;
                CanPutShip(cell, 3);
            }
            else if (ComparableString is "s4" || ComparableString is "FourDeckShip")
            {
                if (FourDeckShip is 0) return;
                CanPutShip(cell, 4);
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
        private ObservableCollection<Ship> ShowShips(int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(cell, PathToShipContent.OneDeckShip);
                    OneDeckShip--;
                    break;
                case 2:
                    ShipsOptions(cell, PathToShipContent.TwoDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.TwoDeckShip_SecondDeck);
                    TwoDeckShip--;
                    break;
                case 3:
                    ShipsOptions(cell, PathToShipContent.ThrieDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.ThrieDeckShip_SecondDeck);
                    ShipsOptions(cell + 2, PathToShipContent.ThrieDeckShip_ThirdDeck);
                    ThrieDeckShip--;
                    break;
                case 4:
                    ShipsOptions(cell, PathToShipContent.FourDeckShip_FirstDeck);
                    ShipsOptions(cell + 1, PathToShipContent.FourDeckShip_SecondDeck);
                    ShipsOptions(cell + 2, PathToShipContent.FourDeckShip_ThirdDeck);
                    ShipsOptions(cell + 3, PathToShipContent.FourDeckShip_FourDeck);
                    FourDeckShip--;
                    break;

            }


            return Ships;
        }
        private bool CanPutShip(int Cell, int DecksCount)
        {
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

            bool CanPutShip = UsersField.CanPutShip(UsersField.field, Indexes.I_index, Indexes.J_index, DecksCount);

            if (CanPutShip is true)
            {
                for (int i = 0; i < DecksCount; i++)
                {
                    UsersField.field[Indexes.I_index, Indexes.J_index + i] = ShipsMark;
                }
                ShowShips(DecksCount, Cell);
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
        public void DragTheShipOnTheFieldAction(object sender, MouseButtonEventArgs e)
        {
            Color = new ObservableCollection<Brush>(colors);
            Button lb = sender as Button;
            shipsDecksCount = CountingDecks(lb.Name);
            DragDrop.DoDragDrop(lb, lb.Content, DragDropEffects.Copy);
        }
        public void DropAction(object sender, DragEventArgs e)
        {
            FrameworkElement feSource = e.Source as FrameworkElement;
            string CellsNumber = "";
            int NewCell = 0;

            for (int i = 1; i < feSource.Name.Length; i++)
                CellsNumber += feSource.Name[i];

            bool isParsed = int.TryParse(CellsNumber, out NewCell);
            if (isParsed is false) return;

            if (cellNumber != null)
            {
                CellIndex PreviousCellIndexes = CellsConverter.ConverCellsToIndexes(Convert.ToInt32(cellNumber));

                int FirstDecksIndex = 0;
                int DecksCount = GameProcess.CountingDecks
                (UsersField.field, PreviousCellIndexes.I_index, PreviousCellIndexes.J_index,
                ref FirstDecksIndex, Ships[Convert.ToInt32(cellNumber)].isHorizontal);

       
                if (Ships[Convert.ToInt32(cellNumber)].isHorizontal is false)
                {
                    PreviousCellIndexes.I_index -= FirstDecksIndex;

                    for (int i = 0; i < DecksCount; i++)
                    {
                        Ships[Convert.ToInt32(cellNumber) + i*11] = new Ship();
                        UsersField.field[PreviousCellIndexes.I_index+i, PreviousCellIndexes.J_index] = null;
                    }
                }
                else
                {
                    PreviousCellIndexes.J_index -= FirstDecksIndex;

                    for (int i = 0; i < DecksCount; i++)
                    {
                        Ships[Convert.ToInt32(cellNumber) + i] = new Ship();
                        UsersField.field[PreviousCellIndexes.I_index, PreviousCellIndexes.J_index + i] = null;
                    }
                }

            }

            bool canPutShip = CanPutShip(NewCell, shipsDecksCount);

            if (canPutShip is false)
            { 
                //redline
            }
           
        }   
        #endregion

    }
}  
  