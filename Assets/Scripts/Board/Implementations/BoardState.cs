using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardState : IBoardState
{
    private ITile[,] _board;
    private ITile[,] _mirroredBoard;

    public BoardState(IBoardGenerator boardGenerator)
    {
        _board = boardGenerator.GenerateBoard();
        _mirroredBoard = boardGenerator.RotateBoard(_board);
    }

    public ITile GetTileAt(IBoardPosition boardPosition) =>
        _board[boardPosition.X, boardPosition.Y];

    public ITile GetMirroredTileAt(IBoardPosition boardPosition) =>
        _mirroredBoard[boardPosition.X, boardPosition.Y];
}
