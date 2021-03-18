using Zenject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardState : IBoardState
{
    public ITile[,] Board { get; private set; }

    public BoardState(IBoardGenerator boardGenerator)
    {
        Board = boardGenerator.GenerateBoard();
    }

    public ITile GetTileAt(IBoardPosition boardPosition) =>
        Board[boardPosition.X, boardPosition.Y];

}
