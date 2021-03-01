using SeaBattle.Infrastructure.Converters;
using SeaBattle.Resource;
using SeaBattle.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SeaBattle.BuisnessLogic
{
    class Field
    {
        public string[,] field { get; set; } = new string[11, 11];

        private const string ShipsMark = "O";

        public Field()
        {


        }

        public string[,] ChangeShipsDirection(string[,] field, int cell)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);

            int DecksInCurrentShip = 0;

            return field;
        }
        public string[,] FieldAutoGeneration(string[,] Field)
        {
            var Random = new Random();
            int ShipCount = 4;
            int DeksCount = 1;

            for (int i = ShipCount; i >= 1; i--)
            {
                for (int j = 1; j <= ShipCount; j++)
                {
                    int firstIndex = Random.Next(1, 11);
                    int secondIndex = Random.Next(1, 11);

                    int direction = Random.Next(1, 3);

                    bool isHorizontal = direction is 1 ? true : false;

                    bool canPutShip = CanPutShip(Field, firstIndex, DeksCount, DeksCount, isHorizontal);

                    if (canPutShip is false)
                    {
                        j--;
                        continue;
                    }
                    if (canPutShip is true)
                    {

                        for (int k = 0; k < DeksCount; k++)
                        {
                            //if (isHorizontal is true)
                            //    ComputerField[firstIndex, secondIndex + k + 1] = ShipsMark;

                            //if (isHorizontal is false)
                            //    ComputerField[firstIndex + k + 1, secondIndex] = ShipsMark;
                        }

                    }
                }

                ShipCount--;
                DeksCount++;
            }
            return Field;
        }
        public bool CanPutShip(string[,] filed, int Current_I, int Current_J, int DeksCount, bool isHorizontal = true)
        {
            int CellsToCheckIn_X_Axis = 2;
            int CellsToCheckIn_Y_Axis = 2;

            int J_Iteration_Count = 0;
            int I_teration_Count = 0;

            _ = isHorizontal is false ?
              CellsToCheckIn_Y_Axis += DeksCount - 1 :
              CellsToCheckIn_X_Axis += DeksCount - 1;


            for (int i = Current_I - 1; i < Current_I + CellsToCheckIn_Y_Axis; i++)
            {
                J_Iteration_Count = 0;
                for (int j = Current_J - 1; j < Current_J + CellsToCheckIn_X_Axis; j++)
                {
                    if (j < 0 || j > 10 || i < 0 || i > 10) return false;
                    if (!string.IsNullOrEmpty(filed[i, j])) return false;

                    J_Iteration_Count++;
                    if (J_Iteration_Count == CellsToCheckIn_X_Axis && j == 10)
                        break;
                }
                I_teration_Count++;
                if (I_teration_Count == CellsToCheckIn_Y_Axis && i == 10)
                    break;
            }
            return true;
        }
        public void CleanField(string[,] Field)
        {
            Field = new string[11, 11];
        }
        public string[,] Damaging(string[,] Field, CellIndex Indexes)
        {

            return Field;
        }
        public string[,] PutShipInCurrentCell(string[,] field, int cell, int ShipsSize)
        {
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(cell);

            if (CanPutShip(field, Indexes.I_index, Indexes.J_index, ShipsSize))
                for (int i = 0; i < ShipsSize; i++)
                    field[Indexes.I_index, Indexes.J_index + i] = ShipsMark;
                
            return field;
        }

        #region Private Methods
      
        #endregion
    }
}
