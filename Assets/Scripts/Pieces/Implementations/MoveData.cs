using UnityEngine;
using Zenject;

public class MoveData : IMoveData
{
   
    public IBoardPosition InitialBoardPosition { get; private set; }
    public IBoardPosition DestinationBoardPosition { get;  private set;}
    public GameObject DisplacedPiece { get;  private set;}
    public GameObject MovedPiece { get;  private set;}
    public Piece MovedPieceComponent { get;  private set;}
    private IBoardState _boardState;

    [Inject]
    public void Construct(GameObject movedPiece, IBoardPosition destination, IBoardState boardState)
    {
        _boardState = boardState;
        MovedPiece = movedPiece;
        MovedPieceComponent = movedPiece.GetComponent<Piece>();
        InitialBoardPosition = MovedPieceComponent.BoardPosition;

        DisplacedPiece = _boardState.GetTileAt(destination).CurrentPiece?.gameObject;
        DestinationBoardPosition = destination;
    }

    public class Factory : PlaceholderFactory<GameObject, IBoardPosition, MoveData> { }
}