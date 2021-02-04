using SeaBattle.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace SeaBattle.BuisnessLogic
{
    public static class FieldCreating<T>
    {
        public static void CreateField(T vm, Grid Field, ICommand command)
        {
            #region Data
            Button[,] button = new Button[11, 11];

            char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            #endregion

            #region FieldCreating
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i is 0 && j is 0) continue;

                    GridCreating(Field);

                    if (i is 0)
                    {
                        ButtonSettings(i, j, button, vm, command);
                        button[i, j].Content = Alphabet[j - 1];
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j, button, Field);

                        continue;
                    }

                    if (j is 0)
                    {
                        ButtonSettings(i, j, button, vm, command);
                        button[i, j].Content = i;
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j, button, Field);

                        continue;
                    }


                    ButtonSettings(i, j, button, vm, command);

                    ButtonAdd(i, j, button, Field);



                }
            }
            #endregion

        }

        #region PrivateMethods

        private static void GridCreating(Grid Field)
        {
            ColumnDefinition column = new ColumnDefinition();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(35);
            column.Width = new GridLength(35);
            Field.ColumnDefinitions.Add(column);
            Field.RowDefinitions.Add(row);
        }
        private static void ButtonSettings(int i, int j, Button[,] button, T vm, ICommand command)
        {
            button[i, j] = new Button();
            button[i, j].BorderBrush = Brushes.Gray;

            Binding BorderBinding = new Binding();
            BorderBinding.Source = vm;
            BorderBinding.Path = new PropertyPath($"Ships[{i*11+j}].Border");
            BorderBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.BorderThicknessProperty, BorderBinding);

            Binding colorBinding = new Binding();
            colorBinding.Source = vm;
            colorBinding.Path = new PropertyPath($"Ships[{i * 11 + j}].Color");
            colorBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ForegroundProperty, colorBinding);

            Binding ContentBinding = new Binding();
            ContentBinding.Source = vm;
            ContentBinding.Path = new PropertyPath($"Ships[{i * 11 + j}].Content");
            ContentBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ContentBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ContentProperty, ContentBinding);

            button[i, j].Name = $"C{i * 11 + j}";
            button[i, j].Width = 35;
            button[i, j].Height = 35;
            button[i, j].Background = Brushes.White;
            button[i, j].Command = command;
            button[i, j].CommandParameter = button[i, j].Name;
        }
        private static void ButtonAdd(int i, int j, Button[,] button, Grid Field)
        {
            Grid.SetRow(button[i, j], i);
            Grid.SetColumn(button[i, j], j);
            Field.Children.Add(button[i, j]);
        }

        #endregion
    }
}   