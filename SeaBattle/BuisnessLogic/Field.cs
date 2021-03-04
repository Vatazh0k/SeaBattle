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
    public class Field
    {
        public string[,] field { get; set; } = new string[11, 11];

        #region ChangeAttackDirection_Data
        int CountOfAttackInOneDirection = 0;
        int totalShipsCount = 10;
        bool isHorizontal = true;
        bool isPositiveDirection = true;
        bool isKilled = false;
        bool isMissed = true;
        bool isHintButNotKilled = false;
        int fixed_I;
        int fixed_J;
        #endregion
        #region Const_Data
        private const string ShipsMark = "O";
        private const string KilledMark = "X";
        private const string MissedMark = " ";
        #endregion

        public Field()
        { }
        public bool ChangeShipsDirection(string[,] field, int cell)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);
            int CurrentDeck = 0;

            bool direction = DeterminingTheDirection(indexes.I_index, indexes.J_index, field);

            int DecksInShipCount = CountingDecks(field, indexes, ref CurrentDeck, direction);

            ShipsModification(direction, indexes, CurrentDeck, DecksInShipCount, null);

            bool canPutShip = CanPutShip(field, indexes.I_index, indexes.J_index, DecksInShipCount, !direction);

            ShipsModification(direction, indexes, CurrentDeck, DecksInShipCount, ShipsMark);
            if (canPutShip is false)
            return false;
            return true;
        }
        public int CountingDecks(string[,] Field, CellIndex indexes, ref int firtShipDeck, bool isHorizontal = true)
        {
            int GeneralDeksCountInShip = 1;
            firtShipDeck = 0;
            try
            {
                for (int k = 1; k <= 4; k++)
                {
                    int firstIndex = indexes.I_index;
                    int secondIndex = indexes.J_index;

                    _ = isHorizontal is true ? 
                     secondIndex = indexes.J_index + k :
                     firstIndex = indexes.I_index + k ;

                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) ||
                       (Field[firstIndex, secondIndex] == MissedMark))
                        break;

                    GeneralDeksCountInShip++;
                }
            }
            catch (Exception) { }
            try
            {
                for (int k = -1; k >= -4; k--)
                {
                    int firstIndex = indexes.I_index;
                    int secondIndex = indexes.J_index;

                    _ = isHorizontal is true ? 
                     secondIndex = indexes.J_index + k : 
                     firstIndex = indexes.I_index + k;

                    if (String.IsNullOrEmpty(Field[firstIndex, secondIndex]) || 
                       (Field[firstIndex, secondIndex] == MissedMark))
                        break;

                    firtShipDeck++;
                    GeneralDeksCountInShip++;
                }
            }
            catch (Exception) { }

            return GeneralDeksCountInShip;

        }
        public bool DeterminingTheDirection(int firstIndex, int secondIndex, string[,] Field)
        {
            bool isHorizontal = true;//random true or false

            for (int i = firstIndex; i < 11; i++)
            {
                for (int j = secondIndex; j < 11; j++)
                {
                    if (Field[i, j - 1] is ShipsMark || (j + 1 <= 10 && Field[i, j + 1] is ShipsMark)) return true;
                    if (Field[i, j - 1] is KilledMark || (j + 1 <= 10 && Field[i, j + 1] is KilledMark)) return true;

                    if (Field[i - 1, j] is KilledMark || (i + 1 <= 10 && Field[i + 1, j] is KilledMark)) return false;
                    if (Field[i - 1, j] is ShipsMark || (i + 1 <= 10 && Field[i + 1, j] is ShipsMark)) return false;
                }
            }

            return isHorizontal;
        }
        public string[,] FieldAutoGeneration(string[,] Field)
        {
            var Random = new Random();
            int ShipCount = 1;
            int DeksCount = 4;

            for (int i = ShipCount; i <= 4; i++)
            {
                for (int j = 1; j <= ShipCount; j++)
                {
                    int firstIndex = Random.Next(1, 11);
                    int secondIndex = Random.Next(1, 11);

                    int direction = Random.Next(1, 3);

                    bool isHorizontal = direction is 1 ? true : false;

                    bool canPutShip = CanPutShip(Field, firstIndex, secondIndex, DeksCount, isHorizontal);

                    if (canPutShip is false)
                    {
                        j--;
                        continue;
                    }
                    if (canPutShip is true)
                    {

                        for (int k = 0; k < DeksCount; k++)
                        {
                            if (isHorizontal is true)
                                Field[firstIndex, secondIndex + k] = ShipsMark;

                            if (isHorizontal is false)
                                Field[firstIndex + k, secondIndex] = ShipsMark;
                        }

                    }
                }

                ShipCount++;
                DeksCount--;
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
        public string[,] CleanField(string[,] Field)
        {
            return Field = new string[11, 11];
        }
        public bool CanMakeDamage(string[,] field, int Cell)
        {
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);
            if (field[Indexes.I_index, Indexes.J_index] is KilledMark ||
                field[Indexes.I_index, Indexes.J_index] is MissedMark)
            {
                return false;
            }
            return true;
        }
        public string[,] UserAttck(string[,] Field, CellIndex Indexes)
        {

            bool isMissed = IsMissed(Field, Indexes);

            if (isMissed is true)
            {
                MarkTheShip(Field, Indexes, MissedMark);
                return Field;
            }

            MarkTheShip(Field, Indexes, KilledMark);

            bool isShipKilled = IsKilled(Field, Indexes);

            if (isShipKilled is false)
                return Field;

            ShipsFuneral(Field, Indexes);

            return Field;
        }
        public string[,] ComputerAttack(string[,] Field)
        {
            bool isComputerField = true;
            CellIndex indexes = new CellIndex();
            CountOfAttackInOneDirection = 0;
            int AttackInOnePoint = 0;

            while (isComputerField != false)
            {
                if (isHintButNotKilled is false)
                {
                    indexes = SearchRandomCell(Field);
                    fixed_I = indexes.I_index;
                    fixed_J = indexes.J_index;
                }
                if (isHintButNotKilled is true)
                {
                    indexes = ChangeAttackDirection(indexes, Field);//Перепесать эту херь
                    if (Field[indexes.I_index, indexes.J_index] is KilledMark || Field[indexes.I_index, indexes.J_index] is MissedMark)
                    {
                        isPositiveDirection = !isPositiveDirection;
                        CountOfAttackInOneDirection = 0;
                        if (AttackInOnePoint is 2)
                        {
                            AttackInOnePoint = 0;
                             isHorizontal = !isHorizontal;
                            continue;
                        }
                        AttackInOnePoint++;
                        continue;

                    }
                }

                isMissed = IsMissed(Field, indexes);

                if (isMissed is true)
                {
                    MarkTheShip(Field, indexes, MissedMark);
                    isComputerField = false;
                    if(isHintButNotKilled is false)
                    isHintButNotKilled = false;
                    if (CountOfAttackInOneDirection is 0)
                    isHorizontal = !isHorizontal;
                    if (CountOfAttackInOneDirection != 0)
                    isPositiveDirection = !isPositiveDirection;
                    return Field;
                }

                MarkTheShip(Field, indexes, KilledMark);

                isKilled = IsKilled(Field, indexes);

                if (isKilled is true)
                {
                    ShipsFuneral(Field, indexes);
                    totalShipsCount--;
                    isHintButNotKilled = false;
                    if (totalShipsCount is 0) return Field;
                    continue;
                }
                isHintButNotKilled = true;
                CountOfAttackInOneDirection++;
            }
            return Field;
        }

        #region Private Methods
        private CellIndex SearchRandomCell( string[,] Field)
        {
            bool canDamage = false;
            Random random = new Random();
            CellIndex index = new CellIndex();

            while (canDamage != true)
            {
                index.I_index = random.Next(1, 11);
                index.J_index = random.Next(1, 11);

                int Cell = CellsConverter.ConvertIndexesToCell(index);

                canDamage = CanMakeDamage(Field, Cell);
            }
            return index;
        }
        private void ShipsFuneral(string[,] field, CellIndex Indexes)
        {
            int FirstShipsDeck = 0;

            bool direction = DeterminingTheDirection(Indexes.I_index, Indexes.J_index, field);
            int DecksCount = CountingDecks(field, Indexes, ref FirstShipsDeck, direction);

            int y_Axis_Ships = Indexes.I_index + 1;
            int x_Axis_Ships = Indexes.J_index + DecksCount - FirstShipsDeck;

            int n = Indexes.I_index - 1;
            int m = Indexes.J_index - 1 - FirstShipsDeck;

            if (direction is false)
            {
                 n = Indexes.I_index - 1 - FirstShipsDeck;
                 m = Indexes.J_index - 1;

                y_Axis_Ships = Indexes.I_index + DecksCount - FirstShipsDeck;
                x_Axis_Ships = Indexes.J_index + 1;
            }
            for (int N = n; N <= y_Axis_Ships; N++)
            {
                for (int M = m; M <= x_Axis_Ships; M++)
                {
                    if (N == 11 || N == -1) continue;
                    if (M == 11 || M == -1) break;
                    if (field[N, M] == KilledMark) continue;
                    field[N, M] = MissedMark;
                }
            }

        }
        private bool IsKilled(string[,] field, CellIndex Indexes)
        {
            int FirstShipsDeck = 0;
            bool direction = DeterminingTheDirection(Indexes.I_index, Indexes.J_index, field);

            int DecksCount = CountingDecks(field, Indexes, ref FirstShipsDeck, direction);

            for (int i = 0; i < DecksCount; i++)
            {
                int I = Indexes.I_index;
                int J = Indexes.J_index;

                _ = direction is true ? 
                    J = Indexes.J_index - FirstShipsDeck + i:
                    I = Indexes.I_index - FirstShipsDeck + i;

                if (field[I, J] is KilledMark)
                    continue;

                return false;
               
            }
            return true;
        }
        private bool IsMissed(string[,] Field, CellIndex indexes)
        {
            if (Field[indexes.I_index, indexes.J_index] is ShipsMark) 
            return false;
            return true;
        }
        private void MarkTheShip(string[,] Field, CellIndex Indexes, string Mark)
        {
            Field[Indexes.I_index, Indexes.J_index] = Mark;
        }
        private CellIndex ChangeAttackDirection(CellIndex indexes, string[,] Field)
        {
            if (CountOfAttackInOneDirection >= 1)
            {
            ChangeDirection:
                if (isHorizontal)
                {
                    if (isPositiveDirection is true)
                    {
                        if (indexes.J_index + 1 <= 10)
                        {
                            indexes.J_index++;
                            isPositiveDirection = true;
                            return indexes;
                        }
                        isPositiveDirection = !isPositiveDirection;
                    }
                    if (isPositiveDirection is false)
                    {
                        if (indexes.J_index - 1 >= 1)
                        {
                            indexes.J_index--;
                            isPositiveDirection = false;
                            return indexes;
                        }
                        isPositiveDirection = !isPositiveDirection;
                        goto ChangeDirection;
                    }
                }
                if (!isHorizontal)
                {
                    if (isPositiveDirection is true)
                    {
                        if (indexes.I_index + 1 <= 10)
                        {
                            indexes.I_index++;
                            isPositiveDirection = true;
                            return indexes;
                        }
                        isPositiveDirection = !isPositiveDirection;
                    }
                    if (isPositiveDirection is false)
                    {
                        if (indexes.I_index - 1 >= 1)
                        {
                            indexes.I_index--;
                            isPositiveDirection = false;
                            return indexes;
                        }
                        isPositiveDirection = !isPositiveDirection;
                        goto ChangeDirection;
                    }
                }
            }

        _ChangeDirection:
            if (isHorizontal && isPositiveDirection)
            {
                if (fixed_J < 10 && fixed_J >= 1)
                {
                    indexes.I_index = fixed_I;
                    indexes.J_index = fixed_J + 1;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (isHorizontal && !isPositiveDirection)
            {
                if (fixed_J > 1 && fixed_J <= 10)
                {
                    indexes.I_index = fixed_I;
                    indexes.J_index = fixed_J - 1;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto _ChangeDirection;
            }
            if (!isHorizontal && isPositiveDirection)
            {
                if (fixed_I < 10 && fixed_I >= 1)
                {
                    indexes.I_index = fixed_I + 1;
                    indexes.J_index = fixed_J;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (!isHorizontal && !isPositiveDirection)
            {
                if (fixed_I > 1 && fixed_I <= 10)
                {
                    indexes.I_index = fixed_I - 1;
                    indexes.J_index = fixed_J;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto _ChangeDirection;
            }

            return indexes;
        }
        private void ShipsModification(bool direction, CellIndex indexes, int CurrentDeck, int DecksCount, string mark)
        {
            if (direction is true)
            {
                int FirstDeckOfHorizontalShip = indexes.J_index - CurrentDeck;
                for (int i = 0; i < DecksCount; i++)
                    field[indexes.I_index, FirstDeckOfHorizontalShip + i] = mark;
            }

            if (direction is false)
            {
                int FirstDeckOfVerticalShip = indexes.I_index - CurrentDeck;
                for (int i = 0; i < DecksCount; i++)
                    field[FirstDeckOfVerticalShip + i, indexes.J_index] = mark;
            }
        }
#endregion
    }
}