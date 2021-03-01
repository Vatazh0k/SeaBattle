using SeaBattle.BuisnessLogic;
using SeaBattle.Infrastructure.Converters;
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

        private string[,] userField = new string[11, 11];
        private const string ShipMark = "O";

        #region Command
        public ICommand ShipsAutoGeneration { get; set; }
        public ICommand ReadyCommand { get; set; }
        public ICommand CleanField { get; set; }
        #endregion
        #endregion

        public FieldCreatingPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            #region Commands
            ShipsAutoGeneration = new Command(ShipsAutoGenerationAction, CanUseCommands);
            ReadyCommand = new Command(ReadyCommandAction, CanUseCommands);
            CleanField = new Command(CelanTheFieldAction, CanUseCommands);
            #endregion
        }

        #region Commands
        private bool CanUseCommands(object p) => true;
        private void CelanTheFieldAction(object p)
        {
            ShipsReplenishment();
            userField = new string[11, 11];
            vm.Color = new ObservableCollection<Brush>(vm.colors);
            vm.Ships = new ObservableCollection<Ship>(vm.ships);
        }
        private void ShipsAutoGenerationAction(object p)
        {
            vm.Color = new ObservableCollection<Brush>(vm.colors);
            userField = CellAssigning();
            ShipsGeneration(vm.FourDeckShip, 4);
            ShipsGeneration(vm.ThrieDeckShip, 3);
            ShipsGeneration(vm.OneDeckShip, 1);
            ShipsGeneration(vm.TwoDeckShip, 2);
        }
        private void ReadyCommandAction(object p)
        {
            vm.Color = new ObservableCollection<Brush>(vm.colors);
            if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
              vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
            {
                ShipsReplenishment();
                ComputerFieldPage = new ComputerFieldPage(vm);
                vm.CurrentPage = ComputerFieldPage;//треба створити класс який буде відовідати за навігацією між сторінками
            }
            else
                MessageBox.Show("Please input all ships", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region PrivateMethods
        private string[,] CellAssigning()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if(vm.Ships[i * 11 + j].isOnField is true)
                    userField[i, j] = ShipMark;
                }
            }
            return userField;
        } 
        private void ShipsReplenishment()
        {
            vm.FourDeckShip = 1;
            vm.ThrieDeckShip = 2;
            vm.TwoDeckShip = 3;
            vm.OneDeckShip = 4;
        }
        private void ShipsOptions(string Path, int Cell, double left, double top, double right, double botom, int i, int j, bool Direction = true)
        {
            vm.Ships[Cell] = new Ship
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(Path, UriKind.Relative)),
                    Stretch = Stretch.Fill
                },
                isOnField = true,
                Border = new Thickness(left, top, right, botom),
                isHorizontal = Direction
            };
            userField[i, j] = ShipMark;
        }
        private void ShipsGeneration(int ShipCount, int DeksCount)
        {
            var Random = new Random();

            for (int i = 1; i <= ShipCount; i++)
            {
                int Cell = Random.Next(11, 121);
                CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);

                int direction = Random.Next(1, 3);

                bool isHorizontal = direction is 1 ? true : false;

                //bool canPutShip = ShipPositionValidation.PositionValidationLogic(Indexes.I_index, Indexes.J_index, userField, DeksCount,isHorizontal);

                //if (canPutShip is false)
                //{
                //    i--;
                //    continue;
                //}
                //if(canPutShip is true)
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
                    ShipsOptions(PathToShipContent.Vertical_OneDeckShip, Cell, 1, 1, 1, 1, Indexes.I_index, Indexes.J_index, false);
                    vm.OneDeckShip--;
                    break;

                case 2:
                    ShipsOptions(PathToShipContent.Vertical_TwoDeckShip_FirstDeck, Cell, 1, 1, 1, 0, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_TwoDeckShip_SecondDeck, Cell + 11, 1, 0, 1, 1, Indexes.I_index+1, Indexes.J_index, false);
                    vm.TwoDeckShip--;
                    break;


                case 3:
                    ShipsOptions(PathToShipContent.Vertical_ThrieDeckShip_FirstDeck, Cell, 1, 1, 1, 0, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_ThrieDeckShip_SecondDeck, Cell + 11, 1, 0, 1, 0, Indexes.I_index+1, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_ThrieDeckShip_ThirdDeck, Cell + 22, 1, 0, 1, 1, Indexes.I_index+2, Indexes.J_index, false);
                    vm.ThrieDeckShip--;
                    break;
                case 4:
                    ShipsOptions(PathToShipContent.Vertical_FourDeckShip_FirstDeck, Cell, 1, 1, 1, 0, Indexes.I_index, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_FourDeckShip_SecondDeck, Cell + 11, 1, 0, 1, 0, Indexes.I_index+1, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_FourDeckShip_ThirdDeck, Cell + 22, 1, 0, 1, 0, Indexes.I_index+2, Indexes.J_index, false);
                    ShipsOptions(PathToShipContent.Vertical_FourDeckShip_FourDeck, Cell + 33, 1, 0, 1, 1, Indexes.I_index+3, Indexes.J_index, false);
                    vm.FourDeckShip--;
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
                    ShipsOptions(PathToShipContent.OneDeckShip, Cell, 1, 1, 1, 1, Indexes.I_index, Indexes.J_index);
                    vm.OneDeckShip--;
                    break;

                case 2:
                    ShipsOptions(PathToShipContent.TwoDeckShip_FirstDeck, Cell, 1, 1, 0, 1, Indexes.I_index, Indexes.J_index);
                    ShipsOptions(PathToShipContent.TwoDeckShip_SecondDeck, Cell + 1, 0, 1, 1, 1, Indexes.I_index, Indexes.J_index + 1);
                    vm.TwoDeckShip--;
                    break;


                case 3:
                    ShipsOptions(PathToShipContent.ThrieDeckShip_FirstDeck, Cell, 1, 1, 0, 1, Indexes.I_index, Indexes.J_index);
                    ShipsOptions(PathToShipContent.ThrieDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1, Indexes.I_index, Indexes.J_index + 1);
                    ShipsOptions(PathToShipContent.ThrieDeckShip_ThirdDeck, Cell + 2, 0, 1, 1, 1, Indexes.I_index, Indexes.J_index + 2);
                    vm.ThrieDeckShip--;
                    break;
                case 4:
                    ShipsOptions(PathToShipContent.FourDeckShip_FirstDeck, Cell, 1, 1, 0, 1, Indexes.I_index, Indexes.J_index);
                    ShipsOptions(PathToShipContent.FourDeckShip_SecondDeck, Cell + 1, 0, 1, 0, 1, Indexes.I_index, Indexes.J_index + 1);
                    ShipsOptions(PathToShipContent.FourDeckShip_ThirdDeck, Cell + 2, 0, 1, 0, 1, Indexes.I_index, Indexes.J_index + 2);
                    ShipsOptions(PathToShipContent.FourDeckShip_FourDeck, Cell + 3, 0, 1, 1, 1, Indexes.I_index, Indexes.J_index + 3);
                    vm.FourDeckShip--;
                    break;
            }
        }
        #endregion
    }
}
  