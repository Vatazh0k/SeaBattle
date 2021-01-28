using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModel
{
    class FieldCreatingPageViewModel : ViewModelBase
    {
        private MainWindowViewModel vm;

        public ICommand ReadyCommand { get; set; }
        public FieldCreatingPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            ReadyCommand = new Command(ReadyCommandAction, CanUseReadyCommand);

        }

        private bool CanUseReadyCommand(object p) => true;
          
        private void ReadyCommandAction(object p)
        {
            if (vm.OneDeckShip is 0 && vm.TwoDeckShip is 0 &&
              vm.ThrieDeckShip is 0 && vm.FourDeckShip is 0)
            {
                Page GameProcessPage = new GameProcessPage();
                vm.CurrentPage = 
                
               
                
            }
            else 
                MessageBox.Show("Please input all ships", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
