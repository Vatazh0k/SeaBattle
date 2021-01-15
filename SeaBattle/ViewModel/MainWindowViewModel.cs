using SeaBattle.Resource;
using SeaBattle.View.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace SeaBattle.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {

        #region Data

        #region Private Data
        private RulesWindow RulesWindow = new RulesWindow();

        #endregion

        #region PUblic Data

        public ICommand ExitCommand { get; set; }
        public ICommand RulesCommand { get; set; }

        #endregion
        #endregion

        public MainWindowViewModel()
        {
            ExitCommand = new Command(ExitCommandAction, CanUseExitCommand);
            RulesCommand = new Command(RulesCommandAction, CanUseRulesCommand);
        }

        #region CanUseCommands

        private bool CanUseExitCommand(object p) => true;

        private bool CanUseRulesCommand(object p) => true;
        #endregion

        #region Commands Actions
        private void ExitCommandAction(object p)
        {
            Environment.Exit(0);
        }
        private void RulesCommandAction(object p)
        {
            RulesWindow.ShowDialog();
        }
        #endregion

    }
}
