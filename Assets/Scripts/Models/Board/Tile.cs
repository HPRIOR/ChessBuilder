﻿using Assets.Scripts.Models.Piece;

public class Tile : ITile
{
    public Piece CurrentPiece { get; set; }
    public IBoardPosition BoardPosition { get; set; }

    public Tile(IBoardPosition boardPosition, Piece currentPiece)
    {
        BoardPosition = boardPosition;
        CurrentPiece = currentPiece;
    }

    public Tile(BoardPosition boardPosition)
    {
        BoardPosition = boardPosition;
        CurrentPiece = new Piece(PieceType.NullPiece);
    }

    public override string ToString() => $"Tile at ({BoardPosition.X}, {BoardPosition.Y}) containing" +
        $" {CurrentPiece}";

    public object Clone() =>
        new Tile(BoardPosition, CurrentPiece);
}