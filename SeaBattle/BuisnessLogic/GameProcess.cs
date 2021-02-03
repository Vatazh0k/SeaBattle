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
            if (String.IsNullOrEmpty(Field[i, j]) || (Field[i, j] == MissedMark))
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

        #region PrivateMethods

        private static string[,] MissedAttack(string[,] Field, int i, int j)
        {
            Field[i, j] = MissedMark;
            return Field;
        }
        private static string[,] SucsessfullAttack(string[,] Field, int i, int j)
        {
            Field[i, j] = KilledMark;

            int IndexOfTheFirstShpsDeck = 0;
            int DecksCount = CountingDecksCount(Field, i, j, ref IndexOfTheFirstShpsDeck);
            IndexOfTheFirstShpsDeck = j - IndexOfTheFirstShpsDeck;

            bool isTheShipKilled = CheckingTheShipState(Field, i, IndexOfTheFirstShpsDeck, DecksCount);

            if (isTheShipKilled is true)
            {
                Field = ShipsFuneral(Field, i, IndexOfTheFirstShpsDeck, DecksCount);
            }
            
            return Field;
        }
        private static string[,] ShipsFuneral(string[,] Field, int i, int j, int DecksCount)
        {

            for (int n = i - 1; n <= i + 1; n++)
            {
                for (int m = j - 1; m <= j + DecksCount; m++)
                {

                    if (n == 11) continue;
                    if (m == 11) break;
                    if (Field[n, m] == KilledMark) Field[n, m] = "o";
                    else
                    Field[n, m] = MissedMark;
                }
            }
            return Field;
        }
        private static int CountingDecksCount(string[,] Field, int i, int j, ref int firtShipDeck)
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
        private static bool CheckingTheShipState(string[,] Field, int i, int j, int deckCount)
        {
            for (int k = 0; k < deckCount; k++)
            {
                if (Field[i, j + k] == KilledMark)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
        #endregion
    }
} 