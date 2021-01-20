using SeaBattle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle
{
    public partial class MainWindow : Window
    {
        #region Data
        private Button[,] button = new Button[11, 11];

        private char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        private int CellsCounter = -1;

        private MainWindowViewModel vm;
        #endregion

        public MainWindow()
        {
            vm = new MainWindowViewModel();
            DataContext = vm;
            InitializeComponent();

            CreateField(vm);
        }

        #region PrivateMethods

        private void CreateField(MainWindowViewModel vm)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    CellsCounter++;
                    if (i is 0 && j is 0) continue;

                    GridCreating();

                    if (i is 0)
                    {
                        ButtonSettings(i, j);
                        button[i, j].Content = Alphabet[j - 1];
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j);

                        continue;
                    }

                    if (j is 0)
                    {
                        ButtonSettings(i, j);
                        button[i, j].Content = i;
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j);

                        continue;
                    }


                    ButtonSettings(i, j);

                    ButtonAdd(i, j);



                }

            }

        }
        private void GridCreating()
        {
            ColumnDefinition column = new ColumnDefinition();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(35);
            column.Width = new GridLength(35);
            Field.ColumnDefinitions.Add(column);
            Field.RowDefinitions.Add(row);
        }
        private void ButtonSettings(int i, int j)
        {
            button[i, j] = new Button();
            button[i, j].BorderBrush = Brushes.Gray;

            Binding BorderBinding = new Binding();
            BorderBinding.Source = vm;
            BorderBinding.Path = new PropertyPath($"Ships[{CellsCounter}].Border");
            BorderBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.BorderThicknessProperty, BorderBinding);

            Binding colorBinding = new Binding();
            colorBinding.Source = vm;
            colorBinding.Path = new PropertyPath($"Ships[{CellsCounter}].Color");
            colorBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ForegroundProperty, colorBinding);

            Binding ContentBinding = new Binding();
            ContentBinding.Source = vm;
            ContentBinding.Path = new PropertyPath($"Ships[{CellsCounter}].Content");
            ContentBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ContentProperty, ContentBinding);

            button[i, j].Name = $"C{CellsCounter}";
            button[i, j].Width = 35;
            button[i, j].Height = 35;
            button[i, j].Background = Brushes.White;
            button[i, j].Command = vm.CreatingShipsCommand;
            button[i, j].CommandParameter = button[i, j].Name;
        }
        private void ButtonAdd(int i, int j)
        {
            Grid.SetRow(button[i, j], i);
            Grid.SetColumn(button[i, j], j);
            Field.Children.Add(button[i, j]);
        }

        #endregion

    }
}
