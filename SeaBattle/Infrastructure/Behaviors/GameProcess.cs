using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.BuisnessLogic
{
    public static class GameProcess
    {
        public static bool ShipsIntegityChecked(string[,] Field, int i, int j, bool Direction)
        {
            int IndexOfTheFirstDeck = 0;
            int FirstIndex = i;
            int SecondIndex = j;


            int DecksCount = CountingDecks(Field, i, j, ref IndexOfTheFirstDeck, Direction);

            _ = Direction is false ?
            FirstIndex = i - IndexOfTheFirstDeck :
            SecondIndex = j - IndexOfTheFirstDeck;


            bool isTheShipKilled = CheckedShipState(Field, FirstIndex, SecondIndex, DecksCount, Direction);

            if (isTheShipKilled)
            {
                Field = ShipsFuneral(Field, FirstIndex, SecondIndex, DecksCount, Direction);
                return true;
            }
            return false;
        }

        public static bool Damaging(string[,] Field, int i, int j)
        {
            if (Field[i, j] is null)
            {
                Field = Missed(Field, i, j);
                return true;
            }
            else
            {
                Field = Hit(Field, i, j);
                return false;
            }
        }
        public static string[,] ShipsFuneral(string[,] Field, int i, int j, int DecksCount, bool isHorizontal = true)
        {
            const string KilledMark = "O";
            const string MissedMark = "X";
            int y_Axis_Ships = i + 1;
            int x_Axis_Ships = j + DecksCount;

            if (isHorizontal is false)
            {
                y_Axis_Ships = i + DecksCount;
                x_Axis_Ships = j + 1;
            }

            for (int n = i - 1; n <= y_Axis_Ships; n++)
            {
                for (int m = j - 1; m <= x_Axis_Ships; m++)
                {

                    if (n == 11 || n == -1) continue;
                    if (m == 11 || n == -1) break;
                    if (Field[n, m] == KilledMark) continue;
                    Field[n, m] = MissedMark;
                }
            }
            return Field;
        }
        public static int CountingDecks(string[,] Field, int i, int j, ref int firtShipDeck, bool isHorizontal = true)
        {
            const string MissedMark = "X";
            int GeneralDeksCountInShip = 1;
            firtShipDeck = 0;
            try
            {
                for (int k = 1; k <= 4; k++)
                {
                    int firstIndex = i;
                    int secondIndex = j;

                    _ = isHorizontal is true ? secondIndex = j + k : firstIndex = i + k;

                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) || (Field[firstIndex, secondIndex] == MissedMark))
                        break;

                    GeneralDeksCountInShip++;
                }
            }
            catch (Exception) { }
            try
            {
                for (int k = -1; k >= -4; k--)
                {
                    int firstIndex = i;
                    int secondIndex = j;

                    _ = isHorizontal is true ? secondIndex = j + k : firstIndex = i + k;

                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) || (Field[firstIndex, secondIndex] == MissedMark))
                        break;

                    firtShipDeck++;
                    GeneralDeksCountInShip++;
                }
            }
            catch (Exception) { }

            return GeneralDeksCountInShip;

        }
        public static bool CheckedShipState(string[,] field, int i, int j, int decksCount, bool isHorizontal = true)
        {
            const string KilledMark = "O";

            for (int k = 0; k < decksCount; k++)
            {
                int I = i, J = j;
                _ = isHorizontal is true ? J = j + k : I = i + k;

                if (field[I, J] == KilledMark)
                    continue;

                return false;
            }
            return true;
        }
        #region PrivateMethods
        private static string[,] Missed(string[,] Field, int i, int j)
        {
            const string MissedMark = "X";

            Field[i, j] = MissedMark;
            return Field;
        }
        private static string[,] Hit(string[,] Field, int i, int j)
        {
            const string KilledMark = "O";

            Field[i, j] = KilledMark;
            return Field;
        }
        #endregion 
    }
}  