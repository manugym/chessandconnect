﻿using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces
{
    public class Pawn : ChessBasePiece
    {
        public Pawn(int id, bool color, Point position) : base(id, PieceType.PAWN, color, position) { }
        public bool FirstMove { get; set; } = true;

        protected override void GetBasicMovements()
        {
            int direction = Player1Piece ? -1 : 1;

            BasicMovements = new List<Point>()
            {
                new Point(direction, 0),
                new Point(direction, 1),
                new Point(direction, -1),
            };
            if (FirstMove)
            {
                BasicMovements.Add(new Point(2 * direction,0));
            }

        }
    }
}

