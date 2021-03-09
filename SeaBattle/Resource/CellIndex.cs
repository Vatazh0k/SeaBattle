using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Resource
{
    public class CellIndex
    {
        public int I_index { get; set; }
        public int J_index { get; set; }
        public CellIndex(int i, int j)
        {
            I_index = i;
            J_index = j;
        }
        public CellIndex()
        {

        }
    }
}
