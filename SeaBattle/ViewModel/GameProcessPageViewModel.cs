using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.ViewModel
{
    class GameProcessPageViewModel : ViewModelBase
    {
        private MainWindowViewModel vm;

        public GameProcessPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;
        }
    }
}
