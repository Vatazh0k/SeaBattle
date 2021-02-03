﻿using SeaBattle.BuisnessLogic;
using SeaBattle.Model;
using SeaBattle.Resource;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SeaBattle.ViewModel
{
    class ComputerFieldPageViewModel : ViewModelBase
    {
        #region PrivateData
        private MainWindowViewModel vm;
        private ObservableCollection<Ship> _ships;
        bool isComputerMove = false;
        private const string MissedMark = "X";
        private const string KilledMark = "W";

        private int OneDeckShipDecksCount = 1;
        private int TwoDeckShipDeksCount = 2;
        private int ThrieDeckShipDeksCount = 3;
        private int FourDeckShipDeksCount = 4;

        private int OneDeckShipCount = 4;
        private int TwoDeckShipCount = 3;
        private int thrieDeckShipCount = 2;
        private int FourDeckShipCount = 1;

        private int NumberOfRemainingUserShips = 10;
        private int NumberOfRemainingComputerShips = 10;
        #endregion

        #region PublicData
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
                Content = null,
                Color = new SolidColorBrush(Colors.White),
                Border = new Thickness(0.5)
            });

            _ships = new ObservableCollection<Ship>(ships);

            #region ShipsGenerating
            ShipsGeneration(FourDeckShipCount, FourDeckShipDeksCount);
            ShipsGeneration(thrieDeckShipCount, ThrieDeckShipDeksCount);
            ShipsGeneration(OneDeckShipCount, OneDeckShipDecksCount);
            ShipsGeneration(TwoDeckShipCount, TwoDeckShipDeksCount);
            #endregion
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
            Field fields = CellsAssignment();

            if (fields.ComputerField[Indexes.I_index, Indexes.J_index] == KilledMark ||
                fields.ComputerField[Indexes.I_index, Indexes.J_index] == MissedMark) return;

            //isComputerMove = true;

            bool isMissed = GameProcess.DamageCreating(fields.ComputerField, Indexes.I_index, Indexes.J_index);

            if (isMissed is true)
            {
                MissedAction(Cell, Ships, Colors.Red, MissedMark);
            }
            if (isMissed is false)
            {
                bool isShipKilled = GameProcess.ChekedTheShipState(fields.ComputerField, Indexes.I_index, Indexes.J_index);

                if (isShipKilled is false)
                {
                    MissedAction(Cell, Ships, Colors.Black, KilledMark);
                }
                if (isShipKilled is true)
                {
                    NumberOfRemainingComputerShips--;
                    Ships = ConsequencesOfAttack(fields.ComputerField, Ships);
                    if (NumberOfRemainingComputerShips is 0) 
                    {
                        MessageBox.Show("You win!", "Congratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                        isComputerMove = true;
                        return;
                    }
     
                }
            }

            //сделать ход компьютера


        }
        #endregion

        #region PrivateMethods
        private ObservableCollection<Ship> ConsequencesOfAttack(string[,] field, ObservableCollection<Ship> Ships)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (field[i, j] == MissedMark && Ships[i * 11 + j].Content != MissedMark)
                    {
                        Ships[i * 11 + j] = new Ship
                        {
                            Content = field[i, j],
                            Color = new SolidColorBrush(Colors.Red),
                            Border = new Thickness(0.5)

                        };
                    }
                    else if (field[i, j] == KilledMark)
                    {
                        Ships[i * 11 + j] = new Ship
                        {
                            Content = field[i, j],
                            Color = new SolidColorBrush(Colors.Black),
                            Border = new Thickness(1)
                        };
                    }
                }
            }
            return Ships;
        }
        private void MissedAction(int cell, ObservableCollection<Ship> ships, Color color, string Mark)
        {
            ships[cell] = new Ship
            {
                Content = Mark,
                Color = new SolidColorBrush(color),
                Border = new Thickness(0.5)
            };

        }
        private Field CellsAssignment()
        {
            Field field = new Field();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    field.ComputerField[i, j] = Ships[i * 11 + j].Content;
                    field.UserField[i, j] = vm.Ships[i * 11 + j].Content;
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
            string[,] tempArr = new string[11, 11];
            tempArr = CellsAssignment(tempArr, Ships);

            var Random = new Random();

            for (int i = 0; i < ShipCount; i++)
            {
                int Cell = Random.Next(11, 121);
                CellIndex Indexes = SearchCellIndexes(Cell);

                if (!ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, tempArr, DeksCount))
                    i--;
                else
                {
                    switch (DeksCount)
                    {
                        default:
                            break;

                        case 1:
                            ShipsOptions(Cell);
                            break;

                        case 2:
                            ShipsOptions(Cell);
                            ShipsOptions(Cell + 1);
                            break;

                        case 3:
                            ShipsOptions(Cell);
                            ShipsOptions(Cell + 1);
                            ShipsOptions(Cell + 2);
                            break;

                        case 4:
                            ShipsOptions(Cell);
                            ShipsOptions(Cell + 1);
                            ShipsOptions(Cell + 2);
                            ShipsOptions(Cell + 3);
                            break;
                    }

                    tempArr[Indexes.I_index, Indexes.J_index] = KilledMark;
                }
            }
        }
        private void ShipsOptions(int Cell)
        {
            Ships[Cell] = new Ship
            {
                Content = " ",
                Border = new Thickness(0.5),
                Color = new SolidColorBrush(Colors.White)

            };
        }
        private string[,] CellsAssignment(string[,] tempArr, ObservableCollection<Ship> Ships)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    tempArr[i, j] = Ships[i * 11 + j].Content;
                }
            }
            return tempArr;
        }
        #endregion
    }
}
  