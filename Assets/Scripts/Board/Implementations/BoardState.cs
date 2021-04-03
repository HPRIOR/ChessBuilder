using System;

public class BoardState : IBoardState, ICloneable
{
    public ITile[,] Board { get; }
    public ITile[,] MirroredBoard { get; }

    public BoardState(ITile[,] board)
    {
        Board = board;
        MirroredBoard = RotateBoard(this.Board);
    }

    private BoardState(ITile[,] board, ITile[,] mirroredBoard)
    {
        Board = board;
        MirroredBoard = mirroredBoard;
       
    }

    public ITile GetTileAt(IBoardPosition boardPosition) =>
        Board[boardPosition.X, boardPosition.Y];

    public ITile GetMirroredTileAt(IBoardPosition boardPosition) =>
        MirroredBoard[boardPosition.X, boardPosition.Y];

    public object Clone() {
        var newBoard = new Tile[8, 8];
        ITile[,] newMirroredBoard; 
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                newBoard[i, j] = (Tile)Board[i, j].Clone();
            }
        newMirroredBoard = RotateBoard(newBoard);
        return new BoardState(newBoard, newMirroredBoard);
    }
    
    private ITile[,] RotateBoard(ITile[,] board)
    {
        var result = new ITile[8, 8];

        for (int iEnd = 7, iStart = 0; iStart < 8; iStart++, iEnd--)
            for (int jEnd = 7, jStart = 0; jStart < 8; jStart++, jEnd--)
                result[iStart, jStart] = board[iEnd, jEnd];
        return result;
    }

}