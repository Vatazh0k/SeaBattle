using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaBattle.Model
{
    public class Ship
    {
        public bool isDead { get; set; } = false;
        public bool isOnField { get; set; } = false;
        public bool isHorizontal { get; set; } = true;
        public Image Content { get; set; }
        public Thickness Border { get; set; } = new Thickness(0.5);
    }
}
