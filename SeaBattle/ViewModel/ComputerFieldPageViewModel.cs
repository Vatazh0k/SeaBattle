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

            ComputerField.field = ComputerField.FieldAutoGeneration(ComputerField.field);
        }

        #region Commands
        private bool CanUseMakeDamageCommand(object p) => !isComputerMove;
        private void MakeDamageCommandAction(object p)
        {
            int Cell = SearchCell(p.ToString());

            if (UserField.CanMakeDamage(ComputerField.field, Cell) is false)
                return;

            CellIndex indexes = CellsConverter.ConverCellsToIndexes(Cell);
            ComputerField.field = UserField.Damaging(ComputerField.field, indexes);
            Ships = CellAssignment(Ships, ComputerField.field, Cell);


        }
        #endregion


        #region PrivateMethods
        private ObservableCollection<Ship> CellAssignment(ObservableCollection<Ship> Ships, string[,] field, int cell)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Ships[GetCell(i, j)].isOnField is false && field[i, j] is MissedMark)
                    {
                        ShipsOptions(Ships, GetCell(i,j), PathToShipContent.MissedMark, 0.5);
                        //isComputerMove = true;
                        continue;
                    }
                    if (Ships[GetCell(i, j)].isOnField is false && field[i, j] is KilledMark)
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
                            _ = Direction is true ? J += k : I += k;
                            if (I > 10 || J > 10) continue;
                            if (field[I, J] is KilledMark)
                            {
                                isKilled = true;
                                continue;
                            }
                            isKilled = false;
                        }
                        if(isKilled is false) ShipsOptions(Ships, GetCell(i, j), PathToShipContent.KilledShip, 1);
                        if (isKilled is true && Ships[GetCell(i,j)].isOnField is false)
                        {
                            _ = Direction is true ? 
                                ShowHorizontalShips(DecksCount, GetCell(i, j)):
                                ShowDeadVerticalShips(DecksCount, GetCell(i, j));
                        }
                    }
                }
            }

            return Ships;
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
                isOnField = true,
                Border = new Thickness(Thickness),
            };
        }
        private ObservableCollection<Ship> ShowDeadVerticalShips(int DecksCount, int cell)
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
        private ObservableCollection<Ship> ShowHorizontalShips(int DecksCount, int cell)
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
        #endregion
        #endregion
    }
}

