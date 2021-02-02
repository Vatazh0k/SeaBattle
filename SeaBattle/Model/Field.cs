using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Model
{
    class Field
    {
        public string[,] ComputerField { get; set; } = new string[11, 11];
        public string[,] UserField { get; set; } = new string[11, 11];

    }
}
