using SeaBattle.Model;
using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SeaBattle.ViewModel
{
    class ComputerFieldPageViewModel : ViewModelBase
    {
        private MainWindowViewModel vm;
        private ObservableCollection<Ship> _ships;

        public ObservableCollection<Ship> Ships
        {
            get => _ships;
            set => Set(ref _ships, value);
        }
        public ICommand MakeDamageCommand { get; set; }

        public ComputerFieldPageViewModel(MainWindowViewModel vm)
        {
            this.vm = vm;

            var ships = Enumerable.Range(0, 121)
            .Select(i => new Ship
            {
                Content = string.Empty,
                Color = new SolidColorBrush(Colors.White),
                Border = new Thickness(0.5)
            });

            _ships = new ObservableCollection<Ship>(ships);

            //заполнить поле кораблями
        }
    }
}
