using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Model
{
    class Field
    {
        public string[,] field { get; set; } = new string[11, 11]; //умовна позиція корабля

        public int OneDeckShipCount { get; set; } = 4;
        public int TwoDeckShipCount { get; set; } = 3;
        public int ThrieDeckShipCount { get; set; } = 2;
        public int FourDeckShipCount { get; set; } = 1;
    }
}
