using SeaBattle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaBattle.BuisnessLogic
{
    static class ShipPositionValidation
    {
        public static bool PositionValidationLogic(int cell, string[,] ships, int decksCount)
        {
            #region Data
            int Fixed_I_Index = 0;
            int Fixed_J_Index = 0;

            int J_Iteration_Count = 0;
            int I_teration_Count = 0;
            #endregion

            #region IndexsSearch
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i * 11 + j == cell)
                    {
                        Fixed_I_Index = i;
                        Fixed_J_Index = j;
                    }
                }
            }
            #endregion

            #region PosValidation
            try
            {
                for (int i = Fixed_I_Index - 1; i < Fixed_I_Index + 2; i++)
                {
                    J_Iteration_Count = 0;
                    for (int j = Fixed_J_Index - 1; j < Fixed_J_Index + decksCount + 1; j++)
                    {
                        if (!string.IsNullOrEmpty(ships[i, j])) return false;

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

            return true;
            #endregion

     
        }
       
    }
}
 