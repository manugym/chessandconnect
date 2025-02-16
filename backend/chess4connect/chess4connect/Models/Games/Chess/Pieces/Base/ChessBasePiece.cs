﻿using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces.Base;

public abstract class ChessBasePiece
{
    public int Id { get; set; }
    public ChessPieceColor Color { get; set; }
    public Point Position { get; set; }
    public PieceType PieceType { get; set; }
    public List<Point> BasicMovements { get; set; }

    public ChessBasePiece(int id, PieceType pieceType, ChessPieceColor color, Point position)
    {
        PieceType = pieceType;
        Id = id;
        Color = color;
        Position = position;

        GetBasicMovements();
    }

    protected abstract void GetBasicMovements();
    

}
