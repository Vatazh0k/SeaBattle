using SeaBattle.Resource;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModel
{
    class RegisterWindowViewModel : ViewModelBase
    {
        #region Data
        private RegisterWindow registerWindow;

        public ICommand BackCommand { get; set; }
        #endregion

        public RegisterWindowViewModel()
        { }

        public RegisterWindowViewModel(RegisterWindow registerWindow)
        {
            this.registerWindow = registerWindow;

            BackCommand = new Command(BackCommandAction, CanUseBackCommand);
        }

        private bool CanUseBackCommand(object p) => true;


        private void BackCommandAction(object p)
        {
            registerWindow.Close();
        }
    }
}
