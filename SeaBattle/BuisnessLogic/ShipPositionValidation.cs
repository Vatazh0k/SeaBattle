using SeaBattle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SeaBattle.BuisnessLogic
{
    static class ShipPositionValidation
    {
        public static bool PositionValidation(int cell, ObservableCollection<Ship> ships, int ShipsDeckCount)
        {

            if (!PositionValidationLogic(cell, ships, ShipsDeckCount))
            {
                MessageBox.Show("You can create ship here!", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }



            return true;
        }

        private static bool PositionValidationLogic(int cell, ObservableCollection<Ship> ships, int decksCount)
        {
            #region Data
            int Fixed_I_Index = 0;
            int Fixed_J_Index = 0;

            int J_Iteration_Count = 0;
            int I_teration_Count = 0;

            string[,] tempArr = new string[11, 11];
            #endregion

            #region CollectionToArray
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    tempArr[i, j] = ships[i * 11 + j].Content;
                    if (i * 11 + j == cell)
                    {
                        Fixed_I_Index = i;
                        Fixed_J_Index = j;
                    }
                }
            }
            #endregion

            #region PosValidation
            //ЦЕ ВАЛІДАЦІЯ ДЛЯ ВСІХ КОРАБЛІВ У БУДЬ ЯКИЙ КЛІТИНЦІ!!! НАВІТЬ НЕ ОЧІКУВАВ, ЩО ВИЙДЕ ТАК ЗРОБИТИ!)
            try
            {
                for (int i = Fixed_I_Index - 1; i < Fixed_I_Index + 2; i++)
                {
                    J_Iteration_Count = 0;
                    for (int j = Fixed_J_Index - 1; j < Fixed_J_Index + decksCount + 1; j++)
                    {
                        if (!string.IsNullOrEmpty(tempArr[i, j])) return false;

                        J_Iteration_Count++;
                        if (J_Iteration_Count == decksCount + 1 && j == 10)
                            j = (j + 1) + decksCount - 1;
                    }
                    I_teration_Count++;
                    if (I_teration_Count == 2 && i == 10)
                        i = (i + 1) + decksCount - 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
          
            #endregion

            switch (decksCount)
            {
                default:
                    break;

                case 1:
                    ships[cell] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    break;

                case 2:
                    ships[cell] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell + 1] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    break;

                case 3:
                    ships[cell] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell+1] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell+2] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    break;
                case 4:
                    ships[cell] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell+1] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell+2] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    ships[cell+3] = new Ship
                    {
                        Content = "O",
                        Border = new Thickness(1),
                        Color = new SolidColorBrush(Colors.Red)
                    };
                    break;
            }


            return true;
        }

    }
}
