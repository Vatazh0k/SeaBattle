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
        public static bool PositionValidationLogic(int Current_I, int Current_J, string[,] ships, int decksCount, bool isHorizontal = true)
        {
            int CellsToCheckIn_X_Axis = 2;
            int CellsToCheckIn_Y_Axis = 2;

            int J_Iteration_Count = 0;
            int I_teration_Count = 0;

            _ = isHorizontal is false ?
              CellsToCheckIn_Y_Axis += decksCount - 1:
              CellsToCheckIn_X_Axis += decksCount - 1;

            try
            {
                for (int i = Current_I - 1; i < Current_I + CellsToCheckIn_Y_Axis; i++)
                {
                    J_Iteration_Count = 0;
                    for (int j = Current_J - 1; j < Current_J + CellsToCheckIn_X_Axis; j++)
                    {
                        if (!string.IsNullOrEmpty(ships[i, j])) return false;

                        J_Iteration_Count++;
                        if (J_Iteration_Count == CellsToCheckIn_X_Axis && j == 10)
                            break;
                    }
                    I_teration_Count++;
                    if (I_teration_Count == CellsToCheckIn_Y_Axis && i == 10)
                        break;
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
  