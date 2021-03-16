using UnityEngine;
using Zenject;

public class MoveData : IMoveData
{
   
    public IBoardPosition InitialBoardPosition { get; private set; }
    public IBoardPosition DestinationBoardPosition { get;  private set;}
    public GameObject DisplacedPiece { get;  private set;}
    public GameObject MovedPiece { get;  private set;}
    public Piece MovedPieceComponent { get;  private set;}

    public MoveData(GameObject movedPiece, IBoardPosition destination, IBoardState boardState)
    {
        MovedPiece = movedPiece;
        MovedPieceComponent = movedPiece.GetComponent<Piece>();
        InitialBoardPosition = MovedPieceComponent.BoardPosition;

        DisplacedPiece = boardState.GetTileAt(destination).CurrentPiece?.gameObject;
        DestinationBoardPosition = destination;
    }

    public class Factory : PlaceholderFactory<GameObject, IBoardPosition, MoveData> { }
}