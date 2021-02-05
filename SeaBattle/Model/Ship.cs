using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeaBattle.Model
{
    public class Ship
    {
        public bool isDead { get; set; }
        public bool isOnField { get; set; }
        public Image Content { get; set; }
        public Thickness Border { get; set; }
    }
}
