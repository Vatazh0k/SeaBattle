using SeaBattle.Resource;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModel
{
    class LoginPageViewModel : ViewModelBase
    {
        #region Data

        private string _login;
        private string _password;
        private MainWindowViewModel mainWindowViewModel;
        private RegisterWindow registerWindow;

        public string Login
        {
            get { return _login; }
            set => Set(ref _login, value);
        }
        public string Password
        {
            get { return _password; }
            set => Set(ref _password, value);
        }

        public ICommand LoginCommand { get; set; }
        public ICommand RegCommand { get; set; }
        #endregion

        public LoginPageViewModel()
        { }

        public LoginPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            RegCommand = new Command(RegCommandAction, CanUseRegCommand);
        }

        private bool CanUseRegCommand(object p) => true;


        private void RegCommandAction(object p)
        {
            registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
