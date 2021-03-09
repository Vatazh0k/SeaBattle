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

        public int OneDeckShip { get; set; } = 4;
        public int TwoDeckShip { get; set; } = 3;
        public int ThrieDeckShip { get; set; } = 2;
        public int FourDeckShip { get; set; } = 1;

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
        public bool ChangeShipsDirection(int cell)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);
            int CurrentDeck = 0;

            if (cell is 0) return false;

            bool direction = DeterminingTheDirection(indexes.I_index, indexes.J_index);

            int DecksInShipCount = CountingDecks(indexes, ref CurrentDeck);

            ShipsModification(direction, indexes, CurrentDeck, DecksInShipCount, null);

            bool canPutShip = CanPutShip(indexes.I_index, indexes.J_index, DecksInShipCount, !direction);

            ShipsModification(direction, indexes, CurrentDeck, DecksInShipCount, ShipsMark);
            if (canPutShip is false)
                return false;
            return true;
        }
        public string[,] UserAttck(CellIndex Indexes)
        {

            bool isMissed = IsMissed(Indexes);

            if (isMissed is true)
            {
                MarkTheShip(Indexes, MissedMark);
                return field;
            }

            MarkTheShip(Indexes, KilledMark);

            bool isShipKilled = IsKilled(Indexes);

            if (isShipKilled is false)
                return field;

            ShipsFuneral(Indexes);

            return field;
        }
        public bool CanPutShip(int Current_I, int Current_J, int DeksCount, bool isHorizontal = true)
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
                    if (!string.IsNullOrEmpty(field[i, j])) return false;

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
        public int CountingDecks(CellIndex indexes, ref int firtShipDeck)
        {
            bool Direction = DeterminingTheDirection(indexes.I_index, indexes.J_index);
            int GeneralDeksCountInShip = 1;
            firtShipDeck = 0;

            for (int k = 1; k <= 4; k++)
            {
                int firstIndex = indexes.I_index;
                int secondIndex = indexes.J_index;

                _ = Direction is true ?
                 secondIndex = indexes.J_index + k :
                 firstIndex = indexes.I_index + k;

                if (secondIndex > 10 || firstIndex > 10) break;

                if (String.IsNullOrEmpty(field[firstIndex, secondIndex]) ||
                   (field[firstIndex, secondIndex] == MissedMark))
                    break;

                GeneralDeksCountInShip++;
            }

            for (int k = -1; k >= -4; k--)
            {
                int firstIndex = indexes.I_index;
                int secondIndex = indexes.J_index;

                _ = Direction is true ?
                 secondIndex = indexes.J_index + k :
                 firstIndex = indexes.I_index + k;

                if (secondIndex < 0 || firstIndex < 0) break;

                if (String.IsNullOrEmpty(field[firstIndex, secondIndex]) ||
                   (field[firstIndex, secondIndex] == MissedMark))
                    break;

                firtShipDeck++;
                GeneralDeksCountInShip++;
            }


            return GeneralDeksCountInShip;

        }
        public bool DeterminingTheDirection(int firstIndex, int secondIndex)
        {
            bool isHorizontal = true;

            for (int i = firstIndex; i < 11; i++)
            {
                for (int j = secondIndex; j < 11; j++)
                {
                    if (field[i, j - 1] is ShipsMark || (j + 1 <= 10 && field[i, j + 1] is ShipsMark)) return true;
                    if (field[i, j - 1] is KilledMark || (j + 1 <= 10 && field[i, j + 1] is KilledMark)) return true;

                    if (field[i - 1, j] is KilledMark || (i + 1 <= 10 && field[i + 1, j] is KilledMark)) return false;
                    if (field[i - 1, j] is ShipsMark || (i + 1 <= 10 && field[i + 1, j] is ShipsMark)) return false;
                }
            }

            return isHorizontal;
        }
        public string[,] FieldAutoGeneration()
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

                    bool canPutShip = CanPutShip(firstIndex, secondIndex, DeksCount, isHorizontal);

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
                                field[firstIndex, secondIndex + k] = ShipsMark;

                            if (isHorizontal is false)
                                field[firstIndex + k, secondIndex] = ShipsMark;
                        }

                    }
                }

                ShipCount++;
                DeksCount--;
            }
            return field;
        }
        public string[,] CleanField()
        {
            return field = new string[11, 11];
        }
        public bool CanMakeDamage(int Cell)
        {
            CellIndex Indexes = CellsConverter.ConverCellsToIndexes(Cell);
            if (field[Indexes.I_index, Indexes.J_index] is KilledMark ||
                field[Indexes.I_index, Indexes.J_index] is MissedMark)
            {
                return false;
            }
            return true;
        }
        public string[,] ComputerAttack()
        {
            bool isComputerField = true;
            CellIndex indexes = new CellIndex();
            CountOfAttackInOneDirection = 0;
            int AttackInOnePoint = 0;

            while (isComputerField != false)
            {
                if (isHintButNotKilled is false)
                {
                    indexes = SearchRandomCell();
                    fixed_I = indexes.I_index;
                    fixed_J = indexes.J_index;
                }
                if (isHintButNotKilled is true)
                {
                    indexes = ChangeCellNumberToNextAttack(indexes);
                    if (field[indexes.I_index, indexes.J_index] is KilledMark ||
                        field[indexes.I_index, indexes.J_index] is MissedMark)
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

                isMissed = IsMissed(indexes);

                if (isMissed is true)
                {
                    MarkTheShip(indexes, MissedMark);
                    isComputerField = false;
                    if (isHintButNotKilled is false)
                        isHintButNotKilled = false;
                    if (CountOfAttackInOneDirection is 0)
                        isHorizontal = !isHorizontal;
                    if (CountOfAttackInOneDirection != 0)
                        isPositiveDirection = !isPositiveDirection;
                    return field;
                }

                MarkTheShip(indexes, KilledMark);

                isKilled = IsKilled(indexes);

                if (isKilled is true)
                {
                    ShipsFuneral(indexes);
                    totalShipsCount--;
                    isHintButNotKilled = false;
                    if (totalShipsCount is 0) return field;
                    continue;
                }
                isHintButNotKilled = true;
                CountOfAttackInOneDirection++;
            }
            return field;
        }

        #region Private Methods
        private CellIndex SearchRandomCell()
        {
            bool canDamage = false;
            Random random = new Random();
            CellIndex index = new CellIndex();

            while (canDamage != true)
            {
                index.I_index = random.Next(1, 11);
                index.J_index = random.Next(1, 11);

                int Cell = CellsConverter.ConvertIndexesToCell(index);

                canDamage = CanMakeDamage(Cell);
            }
            return index;
        }
        private void ShipsFuneral(CellIndex Indexes)
        {
            int FirstShipsDeck = 0;

            bool direction = DeterminingTheDirection(Indexes.I_index, Indexes.J_index);
            int DecksCount = CountingDecks(Indexes, ref FirstShipsDeck);

            switch (DecksCount)
            {
                default: break;
                case 1: OneDeckShip--; break;
                case 2: TwoDeckShip--; break;
                case 3: ThrieDeckShip--; break;
                case 4: FourDeckShip--; break;
            }

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
        private bool IsKilled(CellIndex Indexes)
        {
            int FirstShipsDeck = 0;
            bool direction = DeterminingTheDirection(Indexes.I_index, Indexes.J_index);

            int DecksCount = CountingDecks(Indexes, ref FirstShipsDeck);

            for (int i = 0; i < DecksCount; i++)
            {
                int I = Indexes.I_index;
                int J = Indexes.J_index;

                _ = direction is true ?
                    J = Indexes.J_index - FirstShipsDeck + i :
                    I = Indexes.I_index - FirstShipsDeck + i;

                if (field[I, J] is KilledMark)
                    continue;

                return false;

            }
            return true;
        }
        private bool IsMissed(CellIndex indexes)
        {
            if (field[indexes.I_index, indexes.J_index] is ShipsMark)
                return false;
            return true;
        }
        private void MarkTheShip(CellIndex Indexes, string Mark)
        {
            field[Indexes.I_index, Indexes.J_index] = Mark;
        }
        private CellIndex ChangeAttackDirection(CellIndex indexes, CellIndex IndexToAdd)
        {
        ChangeDirection:
            if (isHorizontal is true && isPositiveDirection is true)
            {
                if (IndexToAdd.J_index + 1 <= 10 && IndexToAdd.J_index >= 1)
                {
                    indexes.J_index = IndexToAdd.J_index + 1;
                    indexes.I_index = IndexToAdd.I_index;
                    isPositiveDirection = true;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (isHorizontal is true && isPositiveDirection is false)
            {
                if (IndexToAdd.J_index - 1 >= 1 && IndexToAdd.J_index <= 10)
                {
                    indexes.J_index = IndexToAdd.J_index - 1;
                    indexes.I_index = IndexToAdd.I_index;
                    isPositiveDirection = false;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto ChangeDirection;
            }

            if (isHorizontal is false && isPositiveDirection is true)
            {
                if (IndexToAdd.I_index + 1 <= 10 && IndexToAdd.I_index >= 1)
                {
                    indexes.I_index = IndexToAdd.I_index + 1;
                    indexes.J_index = IndexToAdd.J_index;
                    isPositiveDirection = true;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
            }
            if (isHorizontal is false && isPositiveDirection is false)
            {
                if (IndexToAdd.I_index - 1 >= 1 && IndexToAdd.I_index <= 10)
                {
                    indexes.I_index = IndexToAdd.I_index - 1;
                    indexes.J_index = IndexToAdd.J_index;
                    return indexes;
                }
                isPositiveDirection = !isPositiveDirection;
                goto ChangeDirection;
            }
            return indexes;
        }
        private CellIndex ChangeCellNumberToNextAttack(CellIndex NewIndex)
        {
            CellIndex PreviousIndex = new CellIndex();
            PreviousIndex.I_index = fixed_I;
            PreviousIndex.J_index = fixed_J;
            if (CountOfAttackInOneDirection >= 1)
            {
                ChangeAttackDirection(NewIndex, NewIndex);
                return NewIndex;
            }
            ChangeAttackDirection(NewIndex, PreviousIndex);




            return NewIndex;
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