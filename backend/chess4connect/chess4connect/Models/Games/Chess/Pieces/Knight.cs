﻿using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Knight : BasePiece
    {
        public Knight(int id, Chess.Color color, Point position) : base(id, color, position) { }

        protected List<Point> BasicMovements()
        {
            return new List<Point>
            {
            new Point(2, 1),  new Point(2, -1), new Point(-2, 1), new Point(-2, -1),
            new Point(1, 2),  new Point(1, -2), new Point(-1, 2), new Point(-1, -2)
            };
        }

    }
}
