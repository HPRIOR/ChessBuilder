using UnityEngine;

public interface IMoveData
{
    IBoardPosition InitialBoardPosition { get; }
    IBoardPosition DestinationBoardPosition { get; }
    GameObject DisplacedPiece { get; }
    GameObject MovedPiece { get; }
    Piece MovedPieceComponent { get; }
}