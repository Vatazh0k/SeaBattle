using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.BuisnessLogic
{
    public static class ShipsGeneration
    {
        private const string ShipMark = "O";
        public static string[,] ShipGeneration(int ShipCount, int DeksCount, string[,] tempArr)
        {
            var Random = new Random();

            for (int k = 1; k <= ShipCount; k++)
            {
                int i = Random.Next(1, 10);
                int j = Random.Next(1, 10);
             

                if (!ShipPositionValidation.PositionValidationLogic(i, j, tempArr, DeksCount))
                {
                    i--;
                    continue;
                }
                else
                {
                    switch (DeksCount)
                    {
                        default:
                            break;

                        case 1:
                            tempArr[i, j] = ShipMark;
                            break;

                        case 2:
                            tempArr[i, j] = ShipMark;
                            tempArr[i, j+1] = ShipMark;
                            break;


                        case 3:

                            tempArr[i, j] = ShipMark;
                            tempArr[i, j+1] = ShipMark;
                            tempArr[i, j+2] = ShipMark;
                            break;
                        case 4:

                            tempArr[i, j] = ShipMark;
                            tempArr[i, j+1] = ShipMark;
                            tempArr[i, j+2] = ShipMark;
                            tempArr[i, j+3] = ShipMark;
                            break;
                    }

                }
            }
            return tempArr;
        }
    }
}
