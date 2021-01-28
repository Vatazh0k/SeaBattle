using SeaBattle.Resource;
using SeaBattle.View.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModel
{
    class FieldCreatingPageViewModel 
    {
        #region Dat
        private MainWindowViewModel vm;
        private Page GameProcessPage;

        public ICommand ReadyCommand { get; set; }
        #endregion

        #region Ctor
        public FieldCreatingPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            ReadyCommand = new Command(ReadyCommandAction, CanUseReadyCommand);

        }
        #endregion

        #region Commands
        private bool CanUseReadyCommand(object p) => true;
          
        private void ReadyCommandAction(object p)
        {
            if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
              vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
            {
                GameProcessPage = new GameProcessPage(vm);
                vm.CurrentPage = GameProcessPage;//треба створити класс який буде відовідати за навігацією між сторінками
            }
            else 
                MessageBox.Show("Please input all ships", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion
    }
}
