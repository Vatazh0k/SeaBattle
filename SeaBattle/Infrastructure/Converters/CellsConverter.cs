using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Infrastructure.Converters
{
    public static class CellsConverter
    {
        public static int ConvertIndexesToCell(CellIndex indexes)
        {
            int cell = 0;
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i == indexes.I_index && j == indexes.J_index)
                    {
                        cell = i * 11 + j;
                        return cell;
                    }
                }
            }
            return cell;
        }

        public static CellIndex ConverCellsToIndexes(int cell)
        {
            CellIndex indexes = new CellIndex();

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i * 11 + j == cell)
                    {
                        indexes.I_index = i;
                        indexes.J_index = j;
                    }
                }
            }

            return indexes;
        }
    }
}
