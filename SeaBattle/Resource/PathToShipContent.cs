﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Resource
{
    public static class PathToShipContent
    {
        public static Dictionary<int, string> HorizontalShips { get; } = new Dictionary<int, string>()
        {
            { 1, "/SeaBattle;component/Resource/Images/ShipsForField/OneDeckShip.png"},
            { 2,"/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_DoubleDeckShip.png" },
            { 3, "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_DoubleDeckShip.png"},
            { 4, "/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_ThrieDeckShip.png"},
            { 5, "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_ThrieDeckShip.png"},
            { 6, "/SeaBattle;component/Resource/Images/ShipsForField/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_FourDeckShip.png"},
            { 8, "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_FourDeckShip.png"},
            { 9, "/SeaBattle;component/Resource/Images/ShipsForField/ThrieDeck_FourDeckShip.png"},
            { 10, "/SeaBattle;component/Resource/Images/ShipsForField/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> VerticalShips { get; } = new Dictionary<int, string>()
        {
            { 1, "/SeaBattle;component/Resource/Images/VerticalShipsForField/OneDeckShip.png"},
            { 2, "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_DoubleDeckShip.png"},
            { 3, "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_DoubleDeckShip.png"},
            { 4,  "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_ThrieDeckShip.png"},
            { 5, "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_ThrieDeckShip.png"},
            { 6, "/SeaBattle;component/Resource/Images/VerticalShipsForField/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_FourDeckShip.png"},
            { 8, "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_FourDeckShip.png"},
            { 9, "/SeaBattle;component/Resource/Images/VerticalShipsForField/ThrieDeck_FourDeckShip.png"},
            { 10, "/SeaBattle;component/Resource/Images/VerticalShipsForField/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> Vertical_Dead_Ships { get; } = new Dictionary<int, string>()
        {
            { 1, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/OneDeckShip.png"},
            { 2, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_DoubleDeckShip.png"},
            { 3, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_DoubleDeckShip.png"},
            { 4, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_ThrieDeckShip.png"},
            { 5, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_ThrieDeckShip.png"},
            { 6, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_FourDeckShip.png"},
            { 8, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_FourDeckShip.png"},
            { 9,  "/SeaBattle;component/Resource/Images/Vertical_DeadShip/ThrieDeck_FourDeckShip.png"},
            { 10, "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FourDeck_FourDeckShip.png"},
        };
        public static Dictionary<int, string> Horizontal_Dead_Ships { get; } = new Dictionary<int, string>()
        {
            { 1, "/SeaBattle;component/Resource/Images/DeadShip/OneDeckShip.png"},
            { 2,  "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_DoubleDeckShip.png"},
            { 3,  "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_DoubleDeckShip.png"},
            { 4, "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_ThrieDeckShip.png"},
            { 5,  "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_ThrieDeckShip.png"},
            { 6, "/SeaBattle;component/Resource/Images/DeadShip/ThirdDeck_ThrieDeckShip.png"},
            { 7, "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_FourDeckShip.png"},
            { 8, "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_FourDeckShip.png"},
            { 9, "/SeaBattle;component/Resource/Images/DeadShip/ThrieDeck_FourDeckShip.png"},
            { 10, "/SeaBattle;component/Resource/Images/DeadShip/FourDeck_FourDeckShip.png"},
        };
        #region Path
        public static string EmptyCell { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/EmptyCell.png";
        public static string KilledShip { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/Vertical_KilledShip.png";
        public static string MissedMark { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/MissedMark.png";
        #region Horizontal
        public static string OneDeckShip { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/OneDeckShip.png";
        public static string TwoDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_DoubleDeckShip.png";
        public static string TwoDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_DoubleDeckShip.png";
        public static string ThrieDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_ThrieDeckShip.png";
        public static string ThrieDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_ThrieDeckShip.png";
        public static string ThrieDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/ThirdDeck_ThrieDeckShip.png";
        public static string FourDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/FirstDeck_FourDeckShip.png";
        public static string FourDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/SecondDeck_FourDeckShip.png";
        public static string FourDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/ThrieDeck_FourDeckShip.png";
        public static string FourDeckShip_FourDeck { get; } = "/SeaBattle;component/Resource/Images/ShipsForField/FourDeck_FourDeckShip.png";
        #endregion
        #region Vertical
        public static string Vertical_OneDeckShip { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/OneDeckShip.png";
        public static string Vertical_TwoDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_DoubleDeckShip.png";
        public static string Vertical_TwoDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_DoubleDeckShip.png";
        public static string Vertical_ThrieDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_ThrieDeckShip.png";
        public static string Vertical_ThrieDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_ThrieDeckShip.png";
        public static string Vertical_ThrieDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/ThirdDeck_ThrieDeckShip.png";
        public static string Vertical_FourDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/FirstDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/SecondDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/ThrieDeck_FourDeckShip.png";
        public static string Vertical_FourDeckShip_FourDeck { get; } = "/SeaBattle;component/Resource/Images/VerticalShipsForField/FourDeck_FourDeckShip.png";
        #endregion
        #region HorizontalDead
        public static string Dead_OneDeckShip { get; } = "/SeaBattle;component/Resource/Images/DeadShip/OneDeckShip.png";
        public static string Dead_TwoDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_DoubleDeckShip.png";
        public static string Dead_TwoDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_DoubleDeckShip.png";
        public static string Dead_ThrieDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_ThrieDeckShip.png";
        public static string Dead_ThrieDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_ThrieDeckShip.png";
        public static string Dead_ThrieDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/ThirdDeck_ThrieDeckShip.png";
        public static string Dead_FourDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/FirstDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/SecondDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/ThrieDeck_FourDeckShip.png";
        public static string Dead_FourDeckShip_FourDeck { get; } = "/SeaBattle;component/Resource/Images/DeadShip/FourDeck_FourDeckShip.png";
        #endregion
        #region VerticalDead
        public static string Vertical_Dead_OneDeckShip { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/OneDeckShip.png";
        public static string Vertical_Dead_TwoDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_DoubleDeckShip.png";
        public static string Vertical_Dead_TwoDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_DoubleDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_ThrieDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/ThirdDeck_ThrieDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_FirstDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FirstDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_SecondDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/SecondDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_ThirdDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/ThrieDeck_FourDeckShip.png";
        public static string Vertical_Dead_FourDeckShip_FourDeck { get; } = "/SeaBattle;component/Resource/Images/Vertical_DeadShip/FourDeck_FourDeckShip.png";
        #endregion
        #endregion
    }
}
