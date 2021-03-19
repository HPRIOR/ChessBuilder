using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossibleBoardMovesGenerator : IPossibleBoardMovesGenerator
{
    private readonly IBoardState _boardState;
    public  IDictionary<GameObject, HashSet<IBoardPosition>> PossibleMoves { get; private set; }

    public PossibleBoardMovesGenerator(IBoardState boardState)
    {
        _boardState = boardState;
    }

    public void GeneratePossibleMoves()
    {
        var activeGameObjects = GetActiveGameObject();
        var activePieceComponents = activeGameObjects.ToList().Select(go => go.GetComponent<Piece>());
        PossibleMoves = activeGameObjects
            .ToDictionary(
                go => go, 
                go => new HashSet<IBoardPosition>(go.GetComponent<Piece>().GetPossiblePieceMoves())
                );
    }

    private IEnumerable<GameObject> GetActiveGameObject()
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
