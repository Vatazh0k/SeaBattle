using SeaBattle.BuisnessLogic;
using SeaBattle.Model;
using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private int OneDeckShipDecksCount = 1;
        private int TwoDeckShipDeksCount = 2;
        private int ThrieDeckShipDeksCount = 3;
        private int FourDeckShipDeksCount = 4;

        private int OneDeckShipCount = 4;
        private int TwoDeckShipCount = 3;
        private int thrieDeckShipCount = 2;
        private int FourDeckShipCount = 1;
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
            #region Data
            int Cell;
            string cellString = string.Empty;
            for (int i = 1; i < p.ToString().Length; i++)
                cellString += p.ToString()[i];
            Cell = Convert.ToInt32(cellString);
            #endregion

            if (Ships[Cell].Content == " ")// в геймп процес, передавать " " || "O" 
            {
                Ships[Cell] = new Ship
                {
                    Color = new SolidColorBrush(Colors.Red),
                    Content = "O"
                };
            }// isComputerMove = true;
        }
        #endregion

        #region PrivateMethods
        private void ShipsGeneration(int ShipCount, int DeksCount)
        {
            #region Data
            string[,] tempArr = new string[11, 11];
            tempArr = CellsAssignment(tempArr, Ships);
            var Random = new Random();
            int Cell;
            #endregion

            for (int i = 0; i < ShipCount; i++)
            {
                Cell = Random.Next(11, 121);
                if (!ShipPositionValidation.PositionValidationLogic(Cell, tempArr, DeksCount))
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

                    int FirstIndex = SerarchCellIndexs(Cell).Item1;
                    int SecondIndex = SerarchCellIndexs(Cell).Item2;
                    tempArr[FirstIndex, SecondIndex] = "O";
                }
            }
        }
        private (int, int) SerarchCellIndexs(int Cell)
        {
            int fixI = 0, fixJ = 0;
            for (int k = 0; k < 11; k++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (k * 11 + j == Cell)
                    {
                        fixI = k;
                        fixJ = j;
                    }

                }
            }
            return (fixI, fixJ);
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
  