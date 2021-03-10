using SeaBattle.BuisnessLogic;
using SeaBattle.Infrastructure.Converters;
using SeaBattle.Model;
using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SeaBattle.ViewModel
{
    public class ComputerFieldPageViewModel : ViewModelBase
    {
        #region PrivateData
        private MainWindowViewModel vm;
        private ObservableCollection<Ship> _ships;
        private Field UserField;
        private Field ComputerField;
        private ComputerIntelligence ComputerIntelligence;
        private CellIndex _indexes;


        private bool GameInProcess = true;
        private bool isComputerMove = false;
        private string[,] TempArr = new string[11, 11];
        private const string KilledMark = "X";
        private const string ShipMark = "O";
        private const string MissedMark = " ";



        private int _NumberOfRemainingComputerShips = 10;
        private int _missedCounter = 0;
        #endregion

        #region PublicData
        public int MissCounter
        {
            get { return _missedCounter; }
            set => Set(ref _missedCounter, value);
        }

        public int NumberOfRemainingComputerShips
        {
            get => _NumberOfRemainingComputerShips;
            set => Set(ref _NumberOfRemainingComputerShips, value);
        }
        public ObservableCollection<Ship> Ships
        {
            get => _ships;
            set => Set(ref _ships, value);
        }
        #endregion

        #region Commands
        public ICommand MakeDamageCommand { get; set; }
        #endregion  

        public ComputerFieldPageViewModel(Field UserField, Field ComputerField, MainWindowViewModel vm)
        {
            this.vm = vm;
            this.UserField = UserField;
            this.ComputerField = ComputerField;
            ComputerIntelligence = new ComputerIntelligence(UserField, ComputerField);

            #region Commands
            MakeDamageCommand = new Command(MakeDamageCommandAction, CanUseMakeDamageCommand);
            #endregion

            var ships = Enumerable
            .Range(0, 121)
            .Select(i => new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(PathToShipContent.EmptyCell, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                Border = new Thickness(0.5)
            });

            _ships = new ObservableCollection<Ship>(ships);

            ComputerField.field = ComputerIntelligence.FieldAutoGeneration();
        }

        #region Commands
        private bool CanUseMakeDamageCommand(object p) => !isComputerMove && GameInProcess;
        private void MakeDamageCommandAction(object p)
        {
            int Cell = SearchCell(p.ToString());
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(Cell);

            if (ComputerField.CanMakeDamage(Cell) is false)
                return;

            ComputerField.field = ComputerField.Attck(indexes);
            Ships = ComputersShipsAssignment(ComputerField.field, Cell);

            if (isComputerMove is false)
                return;

            UserField.field = ComputerIntelligence.ComputerAttack();
            UsersShipsAssignment(UserField.field);

        }
        #endregion

        #region PrivateMethods
        private void UsersShipsAssignment(string[,] field)
        {
            bool isMissed = true;
            Task.Run(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (vm.Ships[GetCell(i, j)].isDead is false && field[i, j] is KilledMark)
                        {
                            isMissed = false;
                            vm.AttackHint = ShowAttackHint(i, j);
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                ShowKilledShip(i, j);
                                ReduceTheShipsCount();
                            }), DispatcherPriority.Normal);
                            Thread.Sleep(500);
                        }
                    }
                }

                if(isMissed)
                Thread.Sleep(700);
                MissedMarkAssignment(UserField.field, isMissed);
            });
            return;
        }
        private int SearchCell(string cell)
        {
            int Cell = 0;
            string cellString = string.Empty;

            for (int i = 1; i < cell.Length; i++)
            {
                cellString += cell[i];
                Cell = Convert.ToInt32(cellString);
            }
            return Cell;
        }
        private string ShowAttackHint(int i, int j)
        {
            char[] Alphabet = new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            vm.OpacityAttackHint = 1;
            return "Last attack in cell " + i + Alphabet[j];
        }
        private ObservableCollection<Ship> ComputersShipsAssignment(string[,] field, int cell)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);

            if (Ships[GetCell(indexes.I_index, indexes.J_index)].isOnField is false && field[indexes.I_index, indexes.J_index] is MissedMark)
            {
                ShipsOptions(Ships, GetCell(indexes.I_index, indexes.J_index), PathToShipContent.MissedMark, 0.5);
                isComputerMove = true;
                MissCounter++;
                return Ships;
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Ships[GetCell(i, j)].isDead is false && field[i, j] is MissedMark)
                    {
                        ShipsOptions(Ships, GetCell(i, j), PathToShipContent.MissedMark, 0.5);
                        continue;
                    }
                    if (Ships[GetCell(i, j)].isDead is false && field[i, j] is KilledMark)
                    {
                        isComputerMove = false;
                        int FirstDecksIndex = 0;
                        bool Direction = ComputerField.DeterminingTheDirection(indexes.I_index, indexes.J_index);
                        int DecksCount = ComputerField.CountingDecks(indexes, ref FirstDecksIndex);
                        bool isKilled = IsKilled(DecksCount, indexes, FirstDecksIndex, field, Direction);

                        if (isKilled is false) ShipsOptions(Ships, GetCell(i, j), PathToShipContent.KilledShip, 1);
                        if (isKilled is true)
                        {
                            int Cell = GetCell(i, j);
                            _ = Direction is true ?
                                ShowHorizontalShips(Ships, DecksCount, Cell - FirstDecksIndex) :
                                ShowDeadVerticalShips(Ships, DecksCount, Cell - FirstDecksIndex * 11);
                            NumberOfRemainingComputerShips--;
                            if (NumberOfRemainingComputerShips is 0)
                            {
                                MessageBox.Show("You win!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                                GameInProcess = false;
                            }
                        }
                    }
                }
            }

            return Ships;
        }
        private bool IsKilled(int DecksCount, CellIndex indexes, int FirstDecksIndex, string[,] field, bool Direction)
        {
            bool isKilled = false;
            for (int k = 0; k < DecksCount; k++)
            {
                int I = indexes.I_index;
                int J = indexes.J_index;
                _ = Direction is true ? J += k - FirstDecksIndex : I += k - FirstDecksIndex;
                if (I > 10 || J > 10) continue;
                if (field[I, J] is KilledMark)
                {
                    isKilled = true;
                    continue;
                }
                isKilled = false;
                break;
            }
            return isKilled;
        }
        private int GetCell(int i, int j)
        {
            return i * 11 + j;
        }
        #region Show Ships
        private void ShowKilledShip(int i, int j)
        {
            int Key = 0;
            if (vm.Ships[GetCell(i, j)].isHorizontal is true)
            {
                Key = PathToShipContent.HorizontalShips
                .FirstOrDefault(x => x.Value == vm.Ships[GetCell(i, j)].ContentPath).Key;

                ShipsOptions(vm.Ships, GetCell(i, j), PathToShipContent.Horizontal_Dead_Ships[Key], 1);
            }
            if (vm.Ships[GetCell(i, j)].isHorizontal is false)
            {
                Key = PathToShipContent.VerticalShips
                .FirstOrDefault(x => x.Value == vm.Ships[GetCell(i, j)].ContentPath).Key;

                ShipsOptions(vm.Ships, GetCell(i, j), PathToShipContent.Vertical_Dead_Ships[Key], 1);
            }

        }
        private void ReduceTheShipsCount()
        {
            vm.OneDeckShip = UserField.OneDeckShip;
            vm.TwoDeckShip = UserField.TwoDeckShip;
            vm.ThrieDeckShip = UserField.ThrieDeckShip;
            vm.FourDeckShip = UserField.FourDeckShip;
        }
        private void ShipsOptions(ObservableCollection<Ship> Ships, int cell, string Path, double Thickness, bool Direction = true)
        {
            Ships[cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isHorizontal = Direction,
                isDead = true,
                Border = new Thickness(Thickness),
            };
        }
        private ObservableCollection<Ship> ShowDeadVerticalShips(ObservableCollection<Ship> Ships, int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(Ships, cell, PathToShipContent.Vertical_Dead_OneDeckShip, 1, false);
                    break;
                case 2:
                    ShipsOptions(Ships, cell, PathToShipContent.Vertical_Dead_TwoDeckShip_FirstDeck, 1, false);
                    ShipsOptions(Ships, cell + 11, PathToShipContent.Vertical_Dead_TwoDeckShip_SecondDeck, 1, false);
                    break;
                case 3:
                    ShipsOptions(Ships, cell, PathToShipContent.Vertical_Dead_ThrieDeckShip_FirstDeck, 1, false);
                    ShipsOptions(Ships, cell + 11, PathToShipContent.Vertical_Dead_ThrieDeckShip_SecondDeck, 1, false);
                    ShipsOptions(Ships, cell + 22, PathToShipContent.Vertical_Dead_ThrieDeckShip_ThirdDeck, 1, false);
                    break;
                case 4:
                    ShipsOptions(Ships, cell, PathToShipContent.Vertical_Dead_FourDeckShip_FirstDeck, 1, false);
                    ShipsOptions(Ships, cell + 11, PathToShipContent.Vertical_Dead_FourDeckShip_SecondDeck, 1, false);
                    ShipsOptions(Ships, cell + 22, PathToShipContent.Vertical_Dead_FourDeckShip_ThirdDeck, 1, false);
                    ShipsOptions(Ships, cell + 33, PathToShipContent.Vertical_Dead_FourDeckShip_FourDeck, 1, false);
                    break;

            }

            return Ships;
        }
        private ObservableCollection<Ship> ShowHorizontalShips(ObservableCollection<Ship> Ships, int DecksCount, int cell)
        {
            switch (DecksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(Ships, cell, PathToShipContent.Dead_OneDeckShip, 1);
                    break;
                case 2:
                    ShipsOptions(Ships, cell, PathToShipContent.Dead_TwoDeckShip_FirstDeck, 1);
                    ShipsOptions(Ships, cell + 1, PathToShipContent.Dead_TwoDeckShip_SecondDeck, 1);
                    break;
                case 3:
                    ShipsOptions(Ships, cell, PathToShipContent.Dead_ThrieDeckShip_FirstDeck, 1);
                    ShipsOptions(Ships, cell + 1, PathToShipContent.Dead_ThrieDeckShip_SecondDeck, 1);
                    ShipsOptions(Ships, cell + 2, PathToShipContent.Dead_ThrieDeckShip_ThirdDeck, 1);
                    break;
                case 4:
                    ShipsOptions(Ships, cell, PathToShipContent.Dead_FourDeckShip_FirstDeck, 1);
                    ShipsOptions(Ships, cell + 1, PathToShipContent.Dead_FourDeckShip_SecondDeck, 1);
                    ShipsOptions(Ships, cell + 2, PathToShipContent.Dead_FourDeckShip_ThirdDeck, 1);
                    ShipsOptions(Ships, cell + 3, PathToShipContent.Dead_FourDeckShip_FourDeck, 1);
                    break;

            }


            return Ships;
        }
        private void MissedMarkAssignment(string[,] field, bool isMissed)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (vm.Ships[GetCell(i, j)].isDead is false && field[i, j] is MissedMark)
                        {
                            if (isMissed is true) vm.AttackHint = ShowAttackHint(i, j);
                            ShipsOptions(vm.Ships, GetCell(i, j), PathToShipContent.MissedMark, 0.5);
                        }
                    }
                }

                if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
                  vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
                {
                    MessageBox.Show("You lose!", "Try Again", MessageBoxButton.OK, MessageBoxImage.Information);
                    GameInProcess = false;
                    return;
                }
                isComputerMove = false;
            }));
        }

        #endregion
        #endregion
    }
}

