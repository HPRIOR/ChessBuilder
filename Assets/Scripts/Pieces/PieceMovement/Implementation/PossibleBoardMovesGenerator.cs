using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleBoardMovesGenerator : IPossibleMovesGenerator
{
    IPieceMoveGeneratorFactory _pieceMoveGenertorFactory;

    PossibleBoardMovesGenerator(IPieceMoveGeneratorFactory pieceMoveGeneratorFactory)
    {
        _pieceMoveGenertorFactory = pieceMoveGeneratorFactory;
    }
    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState)
    {
        var result = new Dictionary<IBoardPosition, HashSet<IBoardPosition>>();
        var board = boardState.Board;
        foreach(var tile in board)
        {
            if (tile.CurrentPiece != PieceType.NullPiece)
            {
                var currentPiece = tile.CurrentPiece;
                var boardPos = tile.BoardPosition;
                var possibleMoves = _pieceMoveGenertorFactory.GetPossibleMoveGenerator(currentPiece).GetPossiblePieceMoves(boardPos, boardState);
                result.Add(boardPos, new HashSet<IBoardPosition>(possibleMoves));
            }
        }
        return result;
    }
}