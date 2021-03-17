using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractPossibleMoveGenerator : IPossibleMoveGenerator
{
    protected IBoardState boardState;
    public AbstractPossibleMoveGenerator(IBoardState boardState)
    {
        this.boardState = boardState;
    }

    protected ITile GetTileAt(int x, int y) => 
        boardState.GetTileAt(new BoardPosition(x, y));

    public abstract IEnumerable<IBoardPosition> PossibleBoardMoves(GameObject piece);
}
