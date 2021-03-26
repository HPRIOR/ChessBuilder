using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveGeneratorFactory : IPieceMoveGeneratorFactory
{
    private readonly IBoardEval _whiteBoardEval;
    private readonly IBoardEval _blackBoardEval;
    
    private readonly IBoardPositionTranslator _whitePositionTranslator;
    private readonly IBoardPositionTranslator _blackPositionTranslator;

    private readonly IBoardScanner _whiteBoardScanner;
    private readonly IBoardScanner _blackBoardScanner;

    public PieceMoveGeneratorFactory(
        IBoardScannerFactory boardScannerFactory,
        IPositionTranslatorFactory boardPositionTranslatorFactory, 
        IBoardEvalFactory boardEvalFactory)
    {
        _whiteBoardEval = boardEvalFactory.Create(PieceColour.White);
        _blackBoardEval = boardEvalFactory.Create(PieceColour.Black);

        _whiteBoardScanner = boardScannerFactory.Create(PieceColour.White);
        _blackBoardScanner = boardScannerFactory.Create(PieceColour.Black);

        _whitePositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.White);
        _blackPositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.Black);

    }

    public IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType) =>
        pieceType switch
        {
            var pawn when pawn == PieceType.BlackPawn || pawn == PieceType.WhitePawn => new PossiblePawnMoves(
                GetPositionTranslatorWith(PieceColourFrom(pieceType)), GetBoardEvalWith(PieceColourFrom(pieceType))),
            //var bishop when bishop == PieceType.BlackBishop || bishop == PieceType.WhiteBishop => throw new System.NotImplementedException(),
            //var knight when knight == PieceType.BlackKnight || knight == PieceType.WhiteKnight => throw new System.NotImplementedException(),
            var rook when rook == PieceType.BlackRook || rook == PieceType.WhiteRook => new PossibleRookMoves(
                GetBoardScannerWith(PieceColourFrom(pieceType)), 
                GetPositionTranslatorWith(PieceColourFrom(pieceType))
                ),
            //var king when king == PieceType.BlackKing || king == PieceType.WhiteKnight => throw new System.NotImplementedException(),
            //var queen when queen == PieceType.BlackQueen || queen == PieceType.WhiteQueen => throw new System.NotImplementedException(),
            _ => new NullPossibleMoveGenerator()
        };

    private PieceColour PieceColourFrom(PieceType pieceType) => 
        pieceType.ToString().StartsWith("White") 
        ? PieceColour.White 
        : PieceColour.Black;

    private IBoardScanner GetBoardScannerWith(PieceColour pieceColour) => 
        pieceColour == PieceColour.White 
        ? _whiteBoardScanner 
        : _blackBoardScanner;

    private IBoardPositionTranslator GetPositionTranslatorWith(PieceColour pieceColour) =>
        pieceColour == PieceColour.White
        ? _whitePositionTranslator
        : _blackPositionTranslator;

    private IBoardEval GetBoardEvalWith(PieceColour pieceColour) =>
        pieceColour == PieceColour.White
        ? _whiteBoardEval
        : _blackBoardEval;
}
