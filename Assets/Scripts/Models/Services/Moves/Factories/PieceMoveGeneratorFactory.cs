using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class PieceMoveGeneratorFactory : IPieceMoveGeneratorFactory
    {
        private readonly IBoardMoveEval _whiteBoardMoveEval;
        private readonly IBoardMoveEval _blackBoardMoveEval;

        private readonly IPositionTranslator _whitePositionTranslator;
        private readonly IPositionTranslator _blackPositionTranslator;

        private readonly IBoardScanner _whiteBoardScanner;
        private readonly IBoardScanner _blackBoardScanner;

        public PieceMoveGeneratorFactory(
            IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory boardPositionTranslatorFactory,
            IBoardEvalFactory boardEvalFactory)
        {
            _whiteBoardMoveEval = boardEvalFactory.Create(PieceColour.White);
            _blackBoardMoveEval = boardEvalFactory.Create(PieceColour.Black);

            _whiteBoardScanner = boardScannerFactory.Create(PieceColour.White);
            _blackBoardScanner = boardScannerFactory.Create(PieceColour.Black);

            _whitePositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.White);
            _blackPositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.Black);
        }

        /// <summary>
        /// Instantiates the relevant board evaluating class which determines the available moves to a given
        /// piece. 
        /// </summary>
        /// <remarks>
        /// Necessary to get the relevant position translator and board evaluator based on the colour of the piece.
        /// Added complexity, although it allows movement logic to be symmetrical among piece colours, and share. 
        /// </remarks>
        /// <param name="pieceType"></param>
        /// <returns></returns>
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

        private static PieceColour PieceColourFrom(PieceType pieceType) =>
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

        private IBoardMoveEval GetBoardEvalWith(PieceColour pieceColour) =>
            pieceColour == PieceColour.White
                ? _whiteBoardMoveEval
                : _blackBoardMoveEval;
    }
}