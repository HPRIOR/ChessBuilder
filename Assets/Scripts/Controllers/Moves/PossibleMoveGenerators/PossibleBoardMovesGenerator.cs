using System;
using System.Linq;
using Assets.Scripts.Controllers.Moves.PossibleMoveHelpers;
using System.Collections.Generic;

public class PossibleBoardMovesGenerator : IPossibleMovesGenerator
{
    private IPieceMoveGeneratorFactory _pieceMoveGeneratorFactory;

    public PossibleBoardMovesGenerator(IPieceMoveGeneratorFactory pieceMoveGeneratorFactory)
    {
        _pieceMoveGeneratorFactory = pieceMoveGeneratorFactory;
    }

    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState)
    {
        var result = new Dictionary<IBoardPosition, HashSet<IBoardPosition>>();
        var board = boardState.Board;
        foreach (var tile in board)
            if (tile.CurrentPiece.Type != PieceType.NullPiece)
            {
                var currentPiece = tile.CurrentPiece;
                var boardPos = tile.BoardPosition;
                var possibleMoves = _pieceMoveGeneratorFactory.GetPossibleMoveGenerator(currentPiece.Type).GetPossiblePieceMoves(boardPos, boardState);
                result.Add(boardPos, new HashSet<IBoardPosition>(possibleMoves));
            }
        return result;
    }

    private IDictionary<IBoardPosition, HashSet<IBoardPosition>> IntersectOnCheck(
        IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves,
        ITile[,] board,
        IBoardPosition checkedKing, IBoardPosition checkingPiece) 
    {
        var possibleNonKingMoves = new HashSet<IBoardPosition>(ScanPositionGenerator.GetPositionsBetween(checkedKing, checkingPiece));
        return possibleMoves.ToList().Select(t =>
        {
            var bp = t.Key;
            var set = t.Value;
            var currentPiece = board[bp.X, bp.Y].CurrentPiece;
            if (currentPiece.Type != PieceType.BlackKing || currentPiece.Type != PieceType.WhiteKing)
                return (bp, set.Intersect(possibleNonKingMoves) as HashSet<IBoardPosition>);
            else return (bp, set);
        }).ToDictionary(t => t.bp, t => t.Item2);

    }
}
