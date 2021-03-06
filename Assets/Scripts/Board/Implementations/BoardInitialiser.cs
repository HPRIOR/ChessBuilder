using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInitialiser : IBoardGenerator
{
    public Tile[,] GenerateBoard()
    {
        Tile[,] board = new Tile[8, 8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = new Tile(
                    new BoardPosition(i, j),
                    PieceType.None
                    );
            }
        board[0, 0].CurrentPiece = PieceType.BlackKing;
        return board;
    }

    
}
