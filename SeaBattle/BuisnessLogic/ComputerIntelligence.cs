using SeaBattle.Infrastructure.Converters;
using SeaBattle.Resource;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.BuisnessLogic
{
    class ComputerIntelligence
    {
        #region Private Data 
        private Field UserField;
        private Field ComputerField;
        private int CountOfAttackInOneDirection = 0;
        int AttackInOnePoint = 0;
        private int totalShipsCount = 10;
        private bool isHorizontal = true;
        private bool isPositiveDirection = true;
        private bool isKilled = false;
        private bool isMissed = true;
        private bool isHintButNotKilled = false;
        private int fixed_I;
        private int fixed_J;

        private const string ShipsMark = "O";
        private const string KilledMark = "X";
        private const string MissedMark = " ";
        #endregion

        public ComputerIntelligence(Field UserField, Field ComputerField)
        {
            this.UserField = UserField;
            this.ComputerField = ComputerField;
        }

        //public string[,] ComputerAttack()
        //{
        //    bool isComputerMove = true;
        //    CellIndex indexes = new CellIndex();
        //    CountOfAttackInOneDirection = 0;
        //    AttackInOnePoint = 0;

        //    while (isComputerMove != false)
        //    {
        //        if (isHintButNotKilled is false)
        //        {
        //            indexes = SearchRandomCell();
        //            indexes.I_index = 4; indexes.J_index = 10; isHorizontal = false; fixed_I = 4; fixed_J = 10; isPositiveDirection = false;//
        //        }
        //        if (isHintButNotKilled is true)
        //        {
        //            indexes = ChangeDirection(indexes);
        //        }

        //        isMissed = IsMissed(indexes);

        //        if (isMissed is true)
        //        {
        //            AssignShipsMark(indexes, MissedMark);
        //            isComputerMove = false;
        //            if (CountOfAttackInOneDirection is 0)
        //                isHorizontal = !isHorizontal;
        //            if (CountOfAttackInOneDirection != 0)
        //                isPositiveDirection = !isPositiveDirection;
        //            return UserField.field;
        //        }

        //        AssignShipsMark(indexes, KilledMark);

        //        isKilled = IsKilled(indexes);

        //        if (isKilled is true)
        //        {
        //            ShipsFuneral(indexes);
        //            if (totalShipsCount is 0) 
        //            return UserField.field;
        //            continue;
        //        }
        //        isHintButNotKilled = true;
        //        CountOfAttackInOneDirection++;
        //    }
        //    return UserField.field;
        //}
        //public string[,] FieldAutoGeneration()
        //{
        //    return ComputerField.FieldAutoGeneration();
        //}

        //#region Private Methods
        //private CellIndex SearchRandomCell()
        //{
        //    bool canDamage = false;
        //    Random random = new Random();
        //    CellIndex index = new CellIndex();

        //    while (canDamage != true)
        //    {
        //        index.I_index = random.Next(1, 11);
        //        index.J_index = random.Next(1, 11);

        //        int Cell = CellsConverter.ConvertIndexesToCell(index);

        //        canDamage = UserField.CanMakeDamage(Cell);
        //    }

        //    fixed_I = index.I_index;
        //    fixed_J = index.J_index;

        //    return index;
        //}
        //private CellIndex CreateNewCellToAttack(CellIndex indexes, CellIndex IndexToAdd)
        //{
        //ChangeDirection:
        //    if (isHorizontal is true && isPositiveDirection is true)
        //    {
        //        if (IndexToAdd.J_index + 1 <= 10 && IndexToAdd.J_index >= 1)
        //        {
        //            indexes.J_index = IndexToAdd.J_index + 1;
        //            indexes.I_index = IndexToAdd.I_index;
        //            isPositiveDirection = true;
        //            return indexes;
        //        }
        //        isPositiveDirection = !isPositiveDirection;
        //    }
        //    if (isHorizontal is true && isPositiveDirection is false)
        //    {
        //        if (IndexToAdd.J_index - 1 >= 1 && IndexToAdd.J_index <= 10)
        //        {
        //            indexes.J_index = IndexToAdd.J_index - 1;
        //            indexes.I_index = IndexToAdd.I_index;
        //            isPositiveDirection = false;
        //            return indexes;
        //        }
        //        isPositiveDirection = !isPositiveDirection;
        //        goto ChangeDirection;
        //    }

        //    if (isHorizontal is false && isPositiveDirection is true)
        //    {
        //        if (IndexToAdd.I_index + 1 <= 10 && IndexToAdd.I_index >= 1)
        //        {
        //            indexes.I_index = IndexToAdd.I_index + 1;
        //            indexes.J_index = IndexToAdd.J_index;
        //            isPositiveDirection = true;
        //            return indexes;
        //        }
        //        isPositiveDirection = !isPositiveDirection;
        //    }
        //    if (isHorizontal is false && isPositiveDirection is false)
        //    {
        //        if (IndexToAdd.I_index - 1 >= 1 && IndexToAdd.I_index <= 10)
        //        {
        //            indexes.I_index = IndexToAdd.I_index - 1;
        //            indexes.J_index = IndexToAdd.J_index;
        //            return indexes;
        //        }
        //        isPositiveDirection = !isPositiveDirection;
        //        goto ChangeDirection;
        //    }
        //    return indexes;
        //}
        //private CellIndex ChangeDirection(CellIndex NewIndex)
        //{
        //    CellIndex PreviousIndex = new CellIndex();
        //    PreviousIndex.I_index = fixed_I;
        //    PreviousIndex.J_index = fixed_J;
        //    if (CountOfAttackInOneDirection >= 1)
        //    {
        //        CreateNewCellToAttack(NewIndex, NewIndex);
        //    }
        //    else
        //    {
        //        CreateNewCellToAttack(NewIndex, PreviousIndex);
        //    }

        //    if (UserField.field[NewIndex.I_index, NewIndex.J_index] is KilledMark ||
        //        UserField.field[NewIndex.I_index, NewIndex.J_index] is MissedMark)
        //    {
        //        isPositiveDirection = !isPositiveDirection;
        //        CountOfAttackInOneDirection = 0;
        //        if (AttackInOnePoint is 2)
        //        {
        //            AttackInOnePoint = 0;
        //            isHorizontal = !isHorizontal;
        //            ChangeDirection(NewIndex);
        //        }
        //        AttackInOnePoint++;
        //        ChangeDirection(NewIndex);

        //    }


        //    return NewIndex;
        //}
        //private bool IsMissed(CellIndex indexes)
        //{
        //    if (UserField.field[indexes.I_index, indexes.J_index] is ShipsMark)
        //        return false;
        //    return true;
        //}
        //private void ShipsFuneral(CellIndex Indexes)
        //{
        //    bool direction = UserField.DeterminingTheDirection(Indexes.I_index, Indexes.J_index);
        //    int DecksCount = UserField.CountingDecks(Indexes);
        //    int FirstShipsDeck = UserField.SearchFirstDeckOfTheShip(Indexes, DecksCount, direction);

        //    switch (DecksCount)
        //    {
        //        default: break;
        //        case 1: UserField.OneDeckShip--; break;
        //        case 2: UserField.TwoDeckShip--; break;
        //        case 3: UserField.ThrieDeckShip--; break;
        //        case 4: UserField.FourDeckShip--; break;
        //    }

        //    int y_Axis_Ships = Indexes.I_index + 1;
        //    int x_Axis_Ships = Indexes.J_index + DecksCount - FirstShipsDeck;

        //    int n = Indexes.I_index - 1;
        //    int m = Indexes.J_index - 1 - FirstShipsDeck;

        //    if (direction is false)
        //    {
        //        n = Indexes.I_index - 1 - FirstShipsDeck;
        //        m = Indexes.J_index - 1;

        //        y_Axis_Ships = Indexes.I_index + DecksCount - FirstShipsDeck;
        //        x_Axis_Ships = Indexes.J_index + 1;
        //    }
        //    for (int N = n; N <= y_Axis_Ships; N++)
        //    {
        //        for (int M = m; M <= x_Axis_Ships; M++)
        //        {
        //            if (N == 11 || N == -1) continue;
        //            if (M == 11 || M == -1) break;
        //            if (UserField.field[N, M] == KilledMark) continue;
        //            UserField.field[N, M] = MissedMark;
        //        }
        //    }
        //    totalShipsCount--;
        //    isHintButNotKilled = false;
        //}
        //private bool IsKilled(CellIndex Indexes)
        //{
        //    bool direction = UserField.DeterminingTheDirection(Indexes.I_index, Indexes.J_index);
        //    int DecksCount = UserField.CountingDecks(Indexes);
        //    int FirstShipsDeck = UserField.SearchFirstDeckOfTheShip(Indexes, DecksCount, direction);

        //    for (int i = 0; i < DecksCount; i++)
        //    {
        //        int I = Indexes.I_index;
        //        int J = Indexes.J_index;

        //        _ = direction is true ?
        //            J = Indexes.J_index - FirstShipsDeck + i :
        //            I = Indexes.I_index - FirstShipsDeck + i;

        //        if (UserField.field[I, J] is KilledMark)
        //            continue;

        //        return false;

        //    }
        //    return true;
        //}
        //private void AssignShipsMark(CellIndex Indexes, string Mark)
        //{
        //    UserField.field[Indexes.I_index, Indexes.J_index] = Mark;
        //}
        //#endregion
    }
}