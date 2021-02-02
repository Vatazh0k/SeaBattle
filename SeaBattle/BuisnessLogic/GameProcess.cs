using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.BuisnessLogic
{
    public static class GameProcess
    {
        public static bool DamageCreating(string[,] Field, int i, int j)
        {
            if (String.IsNullOrEmpty(Field[i, j]))
            {
                
                return true;
            }
            return false;


           
        }
    }
}
