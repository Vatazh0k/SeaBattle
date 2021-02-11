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
        public static bool PositionValidationLogic(int Fixed_I, int Fixed_J, string[,] ships, int decksCount, bool isHorizontal)//преедать дирекшн и менять первый или второ цикл
        {
            int J_Iteration_Count = 0;
            int I_teration_Count = 0;
            int CellIndex_I = 0;
            int CellIndex_J = 0;

            if (isHorizontal is true)
            {
                CellIndex_I = Fixed_I;
                CellIndex_J = Fixed_J;
            }
            if (isHorizontal is false)
            {
                CellIndex_I = Fixed_J;
                CellIndex_J = Fixed_I;
            }

            try
            {
                for (int i = CellIndex_I - 1; i < CellIndex_I + 2; i++)
                {
                    J_Iteration_Count = 0;
                    for (int j = CellIndex_J - 1; j < CellIndex_J + decksCount + 1; j++)
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


     
        }
       
    } 
}
  