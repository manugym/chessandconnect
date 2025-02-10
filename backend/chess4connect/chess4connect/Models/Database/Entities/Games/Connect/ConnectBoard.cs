﻿using chess4connect.Models.Database.Entities.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Database.Entities.Games.Connect;

public class ConnectBoard
{
    public static int ROWS = 6;
    public static int COLUMNS = 7;

    public Piece[,] Board { get; set; } = new Piece[ROWS, COLUMNS];
}


