using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Changes: tile will contain reference to piece type not game object. Get possible moves will refer to a different
 * mechanism that is give through the piece type. e.g. some factory which produces GetPieceTypeMove class
 * board state passed as argument instead of through constructor, produces dictionary instead of being a member of the 
 * class
 */
public class PossibleBoardMovesGenerator : IPossibleBoardMovesGenerator
{
    private readonly IBoardState _boardState;
    public IDictionary<GameObject, HashSet<IBoardPosition>> PossibleMoves { get; private set; }

    public PossibleBoardMovesGenerator(IBoardState boardState)
    {
        _boardState = boardState;
    }

    public void GeneratePossibleMoves()
    {
        var activeGameObjects = GetActiveGameObjects();
        var activePieceComponents = activeGameObjects.ToList().Select(go => go.GetComponent<Piece>());
        PossibleMoves = activeGameObjects
            .ToDictionary(
                gameObject => gameObject,
                gameObject => new HashSet<IBoardPosition>(gameObject.GetComponent<Piece>().GetPossibleMoves())
                );
    }

    private IEnumerable<GameObject> GetActiveGameObjects()
    {
        var activeGameObjects = new List<GameObject>();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                var tile = _boardState.GetTileAt(new BoardPosition(i, j));
                if (tile.CurrentPiece != null)
                    activeGameObjects.Add(tile.CurrentPiece);
            }
        return activeGameObjects;
    }
}