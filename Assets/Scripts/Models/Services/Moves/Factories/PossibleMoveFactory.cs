using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class PossibleMoveFactory : IPossibleMoveFactory
    {
        private readonly IBoardScanner _blackBoardScanner;
        private readonly IPositionTranslator _blackPositionTranslator;
        private readonly ITileEvaluator _blackTileEvaluator;

        private readonly IBoardScanner _whiteBoardScanner;

        private readonly IPositionTranslator _whitePositionTranslator;
        private readonly ITileEvaluator _whiteTileEvaluator;

        public PossibleMoveFactory(
            IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory boardPositionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _whiteTileEvaluator = tileEvaluatorFactory.Create(PieceColour.White);
            _blackTileEvaluator = tileEvaluatorFactory.Create(PieceColour.Black);

            _whiteBoardScanner = boardScannerFactory.Create(PieceColour.White);
            _blackBoardScanner = boardScannerFactory.Create(PieceColour.Black);

            _whitePositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.White);
            _blackPositionTranslator = boardPositionTranslatorFactory.Create(PieceColour.Black);
        }

        /// <summary>
        ///     Builds the relevant possible move generating class for a piece and colour. The colour is passed to the
        ///     constructor of the position translator and board evaluators, in order
        /// </summary>
        /// <remarks>
        ///     Necessary to get the relevant position translator and board evaluator based on the colour of the piece.
        ///     Added complexity, although it allows movement logic to be symmetrical among piece colours, and share.
        /// </remarks>
        /// <param name="pieceType"></param>
        /// <returns></returns>
        public IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece)
        {
            // TODO refactor each possible move generator to use zenject factories 
            //      GetPossibleMoveGenerator takes in Piece instead of piece type so that colour can be used 
            //      and passed to factory.
            //      Factories of each possible move generator made responsible for instantiating auxiliary class
            //      All possible moves then instantiates all the relevant move generators in once go and calls their methods 
            //      when relevant. These should not be generated each move, but persist throughout the program.
            return piece.Type switch
            {
                var pawn when pawn == PieceType.BlackPawn || pawn == PieceType.WhitePawn => new PossiblePawnMoves(
                    GetPositionTranslatorWith(piece.Colour),
                    GetBoardEvalWith(piece.Colour)),
                var bishop when bishop == PieceType.BlackBishop || bishop == PieceType.WhiteBishop => new
                    PossibleBishopMoves(
                        GetBoardScannerWith(piece.Colour),
                        GetPositionTranslatorWith(piece.Colour)),
                var knight when knight == PieceType.BlackKnight || knight == PieceType.WhiteKnight => new
                    PossibleKnightMoves(
                        GetPositionTranslatorWith(piece.Colour),
                        GetBoardEvalWith(piece.Colour)
                    ),
                var rook when rook == PieceType.BlackRook || rook == PieceType.WhiteRook => new PossibleRookMoves(
                    GetBoardScannerWith(piece.Colour),
                    GetPositionTranslatorWith(piece.Colour)
                ),
                var king when king == PieceType.BlackKing || king == PieceType.WhiteKing => new PossibleKingMoves(
                    GetPositionTranslatorWith(piece.Colour),
                    GetBoardEvalWith(piece.Colour)),
                var queen when queen == PieceType.BlackQueen || queen == PieceType.WhiteQueen => new PossibleQueenMoves(
                    GetPositionTranslatorWith(piece.Colour),
                    GetBoardScannerWith(piece.Colour)),
                _ => new NullPossibleMoveGenerator()
            };
        }

        private IBoardScanner GetBoardScannerWith(PieceColour pieceColour)
        {
            return pieceColour == PieceColour.White
                ? _whiteBoardScanner
                : _blackBoardScanner;
        }

        private IPositionTranslator GetPositionTranslatorWith(PieceColour pieceColour)
        {
            return pieceColour == PieceColour.White
                ? _whitePositionTranslator
                : _blackPositionTranslator;
        }

        private ITileEvaluator GetBoardEvalWith(PieceColour pieceColour)
        {
            return pieceColour == PieceColour.White
                ? _whiteTileEvaluator
                : _blackTileEvaluator;
        }
    }
}