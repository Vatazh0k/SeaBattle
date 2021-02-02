using SeaBattle.BuisnessLogic;
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
        private const string KilledMark = "O";

        private int OneDeckShipDecksCount = 1;
        private int TwoDeckShipDeksCount = 2;
        private int ThrieDeckShipDeksCount = 3;
        private int FourDeckShipDeksCount = 4;

        private int OneDeckShipCount = 4;
        private int TwoDeckShipCount = 3;
        private int thrieDeckShipCount = 2;
        private int FourDeckShipCount = 1;

        private int NumberOfRemainingUserShips = 10;
        private int NumberOfRemainingComputerShips = 20;
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
            isComputerMove = true;

            int Cell = 0;
            string cellString = string.Empty;

            for (int i = 1; i < p.ToString().Length; i++)
            {
                cellString += p.ToString()[i];
                Cell = Convert.ToInt32(cellString);
            }

            CellIndex Indexes = SearchCellIndexes(Cell);

            Field fields = CellsAssignment();

            if (fields.ComputerField[Indexes.I_index, Indexes.J_index] == MissedMark ||
                fields.ComputerField[Indexes.I_index, Indexes.J_index] == KilledMark)
            {
                isComputerMove = false;
                return; 
            }

            bool isMissed = GameProcess.DamageCreating(fields.ComputerField, Indexes.I_index, Indexes.J_index);

            if (isMissed is true)
            {
                Ships[Cell] = new Ship
                {
                    Content = MissedMark,
                    Color = new SolidColorBrush(Colors.Red),
                    Border = new Thickness(0.5)
                };
            }

            else 
            {
                ConsequencesOfAttack(fields.ComputerField, Ships);
                isComputerMove = false;

                NumberOfRemainingComputerShips--;
                if (NumberOfRemainingComputerShips is 0)
                    MessageBox.Show("Congratulation", "You win", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //async//заморозить окно//алгортим удара переделать

            while (isComputerMove != false)
            {
                if (isComputerMove == true)
                {
                    isComputerMove = false;

                    CellIndex CellsIndexes = SearchRandomCells(fields.UserField);

                    isMissed = GameProcess.DamageCreating(fields.UserField, CellsIndexes.I_index, CellsIndexes.J_index);

                    Cell = SearchCellIndex(CellsIndexes.I_index, CellsIndexes.J_index);

                    if (isMissed is true)
                    {
                        vm.Ships[Cell] = new Ship
                        {
                            Content = MissedMark,
                            Color = new SolidColorBrush(Colors.Red),
                            Border = new Thickness(0.5)
                        };
                    }

                    else
                    {
                        isComputerMove = true;
                        ConsequencesOfAttack(fields.UserField, vm.Ships);

                        NumberOfRemainingUserShips--;
                        if(NumberOfRemainingUserShips is 10)
                            MessageBox.Show("Try again", "You lose", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
        #endregion

        #region PrivateMethods
        private int SearchCellIndex(int I, int J)
        {
            int Cell = 0;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i == I && j == J)
                        Cell = i * 11 + j;
                }
            }
            return Cell;
        }
        private CellIndex SearchRandomCells(string [,] UserField)
        {
            CellIndex cellIndex = new CellIndex();
            var random = new Random();
            int ItterationCount = 0;
            while (true)
            {
                ItterationCount++;
                cellIndex.I_index = random.Next(1, 9);
                cellIndex.J_index = random.Next(1, 9);

                if (ItterationCount == 11)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            if (UserField[i, j] != MissedMark)
                            {
                                cellIndex.I_index = i;
                                cellIndex.J_index = j;
                            }
                        }
                    }
                }
                if (UserField[cellIndex.I_index, cellIndex.J_index] == MissedMark)
                    continue;
                break;
            }
            return cellIndex;
        }
        private void ConsequencesOfAttack(string[,] Field, ObservableCollection<Ship> Ships)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Field[i, j] == MissedMark && Ships[i * 11 + j].Content != MissedMark)
                    {
                        Ships[i * 11 + j] = new Ship
                        {
                            Content = Field[i, j],
                            Color = new SolidColorBrush(Colors.Red),
                            Border = new Thickness(0.5)

                        };
                    }
                    else if (Field[i, j] == KilledMark && Ships[i * 11 + j].Content != KilledMark)
                    {
                        Ships[i * 11 + j] = new Ship
                        {
                            Content = Field[i, j],
                            Color = new SolidColorBrush(Colors.DarkRed),
                            Border = new Thickness(1)
                        };
                    }
                }
            }
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
  