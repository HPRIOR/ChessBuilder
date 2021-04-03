public class PieceMoveGeneratorFactory : IPieceMoveGeneratorFactory
{
    private readonly IBoardEval _whiteBoardEval;
    private readonly IBoardEval _blackBoardEval;

    private readonly IPositionTranslator _whitePositionTranslator;
    private readonly IPositionTranslator _blackPositionTranslator;

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
            var bishop when bishop == PieceType.BlackBishop || bishop == PieceType.WhiteBishop => new PossibleBishopMoves(
                GetBoardScannerWith(PieceColourFrom(pieceType)), GetPositionTranslatorWith(PieceColourFrom(pieceType))),
            var knight when knight == PieceType.BlackKnight || knight == PieceType.WhiteKnight => new PossibleKnightMoves(
                GetPositionTranslatorWith(PieceColourFrom(pieceType)), GetBoardEvalWith(PieceColourFrom(pieceType))
                ),
            var rook when rook == PieceType.BlackRook || rook == PieceType.WhiteRook => new PossibleRookMoves(
                GetBoardScannerWith(PieceColourFrom(pieceType)),
                GetPositionTranslatorWith(PieceColourFrom(pieceType))
                ),
            var king when king == PieceType.BlackKing || king == PieceType.WhiteKing => new PossibleKingMoves(
                GetPositionTranslatorWith(PieceColourFrom(pieceType)), GetBoardEvalWith(PieceColourFrom(pieceType))),
            var queen when queen == PieceType.BlackQueen || queen == PieceType.WhiteQueen => new PossibleQueenMoves(
                GetPositionTranslatorWith(PieceColourFrom(pieceType)), GetBoardScannerWith(PieceColourFrom(pieceType))),
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

    private IPositionTranslator GetPositionTranslatorWith(PieceColour pieceColour) =>
        pieceColour == PieceColour.White
        ? _whitePositionTranslator
        : _blackPositionTranslator;

    private IBoardEval GetBoardEvalWith(PieceColour pieceColour) =>
        pieceColour == PieceColour.White
        ? _whiteBoardEval
        : _blackBoardEval;
}