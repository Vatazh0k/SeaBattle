using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.ViewModel
{
    class FieldCreatingPageViewModel
    {
        private MainWindowViewModel mainWindowViewModel;


        public FieldCreatingPageViewModel()
        {

        }
        public FieldCreatingPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
    }
}
