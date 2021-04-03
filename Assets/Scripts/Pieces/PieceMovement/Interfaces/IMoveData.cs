using UnityEngine;

public interface IMoveData
{
    IBoardPosition InitialBoardPosition { get; }
    IBoardPosition DestinationBoardPosition { get; }
    PieceType DisplacedPiece { get; }
    PieceType MovedPiece { get; }
    Piece MovedPieceComponent { get; }
}