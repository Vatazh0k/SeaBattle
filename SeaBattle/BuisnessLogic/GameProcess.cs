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
        public static bool ChekedTheShipState(string[,] Field, int i, int j)
        {
            int IndexOfTheFirstShpsDeck = 0;

            int DecksCount = CountingDecksCount(Field, i, j, ref IndexOfTheFirstShpsDeck);

            IndexOfTheFirstShpsDeck = j - IndexOfTheFirstShpsDeck;

            bool isTheShipKilled = ShipState(Field, i, IndexOfTheFirstShpsDeck, DecksCount);

            if(isTheShipKilled)
            {
                Field = ShipsFuneral(Field, i, IndexOfTheFirstShpsDeck, DecksCount);
                return true;
            }
            return false;
        }
        public static string[,] ShipsFuneral(string[,] Field, int i, int j, int DecksCount)
        {

            for (int n = i - 1; n <= i + 1; n++)
            {
                for (int m = j - 1; m <= j + DecksCount; m++)
                {

                    if (n == 11) continue;
                    if (m == 11) break;
                    if (Field[n, m] == KilledMark) continue;
                    Field[n, m] = MissedMark;
                }
            }
            return Field;
        }
        public static int CountingDecksCount(string[,] Field, int i, int j, ref int firtShipDeck)
        {
            int decksCount = 1;
            try
            {
                for (int k = 1; k <= 4; k++)
                {
                    if (String.IsNullOrEmpty(Field[i, j + k]) || (Field[i, j + k] == MissedMark))
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
                    if (String.IsNullOrEmpty(Field[i, j + k]) || (Field[i, j + k] == MissedMark))
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
        public static bool ShipState(string[,] field, int i, int j, int decksCount)
        {

            for (int k = 0; k < decksCount; k++)
            {
                if (field[i, j + k] == KilledMark)
                {
                    continue;
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