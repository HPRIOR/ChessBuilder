using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveGeneratorFactory : IPieceMoveGeneratorFactory
{
    private IBoardState _boardState;

    public PieceMoveGeneratorFactory(IBoardState boardState)
    {
        _boardState = boardState;
    }

    public IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType) =>
        pieceType switch
        {
            var pawn when pawn == PieceType.BlackPawn || pawn == PieceType.WhitePawn => new PossiblePawnMoves(_boardState),
            //var bishop when bishop == PieceType.BlackBishop || bishop == PieceType.WhiteBishop => throw new System.NotImplementedException(),
            //var knight when knight == PieceType.BlackKnight || knight == PieceType.WhiteKnight => throw new System.NotImplementedException(),
            var rook when rook == PieceType.BlackRook || rook == PieceType.WhiteRook => new PossibleRookMoves(_boardState),
            //var king when king == PieceType.BlackKing || king == PieceType.WhiteKnight => throw new System.NotImplementedException(),
            //var queen when queen == PieceType.BlackQueen || queen == PieceType.WhiteQueen => throw new System.NotImplementedException(),
            _ => new NullPossibleMoveGenerator()
        };
    
}
