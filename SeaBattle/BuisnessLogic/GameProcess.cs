using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.BuisnessLogic
{
    public static class GameProcess
    {
        private const string MissedMark = "X";
        private const string KilledMark = "O";
        public static bool DamageCreating(string[,] Field, int i, int j)
        {
            if (Field[i, j] is null)
            {
                Field = MissedAttack(Field, i, j);
                return true;
            }
            else
            {
                Field = SucsessfullAttack(Field, i, j);
                return false;
            }
        }
        public static bool ChekedTheShipState(string[,] Field, int i, int j, bool Direction)
        {
            int IndexOfTheFirstShpsDeck = 0;
            int FirstIndex = i;
            int SecondIndex = j;


            int DecksCount = CountingDecksCount(Field, i, j, ref IndexOfTheFirstShpsDeck, Direction);

            _ = Direction is false?
            FirstIndex = i - IndexOfTheFirstShpsDeck:
            SecondIndex = j - IndexOfTheFirstShpsDeck;


            bool isTheShipKilled = ShipState(Field, FirstIndex, SecondIndex, DecksCount, Direction);

            if(isTheShipKilled)
            {
                Field = ShipsFuneral(Field, FirstIndex, SecondIndex, DecksCount, Direction);
                return true;
            }
            return false;
        }
        public static string[,] ShipsFuneral(string[,] Field, int i, int j, int DecksCount, bool isHorizontal = true)
        {
            int y_Axis_Ships = i + 1;
            int x_Axis_Ships = DecksCount;

            if (isHorizontal is false)
            {
                y_Axis_Ships = i + DecksCount;
                x_Axis_Ships = 1;
            }            

            for (int n = i - 1; n <= y_Axis_Ships; n++)
            {
                for (int m = j - 1; m <= j + x_Axis_Ships; m++)
                {

                    if (n == 11) continue;
                    if (m == 11) break;
                    if (Field[n, m] == KilledMark) continue;
                    Field[n, m] = MissedMark;
                }
            }
            return Field;
        }
        public static int CountingDecksCount(string[,] Field, int i, int j, ref int firtShipDeck, bool isHorizontal = true)
        {

            int decksCount = 1;
            int firstIndex = 0;
            int secondIndex = 0;
            try
            {
                for (int k = 1; k <= 4; k++)
                {
                    if (isHorizontal is true)
                    {
                        firstIndex = i;
                        secondIndex = j + k;                        
                    }
                    if (isHorizontal is false)
                    {
                        firstIndex = i + k;
                        secondIndex = j;
                    }
                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) || (Field[firstIndex, secondIndex] == MissedMark))
                    {
                        break;
                    }
                    decksCount++;
                }
            }
            catch (Exception) { }
            try
            {
                for (int k = -1; k >= -4; k--)
                {
                    if (isHorizontal is true)
                    {
                        firstIndex = i;
                        secondIndex = j + k;
                    }
                    if (isHorizontal is false)
                    {
                        firstIndex = i + k;
                        secondIndex = j;
                    }
                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) || (Field[firstIndex, secondIndex] == MissedMark))
                    {
                        break;
                    }
                    firtShipDeck++;
                    decksCount++;
                }
            }
            catch (Exception) { }

            return decksCount;

        }
        public static bool ShipState(string[,] field, int i, int j, int decksCount, bool isHorizontal = true)
        {

            for (int k = 0; k < decksCount; k++)
            {
                if (isHorizontal is false)
                {
                    if (field[i + k, j] == KilledMark)
                    {
                        continue;
                    }
                }
                if (isHorizontal is true)
                {
                    if (field[i, j + k] == KilledMark)
                    {
                        continue;
                    }
                }
                return false;
            }
            return true;
        }
        #region PrivateMethods
        private static string[,] MissedAttack(string[,] Field, int i, int j)
        {
            Field[i, j] = MissedMark;
            return Field;
        }
        private static string[,] SucsessfullAttack(string[,] Field, int i, int j)
        {
            Field[i, j] = KilledMark;
            return Field;
        }
        #endregion 
    } 
} 