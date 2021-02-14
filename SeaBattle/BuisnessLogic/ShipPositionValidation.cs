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
        public static bool PositionValidationLogic(int Fixed_I, int Fixed_J, string[,] ships, int decksCount, bool isHorizontal = true)
        {
            int CellsToCheckIn_X_Axis = 2;
            int CellsToCheckIn_Y_Axis = decksCount + 1;

            bool CanPutShipInCurrentCell = isHorizontal is true ?
            ValidationAlgorithm(Fixed_I, Fixed_J, ships, CellsToCheckIn_Y_Axis, CellsToCheckIn_X_Axis) :
            ValidationAlgorithm(Fixed_I, Fixed_J, ships, CellsToCheckIn_X_Axis, CellsToCheckIn_Y_Axis);

            if (CanPutShipInCurrentCell is false)
                return false;
            return true;
           

        }
        #region PrivateMethods
        private static bool ValidationAlgorithm(int CellIndex_I, int CellIndex_J, string[,]ships, int X_Pos_ShipsDeks, int Y_Pos_ShipsDeks)
        {
            int J_Iteration_Count = 0;
            int I_teration_Count = 0;
            try
            {
                for (int i = CellIndex_I - 1; i < CellIndex_I + Y_Pos_ShipsDeks; i++)
                {
                    J_Iteration_Count = 0;
                    for (int j = CellIndex_J - 1; j < CellIndex_J + X_Pos_ShipsDeks; j++)
                    {
                        if (!string.IsNullOrEmpty(ships[i, j])) return false;

                        J_Iteration_Count++;
                        if (J_Iteration_Count == X_Pos_ShipsDeks && j == 10)
                            break;
                    }
                    I_teration_Count++;
                    if (I_teration_Count == Y_Pos_ShipsDeks && i == 10)
                        break;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
  