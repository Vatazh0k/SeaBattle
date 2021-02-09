using SeaBattle.BuisnessLogic;
using SeaBattle.Model;
using SeaBattle.Resource;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaBattle.ViewModel
{
    class ComputerFieldPageViewModel : ViewModelBase
    {
        #region PrivateData
        private MainWindowViewModel vm;
        private ObservableCollection<Ship> _ships;
        private Field fields;
        bool isComputerMove = false;
        private string[,] TempArr = new string[11,11];
        private const string MissedMark = "X";
        private const string ShipMark = "O";
        private const string EmptyCellMark = " ";

        private int OneDeckShipDecksCount = 1;
        private int TwoDeckShipDeksCount = 2;
        private int ThrieDeckShipDeksCount = 3;
        private int FourDeckShipDeksCount = 4;

        private int OneDeckShipCount = 4;
        private int TwoDeckShipCount = 3;
        private int thrieDeckShipCount = 2;
        private int FourDeckShipCount = 1;

        private int NumberOfRemainingUserShips = 10;
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

        public ComputerFieldPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;
            #region Commands
            MakeDamageCommand = new Command(MakeDamageCommandAction, CanUseMakeDamageCommand);
            #endregion

            var ships = Enumerable.Range(0, 121)
            .Select(i => new Ship
            {
                Content = new Image(),
                Border = new Thickness(0.5),
                isOnField = false,
                isDead = false
            });

            _ships = new ObservableCollection<Ship>(ships);

           #region ShipsGenerating
            ShipsGeneration(FourDeckShipCount, FourDeckShipDeksCount);
            ShipsGeneration(thrieDeckShipCount, ThrieDeckShipDeksCount);
            ShipsGeneration(OneDeckShipCount, OneDeckShipDecksCount);
            ShipsGeneration(TwoDeckShipCount, TwoDeckShipDeksCount);
            #endregion

            fields = CellsAssignment();
        } 

        #region Commands
        private bool CanUseMakeDamageCommand(object p) => !isComputerMove;
        private void MakeDamageCommandAction(object p)
        {
            int Cell = 0;
            string cellString = string.Empty;

            for (int i = 1; i < p.ToString().Length; i++)
            {
                cellString += p.ToString()[i];
                Cell = Convert.ToInt32(cellString);
            }

            CellIndex Indexes = SearchCellIndexes(Cell);

            bool isGameover = UserTurn(fields, Indexes, Cell);
            if (isGameover is true) return;
            //подправить код, уменшить код, 
            //сделать чтоб не убивал весь корабль, а бил индекс + 1,
            //добавить возможеость развернуть корабль.

            ComputerTurn(fields.UserField);

        }
        #endregion
         
        #region PrivateMethods
        private void ComputerTurn(string[,] userField)
        {
            int NextAttackCellNumber = 0;

            CellIndex indexes = SearchRandomCell(userField);

            int Cell = ConvertIndexesToCell(indexes);

            while (isComputerMove != false)
            {
                bool isMissed = GameProcess.DamageCreating(userField, indexes.I_index, indexes.J_index + NextAttackCellNumber);

                if (isMissed is true)
                {
                    MissCounter++;
                    isComputerMove = false;
                    MissedAction(Cell, vm.Ships, PathToShipContent.MissedMark, 0.5);
                }
                if (isMissed is false)
                {
                    int IndexOfTheFirstShpsDeck = 0;
                    NextAttackCellNumber++;

                    int DecksCount = GameProcess.CountingDecksCount(userField, indexes.I_index, indexes.J_index, ref IndexOfTheFirstShpsDeck);

                    switch (DecksCount)
                    {
                        default:
                            break;

                        case 1:
                            vm.OneDeckShip--;
                            break;
                        case 2:
                            vm.TwoDeckShip--;
                            break;
                        case 3:
                            vm.ThrieDeckShip--;
                            break;
                        case 4:
                            vm.FourDeckShip--;
                            break;

                    }

                    IndexOfTheFirstShpsDeck = indexes.J_index - IndexOfTheFirstShpsDeck;

                    for (int i = IndexOfTheFirstShpsDeck; i < DecksCount + IndexOfTheFirstShpsDeck; i++)
                    {
                        userField[indexes.I_index,i] = ShipMark;
                    }

                    userField = GameProcess.ShipsFuneral(userField, indexes.I_index, IndexOfTheFirstShpsDeck, DecksCount);

                    NumberOfRemainingUserShips--;
                    vm.Ships = ConsequencesOfAttack(userField, vm.Ships);
                    if (NumberOfRemainingUserShips is 0)
                    {
                        MessageBox.Show("You loose!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        isComputerMove = true;
                        return;
                    }


                }
            }
        }
        private bool UserTurn(Field fields, CellIndex Indexes, int Cell)
        {
            if (fields.ComputerField[Indexes.I_index, Indexes.J_index] == ShipMark ||
               fields.ComputerField[Indexes.I_index, Indexes.J_index] == MissedMark) return false;

            isComputerMove = true;

            bool isMissed = GameProcess.DamageCreating(fields.ComputerField, Indexes.I_index, Indexes.J_index);

            if (isMissed is true)
            {
                MissedAction(Cell, Ships, PathToShipContent.MissedMark, 0.5);
            }
            if (isMissed is false)
            {
                isComputerMove = false;

                bool isShipKilled = GameProcess.ChekedTheShipState(fields.ComputerField, Indexes.I_index, Indexes.J_index);

                if (isShipKilled is false)
                {
                    MissedAction(Cell, Ships, PathToShipContent.KilledShip, 0.5);
                }
                if (isShipKilled is true)
                {
                    NumberOfRemainingComputerShips--;
                    Ships = ConsequencesOfAttack(fields.ComputerField, Ships);
                    if (NumberOfRemainingComputerShips is 0)
                    {
                        MessageBox.Show("You win!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                        isComputerMove = true;
                        return true;
                    }

                }
            }
            return false;
        }
         
        private int ConvertIndexesToCell(CellIndex indexes)
        {
            int cell = 0;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i == indexes.I_index && j == indexes.J_index)
                    {
                        cell = i * 11 + j;
                        return cell;
                    }
                }
            }
            return cell;
        }
        private CellIndex SearchRandomCell(string[,] userField)
        {
            var index = new CellIndex();
            var random = new Random();

            bool IsCellEmpty = false;

            while (IsCellEmpty != true)
            {
                index.I_index = random.Next(1, 11);
                index.J_index = random.Next(1, 11);

                IsCellEmpty = CellPositionValidation(userField, index.I_index, index.J_index);

            }
            

            return index;
        }
        private bool CellPositionValidation(string[,] userField, int i, int j)
        {
            if (userField[i, j] == ShipMark || userField[i, j] == MissedMark)
            {
                return false;
            }

            return true;
        }
        private ObservableCollection<Ship> ConsequencesOfAttack(string[,] field, ObservableCollection<Ship> Ship)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (field[i, j] == MissedMark)
                    {
                        Ship[i * 11 + j] = new Ship
                        {
                            Content = new Image
                            {
                                Source = new BitmapImage(new Uri(PathToShipContent.MissedMark, UriKind.Relative)),
                                Stretch = Stretch.Fill
                            },
                            Border = new Thickness(0.5),
                        };
                    }
                    else if (field[i, j] == ShipMark)
                    {
                        Ship[i * 11 + j] = new Ship
                        {
                            Content = new Image
                            {
                                Source = new BitmapImage(new Uri(PathToShipContent.KilledShip, UriKind.Relative)),
                                Stretch = Stretch.Fill
                            },
                            Border = new Thickness(1),
                        };
                    }
                }
            }
            return Ship;
        }
        private void MissedAction(int cell, ObservableCollection<Ship> ships, string Mark, double BorderSize)
        {
            ships[cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Mark, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                Border = new Thickness(BorderSize),
            };

        }
        private Field CellsAssignment()
        {
            Field field = new Field();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if(Ships[i * 11 + j].isOnField)
                    field.ComputerField[i, j] = EmptyCellMark;
                    if (vm.Ships[i * 11 + j].isOnField)
                    field.UserField[i, j] = EmptyCellMark;
        
                }
            }
            return field;
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
        private void ShipsGeneration(int ShipCount, int DeksCount)
        {
            var Random = new Random();

            for (int i = 0; i < ShipCount; i++)
            {
                int Cell = Random.Next(11, 121);
                CellIndex Indexes = SearchCellIndexes(Cell);

                if (!ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, TempArr, DeksCount))
                {
                    i--;
                    continue;
                }
                else
                {
                    switch (DeksCount)
                    {
                        default:
                            break;

                        case 1:
                            ShipsOptions(Cell, Indexes.I_index, Indexes.J_index);
                            break;

                        case 2:
                            ShipsOptions(Cell, Indexes.I_index, Indexes.J_index);
                            ShipsOptions(Cell + 1, Indexes.I_index, Indexes.J_index + 1);
                            break;

                        case 3:
                            ShipsOptions(Cell, Indexes.I_index, Indexes.J_index);
                            ShipsOptions(Cell + 1, Indexes.I_index, Indexes.J_index + 1);
                            ShipsOptions(Cell + 2, Indexes.I_index, Indexes.J_index + 2);
                            break;

                        case 4:
                            ShipsOptions(Cell, Indexes.I_index, Indexes.J_index);
                            ShipsOptions(Cell + 1, Indexes.I_index, Indexes.J_index + 1);
                            ShipsOptions(Cell + 2, Indexes.I_index, Indexes.J_index + 2);
                            ShipsOptions(Cell + 3, Indexes.I_index, Indexes.J_index + 3);
                            break;
                    }
                }
            }
        }
        private void ShipsOptions(int Cell, int i, int j)
        {
            Ships[Cell] = new Ship
            { 
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(PathToShipContent.EmptyCell, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isOnField = true,
                Border = new Thickness(0.5)
            };
            TempArr[i, j] = ShipMark;
        }
        #endregion
    }
}
   