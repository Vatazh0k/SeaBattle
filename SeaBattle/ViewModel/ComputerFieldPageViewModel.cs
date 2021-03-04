﻿using SeaBattle.BuisnessLogic;
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
        private Field UserField;
        private Field ComputerField;
        private CellIndex _indexes;


        private bool isInProcess = true;
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

           // ComputerField.field = ComputerField.FieldAutoGeneration(ComputerField.field);
        }

        #region Commands
        private bool CanUseMakeDamageCommand(object p) => !isComputerMove && isInProcess;
        private void MakeDamageCommandAction(object p)
        {
            int Cell = SearchCell(p.ToString());

            if (UserField.CanMakeDamage(ComputerField.field, Cell) is false)
                return;

            isComputerMove = true;
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(Cell);
            ComputerField.field = UserField.UserAttck(ComputerField.field, indexes);
            Ships = ComputersShipsAssignment(ComputerField.field, Cell);

            if (isComputerMove is false)
                return;

            UserField.field = ComputerField.ComputerAttack(UserField.field);
            UsersShipsAssignment(UserField.field);
            isComputerMove = false;

        }
        #endregion


        #region PrivateMethods
        private void UsersShipsAssignment(string[,] field)
        {

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {

                    if (vm.Ships[GetCell(i, j)].isDead is false && field[i, j] is MissedMark)
                    {
                        ShipsOptions(vm.Ships, GetCell(i, j), PathToShipContent.MissedMark, 0.5);
                    }
                    if (vm.Ships[GetCell(i, j)].isDead is false && field[i, j] is KilledMark)
                    {
                        bool isKilled = false;
                        isComputerMove = false;
                        int FirstDecksIndex = 0;
                        bool Direction = ComputerField.DeterminingTheDirection(i, j, field);
                        CellIndex indexes = new CellIndex(); indexes.I_index = i; indexes.J_index = j;
                        int DecksCount = ComputerField.CountingDecks(field, indexes, ref FirstDecksIndex, Direction);

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
                        ShipsOptions(vm.Ships, GetCell(i, j), PathToShipContent.KilledShip, 1);

                        if (isKilled is true)
                        {
                            switch (DecksCount)
                            {
                                default: break;
                                case 1: vm.OneDeckShip--; break;
                                case 2: vm.TwoDeckShip--; break;
                                case 3: vm.ThrieDeckShip--; break;
                                case 4: vm.FourDeckShip--; break;
                            }
                        }
                    }

                }

            }


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
        private int GetCell(int i, int j)
        {
            return i * 11 + j;
        }
        #region Show Ships
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
                    ShipsOptions(Ships,cell, PathToShipContent.Vertical_Dead_ThrieDeckShip_FirstDeck, 1, false);
                    ShipsOptions(Ships,cell + 11, PathToShipContent.Vertical_Dead_ThrieDeckShip_SecondDeck, 1, false);
                    ShipsOptions(Ships, cell + 22, PathToShipContent.Vertical_Dead_ThrieDeckShip_ThirdDeck, 1, false);
                    break;
                case 4:
                    ShipsOptions(Ships,cell, PathToShipContent.Vertical_Dead_FourDeckShip_FirstDeck, 1, false);
                    ShipsOptions(Ships,cell + 11, PathToShipContent.Vertical_Dead_FourDeckShip_SecondDeck, 1, false);
                    ShipsOptions(Ships,cell + 22, PathToShipContent.Vertical_Dead_FourDeckShip_ThirdDeck, 1, false);
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
                        bool isKilled = false;
                        isComputerMove = false;
                        int FirstDecksIndex = 0;
                        bool Direction = ComputerField.DeterminingTheDirection(indexes.I_index, indexes.J_index, field);
                        int DecksCount = ComputerField.CountingDecks(field, indexes, ref FirstDecksIndex, Direction);

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
                        if (isKilled is false) ShipsOptions(Ships, GetCell(i, j), PathToShipContent.KilledShip, 1);
                        if (isKilled is true && Ships[GetCell(i, j)].isDead is false)
                        {
                            int Cell = GetCell(i, j);
                            _ = Direction is true ?
                                ShowHorizontalShips(Ships, DecksCount, Cell - FirstDecksIndex) :
                                ShowDeadVerticalShips(Ships, DecksCount, Cell - FirstDecksIndex * 11);
                            NumberOfRemainingComputerShips--;
                            if (NumberOfRemainingComputerShips is 0)
                            {
                                MessageBox.Show("You win!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                                isInProcess = false;
                            }
                        }
                    }
                }
            }

            return Ships;
        }
        #endregion
        #endregion
    }
}

