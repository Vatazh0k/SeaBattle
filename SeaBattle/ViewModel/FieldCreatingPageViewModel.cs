using SeaBattle.BuisnessLogic;
using SeaBattle.Model;
using SeaBattle.Resource;
using SeaBattle.View.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaBattle.ViewModel
{
    class FieldCreatingPageViewModel
    {
        #region Data
        private MainWindowViewModel vm;
        private Page ComputerFieldPage;
        private readonly string ShipMark = "O";
        private readonly string EmptyCellMark = " ";

        public ICommand ShipsAutoGeneration { get; set; }
        public ICommand ReadyCommand { get; set; }
        public ICommand CleanField { get; set; }
        #endregion

        public FieldCreatingPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            ShipsAutoGeneration = new Command(ShipsAutoGenerationAction, CanUseCommands);
            ReadyCommand = new Command(ReadyCommandAction, CanUseCommands);
            CleanField = new Command(CelanTheFieldAction, CanUseCommands);

        }

        #region Commands
        private bool CanUseCommands(object p) => true;
        private void CelanTheFieldAction(object p)
        {
            vm.FourDeckShip = 1;
            vm.ThrieDeckShip = 2;
            vm.TwoDeckShip = 3;
            vm.OneDeckShip = 4;
            for (int i = 0; i < 121; i++)
            {
                vm.Ships[i] = new Ship
                {
                    Content = new Image(),
                    Border = new Thickness(0.5),
                    isDead = false,
                    isOnField = false
                };
            }
        }
        private void ShipsAutoGenerationAction(object p)
        {
            ShipsGeneration(vm.FourDeckShip, 4);
            ShipsGeneration(vm.ThrieDeckShip, 3);
            ShipsGeneration(vm.OneDeckShip, 1);
            ShipsGeneration(vm.TwoDeckShip, 2);
        }
        private void ReadyCommandAction(object p)
        {
            if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
              vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
            {
                vm.FourDeckShip = 1;
                vm.ThrieDeckShip = 2;
                vm.TwoDeckShip = 3;
                vm.OneDeckShip = 4;
                ComputerFieldPage = new ComputerFieldPage(vm);
                vm.CurrentPage = ComputerFieldPage;//треба створити класс який буде відовідати за навігацією між сторінками
            }
            else
                MessageBox.Show("Please input all ships", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region PrivateMethods
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
        private void ShipsOptions(string Path, int Cell, double left, double top, double right, double botom)
        {
            vm.Ships[Cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isOnField = true,
                Border = new Thickness(left, top, right, botom)
            };
        }
        private string[,] CellsAssignment(string[,] tempArr, ObservableCollection<Ship> Ships)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (Ships[i * 11 + j].isDead == true)
                        tempArr[i, j] = ShipMark;
                    if (Ships[i * 11 + j].isOnField == true && Ships[i * 11 + j].isDead == false)
                        tempArr[i, j] = EmptyCellMark;
                }
            }
            return tempArr;
        }
        private void ShipsGeneration(int ShipCount, int DeksCount)
        {
            string[,] tempArr = new string[11, 11];
            tempArr = CellsAssignment(tempArr, vm.Ships);

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
                            ShipsOptions(PathToShipContent.OneDeckShip, Cell, 1, 1, 1, 1);
                            vm.OneDeckShip--;
                            break;

                        case 2:
                            ShipsOptions(PathToShipContent.TwoDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                            ShipsOptions(PathToShipContent.TwoDeckShip_SecondDeck, Cell + 1, 0, 1, 1, 1);
                            vm.TwoDeckShip--;
                            break;


                        case 3:
                            ShipsOptions(PathToShipContent.ThrieDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                            ShipsOptions(PathToShipContent.ThrieDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1);
                            ShipsOptions(PathToShipContent.ThrieDeckShip_ThirdDeck, Cell + 2, 0, 1, 1, 1);
                            vm.ThrieDeckShip--;
                            break;
                        case 4:
                            ShipsOptions(PathToShipContent.FourDeckShip_FirstDeck, Cell, 1, 1, 0, 1);
                            ShipsOptions(PathToShipContent.FourDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1);
                            ShipsOptions(PathToShipContent.FourDeckShip_ThirdDeck, Cell + 2, 0, 1, 0, 1);
                            ShipsOptions(PathToShipContent.FourDeckShip_FourDeck, Cell + 3, 0, 1, 1, 1);
                            vm.FourDeckShip--;
                            break;
                    }

                    tempArr[Indexes.I_index, Indexes.J_index] = ShipMark;
                }
            }
        }
        #endregion
    }
}
