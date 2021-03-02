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
        public bool ChangeShipsDirection(string[,] field, int cell, bool direction)
        {
            CellIndex indexes = CellsConverter.ConverCellsToIndexes(cell);
            int CurrentDeck = 0;

            int DecksInShipCount = CountingDecks(field, indexes.I_index, indexes.J_index, ref CurrentDeck, direction);

            int FirstDeckOfHorizontalShip = indexes.J_index - CurrentDeck;
            int FirstDeckOfVerticalShip = indexes.I_index - CurrentDeck;

            if (direction is true)
                for (int i = 0; i < DecksInShipCount; i++)
                    field[indexes.I_index, FirstDeckOfHorizontalShip + i] = null;

            if (direction is false)
                for (int i = 0; i < DecksInShipCount; i++)
                    field[FirstDeckOfVerticalShip + i, indexes.J_index] = null;

            bool canPutShip = CanPutShip
            (field, indexes.I_index, indexes.J_index, DecksInShipCount, !direction);

            if (canPutShip is false)
            {
                if (direction is true)
                    for (int i = 0; i < DecksInShipCount; i++)
                        field[indexes.I_index, FirstDeckOfHorizontalShip + i] = ShipsMark;

                if (direction is false)
                    for (int i = 0; i < DecksInShipCount; i++)
                        field[FirstDeckOfVerticalShip + i, indexes.J_index] = ShipsMark;
                return false;
            }
            if (canPutShip is true)
            {
                if (Ships[CellNumber].isHorizontal is true)
                {
                    int Cell = CellNumber - CurrentDeck;
                    for (int i = 0; i < DecksInShipCount; i++)
                        Ships[Cell + i] = new Ship();

                    ShowVerticalShips(DecksInShipCount, CellNumber);
                    for (int i = 0; i < DecksInShipCount; i++)
                        UsersField.field[indexes.I_index + i, indexes.J_index] = ShipsMark;

                    return true;
                }
                if (Ships[CellNumber].isHorizontal is false)
                {
                    int Cell = CellNumber;
                    switch (CurrentDeck)
                    {
                        default: break;
                        case 0: Cell -= 11; break;
                        case 1: Cell -= 22; break;
                        case 2: Cell -= 33; break;
                        case 3: Cell -= 44; break;

                    }

                    for (int i = 0; i < DecksInShipCount; i++)
                        Ships[Cell += 11] = new Ship();

                    ShowHorizontalShips(DecksInShipCount, CellNumber);
                    for (int i = 0; i < DecksInShipCount; i++)
                        UsersField.field[indexes.I_index, indexes.J_index + i] = ShipsMark;

                    return true;
                }
            }

            return true;
        }
        public int CountingDecks(string[,] Field, int i, int j, ref int firtShipDeck, bool isHorizontal = true)
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
        public bool DeterminingTheDirection(int IndexOfFirstDeck, int secondIndex, string[,] Field)
        {
            bool isHorizontal = true;
            
            for (int i = IndexOfFirstDeck; i < 11; i++)
            {
                for (int j = secondIndex; j < 11; j++)
                {
                    if (j + 1 <= 10 && Field[i, j + 1] is ShipsMark) return true;
         
                    if (i + 1 <= 10 && Field[i + 1, j] is ShipsMark) return false;
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
                                Field[firstIndex, secondIndex + k ] = ShipsMark;

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
        public void CleanField(string[,] Field)
        {
            Field = new string[11, 11];
        }
        public string[,] Damaging(string[,] Field, CellIndex Indexes)
        {

            return Field;
        }

        #region Private Methods
      
        #endregion
    }
}
