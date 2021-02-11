using SeaBattle.BuisnessLogic;
using SeaBattle.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle.View.Pages
{
    /// <summary>
    /// Interaction logic for ComputerFieldPage.xaml
    /// </summary>
    public partial class ComputerFieldPage : Page
    {
        public ComputerFieldPage(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            var vm = new ComputerFieldPageViewModel(mainWindowViewModel);
            DataContext = vm;

            FieldCreating<ComputerFieldPageViewModel>.CreateField(vm, Field, vm.MakeDamageCommand, mainWindowViewModel);
        }
    }
}

