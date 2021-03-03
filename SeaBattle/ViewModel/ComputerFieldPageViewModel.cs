using SeaBattle.BuisnessLogic;
using SeaBattle.Infrastructure.Converters;
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
using System.Windows.Threading;

namespace SeaBattle.ViewModel
{
    public class ComputerFieldPageViewModel : ViewModelBase
    {
        #region PrivateData
        private MainWindowViewModel vm;
        private ObservableCollection<Ship> _ships;
        private Field fields;
        private CellIndex _indexes;

        int CountOfAttaksInOneDirection = 0;
        private bool isHitButNotKilled = false;
        private bool isHorizontal = false;
        private bool isRightDirection = true;
        private bool isUpwardDirection = false;
        private int Fixed_i = 0;
        private int Fixed_j = 0;
        private int Fixed_cell = 0;

        private bool isComputerMove = false;
        private string[,] TempArr = new string[11, 11];
        private const string MissedMark = "X";
        private const string ShipMark = "O";
        private const string EmptyCellMark = " ";



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
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(PathToShipContent.EmptyCell, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                Border = new Thickness(0.5)

            });

            _ships = new ObservableCollection<Ship>(ships);

            #region ShipsGenerating
            int OneDeckShip_DecksCount = 1;
            int TwoDeckShip_DeksCount = 2;
            int ThrieDeckShip_DeksCount = 3;
            int FourDeckShip_DeksCount = 4;

            int OneDeckShipCount = 4;
            int TwoDeckShipCount = 3;
            int thrieDeckShipCount = 2;
            int FourDeckShipCount = 1;

            ShipsGenerating(FourDeckShipCount, FourDeckShip_DeksCount);
            ShipsGenerating(thrieDeckShipCount, ThrieDeckShip_DeksCount);
            ShipsGenerating(OneDeckShipCount, OneDeckShip_DecksCount);
            ShipsGenerating(TwoDeckShipCount, TwoDeckShip_DeksCount);
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

            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

            //bool isGameover = UserTurn(fields, Indexes, Cell);
            //if (isGameover is true) return;

            //ComputerTurn(fields.UserField);

        }
        #endregion

        #region PrivateMethods
        private void ComputerTurn(string[,] userField)
        {
            Task.Run(() =>
            {
                CountOfAttaksInOneDirection = 0;
                while (isComputerMove != false)
                {
                    if (isHitButNotKilled is false)
                    {
                        _indexes = SearchRandomCell(userField);
                        Fixed_i = _indexes.I_index;
                        Fixed_j = _indexes.J_index;
                        Fixed_cell = CellsConverter.ConvertIndexesToCell(_indexes);
                    }

                    int Cell = CellsConverter.ConvertIndexesToCell(_indexes);

                    if (CanChangingTheAttckDirection(ref Cell, userField)) continue;

                    bool isMissed = true;// GameProcess.Damaging(userField, _indexes.I_index, _indexes.J_index);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (isMissed is true)
                        {
                            isComputerMove = false;
                            AssignTheAppropiateMark(Cell, vm.Ships, PathToShipContent.MissedMark, 0.5);

                            if (isHitButNotKilled is false) CountOfAttaksInOneDirection = 0;

                            RefreshData(ref Cell, ref CountOfAttaksInOneDirection);

                        }

                        if (isMissed is false)
                        {
                            isComputerMove = true;
                            int IndexOfTheFirstShipsDeck = 0;
                            int FirstIndex = _indexes.I_index;
                            int SecondIndex = _indexes.J_index;

                            int DecksCount = 4;
                            //    int DecksCount = GameProcess.CountingDecks
                            //(userField, _indexes.I_index, _indexes.J_index, ref IndexOfTheFirstShipsDeck, vm.Ships[Cell].isHorizontal);

                            _ = vm.Ships[Cell].isHorizontal is false ?
                           FirstIndex = _indexes.I_index - IndexOfTheFirstShipsDeck :
                           SecondIndex = _indexes.J_index - IndexOfTheFirstShipsDeck;

                            bool isKilled = false;// GameProcess.CheckedShipState
                       // (userField, FirstIndex, SecondIndex, DecksCount, vm.Ships[Cell].isHorizontal);

                            if (isKilled is false)
                            {
                                AssignTheAppropiateMark(Cell, vm.Ships, PathToShipContent.KilledShip, 0.5);
                                isComputerMove = true;
                                isHitButNotKilled = true;

                            }
                            if (isKilled is true)
                            {
                                CountOfAttaksInOneDirection = 0;
                                isHitButNotKilled = false;
                                //userField = GameProcess.ShipsFuneral(userField, FirstIndex, SecondIndex, DecksCount, vm.Ships[Cell].isHorizontal);
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

                                vm.Ships = ConsequencesOfAttack(userField, vm.Ships);

                                if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
                               vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
                                {
                                    MessageBox.Show("You lose!", "Try Again!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    isComputerMove = true;
                                    return;
                                }

                            }
                        }
                    }), DispatcherPriority.Normal);
                    Thread.Sleep(500);
                }
            });

        }
        //private bool UserTurn(Field fields, CellIndex Indexes, int Cell)
        //{
        //    if (fields.ComputerField[Indexes.I_index, Indexes.J_index] == ShipMark ||
        //       fields.ComputerField[Indexes.I_index, Indexes.J_index] == MissedMark) return false;

        //    isComputerMove = true;

        //    bool isMissed = GameProcess.Damaging(fields.ComputerField, Indexes.I_index, Indexes.J_index);

        //    if (isMissed is true)
        //    {
        //        MissCounter++;
        //        AssignTheAppropiateMark(Cell, Ships, PathToShipContent.MissedMark, 0.5);
        //    }
        //    if (isMissed is false)
        //    {
        //        isComputerMove = false;

        //        bool isShipKilled = GameProcess.ShipsIntegityChecked
        //        (fields.ComputerField, Indexes.I_index, Indexes.J_index, Ships[Cell].isHorizontal);

        //        if (isShipKilled is false)
        //        {
        //            AssignTheAppropiateMark(Cell, Ships, PathToShipContent.KilledShip, 0.5);
        //        }
        //        if (isShipKilled is true)
        //        {
        //            NumberOfRemainingComputerShips--;
        //            Ships = ConsequencesOfAttack(fields.ComputerField, Ships);
        //            if (NumberOfRemainingComputerShips is 0)
        //            {
        //                MessageBox.Show("You win!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
        //                isComputerMove = true;
        //                return true;
        //            }

        //        }
        //    }
        //    return false;
        //}
        private bool CanChangingTheAttckDirection(ref int Cell, string[,] userField)
        {
            if (isHitButNotKilled is true)
            {
                CountOfAttaksInOneDirection++;
                if (isHorizontal is true)
                {
                    if (isRightDirection is true)
                    {
                        _indexes.J_index += 1;
                        Cell += 1;

                        if (IndexsesValidation(userField, ref Cell))
                            return true;

                    }
                    if (isRightDirection is false)
                    {
                        _indexes.J_index -= 1;
                        Cell -= 1;


                        if (IndexsesValidation(userField, ref Cell))
                            return true;
                    }
                }
                if (isHorizontal is false)
                {
                    if (isUpwardDirection is true)
                    {
                        _indexes.I_index -= 1;
                        Cell -= 11;


                        if (IndexsesValidation(userField, ref Cell))
                            return true;
                    }
                    if (isUpwardDirection is false)
                    {
                        _indexes.I_index += 1;
                        Cell += 11;


                        if (IndexsesValidation(userField, ref Cell))
                            return true;
                    }
                }
            }
            return false;
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
        private void AssignTheAppropiateMark(int cell, ObservableCollection<Ship> ships, string Mark, double BorderSize)
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
                    //if (Ships[i * 11 + j].isOnField)
                    //    field.ComputerField[i, j] = EmptyCellMark;
                    //if (vm.Ships[i * 11 + j].isOnField)
                    //    field.UserField[i, j] = EmptyCellMark;

                }
            }
            return field;
        }
        private void RefreshData(ref int Cell, ref int CountOfAttaksInOneDirection)
        {
            _indexes.I_index = Fixed_i;
            _indexes.J_index = Fixed_j;
            Cell = Fixed_cell;
            if (isHitButNotKilled is true && CountOfAttaksInOneDirection <= 1)
            {
                isHorizontal = !isHorizontal;
                return;
            }
            if (isHitButNotKilled is true)
            {
                if (isHorizontal is true)
                {
                    isRightDirection = !isRightDirection;
                }
                if (isHorizontal is false)
                {
                    isUpwardDirection = !isUpwardDirection;
                }
                CountOfAttaksInOneDirection = 0;
            }
        }
        private bool IndexsesValidation(string[,] userField, ref int Cell)
        {
            try
            {
                if (userField[_indexes.I_index, _indexes.J_index] is MissedMark || _indexes.I_index is 0 || _indexes.J_index is 0)
                {
                    RefreshData(ref Cell, ref CountOfAttaksInOneDirection);
                    return true;
                }
            }
            catch (Exception)
            {

                RefreshData(ref Cell, ref CountOfAttaksInOneDirection);
                return true;
            }
            return false;
        }
        private void ShipsGenerating(int ShipCount, int DeksCount)
        {
            var Random = new Random();
            for (int i = 1; i <= ShipCount; i++)
            {
                int Cell = Random.Next(11, 121);
                CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

                int direction = Random.Next(1, 3);

                bool isHorizontal = direction is 1 ? true : false;

                //bool canPutShip = ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, TempArr, DeksCount, isHorizontal);

                //if (canPutShip is false)
                //{
                //    i--;
                //    continue;
                //}
                //if (canPutShip is true)
                //{
                //    if (isHorizontal is true)
                //        ShipsCreatingForHorizontalAxis(DeksCount, Cell, Indexes);

                //    if (isHorizontal is false)
                //        ShipsCreatingForVerticalAxis(DeksCount, Cell, Indexes);
                //}
            }
        }
        private void ShipsCreatingForVerticalAxis(int DeksCount, int Cell, CellIndex Indexes)
        {
            switch (DeksCount)
            {
                default:
                    break;

                case 1:
                    ShipsOptions(Cell, Indexes.I_index, Indexes.J_index, false);
                    break;

                case 2:
                    ShipsOptions(Cell, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(Cell + 11, Indexes.I_index + 1, Indexes.J_index, false);
                    break;

                case 3:
                    ShipsOptions(Cell, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(Cell + 11, Indexes.I_index + 1, Indexes.J_index, false);
                    ShipsOptions(Cell + 22, Indexes.I_index + 2, Indexes.J_index, false);
                    break;

                case 4:
                    ShipsOptions(Cell, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(Cell + 11, Indexes.I_index + 1, Indexes.J_index, false);
                    ShipsOptions(Cell + 22, Indexes.I_index + 2, Indexes.J_index, false);
                    ShipsOptions(Cell + 33, Indexes.I_index + 3, Indexes.J_index, false);
                    break;
            }
        }
        private void ShipsCreatingForHorizontalAxis(int DeksCount, int Cell, CellIndex Indexes)
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
        private void ShipsOptions(int Cell, int i, int j, bool direction = true)
        {
            Ships[Cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(PathToShipContent.EmptyCell, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isOnField = true,
                Border = new Thickness(0.5),
                isHorizontal = direction
            };
            TempArr[i, j] = ShipMark;
        }
        #endregion 
    }
}

