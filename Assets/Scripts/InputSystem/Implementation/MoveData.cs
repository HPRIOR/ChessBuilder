using UnityEngine;

public class MoveData : IMoveData
{
    private static GameController _gameController =
        GameObject
        .FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();

    public IBoardPosition InitialBoardPosition { get; }
    public IBoardPosition DestinationBoardPosition { get; }
    public GameObject DisplacedPiece { get; }
    public GameObject MovedPiece { get; }
    public Piece MovedPieceComponent { get; }

    public MoveData(GameObject movedPiece, IBoardPosition destination)
    {
        MovedPiece = movedPiece;
        MovedPieceComponent = movedPiece.GetComponent<Piece>();
        InitialBoardPosition = MovedPieceComponent.BoardPosition;

        DisplacedPiece = _gameController.GetTileAt(destination).CurrentPiece?.gameObject;
        DestinationBoardPosition = destination;
    }
}