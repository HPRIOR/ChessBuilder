using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class PossibleMoveFactory : IPossibleMoveFactory
    {
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _pieceMoveGenerators;
        private readonly PossibleBishopMovesFactory _possibleBishopMovesFactory;
        private readonly PossibleKingMovesFactory _possibleKingMovesFactory;
        private readonly PossibleKnightMovesFactory _possibleKnightMovesFactory;
        private readonly PossiblePawnMovesFactory _possiblePawnMovesFactory;
        private readonly PossibleQueenMovesFactory _possibleQueenMovesFactory;
        private readonly PossibleRookMovesFactory _possibleRookMovesFactory;

        public PossibleMoveFactory(
            PossiblePawnMovesFactory possiblePawnMovesFactory,
            PossibleBishopMovesFactory possibleBishopMovesFactory,
            PossibleKnightMovesFactory possibleKnightMovesFactory,
            PossibleRookMovesFactory possibleRookMovesFactory,
            PossibleQueenMovesFactory possibleQueenMovesFactory,
            PossibleKingMovesFactory possibleKingMovesFactory
        )
        {
            _possiblePawnMovesFactory = possiblePawnMovesFactory;
            _possibleBishopMovesFactory = possibleBishopMovesFactory;
            _possibleKnightMovesFactory = possibleKnightMovesFactory;
            _possibleRookMovesFactory = possibleRookMovesFactory;
            _possibleQueenMovesFactory = possibleQueenMovesFactory;
            _possibleKingMovesFactory = possibleKingMovesFactory;
            _pieceMoveGenerators = GetPieceMoveGenerators();
        }

        public IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece)
        {
            return _pieceMoveGenerators[piece.Type];
        }

        private Dictionary<PieceType, IPieceMoveGenerator> GetPieceMoveGenerators()
        {
            return new Dictionary<PieceType, IPieceMoveGenerator>
            {
                {PieceType.BlackPawn, _possiblePawnMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhitePawn, _possiblePawnMovesFactory.Create(PieceColour.White)},
                {PieceType.BlackBishop, _possibleBishopMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhiteBishop, _possibleBishopMovesFactory.Create(PieceColour.White)},
                {PieceType.BlackKnight, _possibleKnightMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhiteKnight, _possibleKnightMovesFactory.Create(PieceColour.White)},
                {PieceType.BlackRook, _possibleRookMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhiteRook, _possibleRookMovesFactory.Create(PieceColour.White)},
                {PieceType.BlackKing, _possibleKingMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhiteKing, _possibleKingMovesFactory.Create(PieceColour.White)},
                {PieceType.BlackQueen, _possibleQueenMovesFactory.Create(PieceColour.Black)},
                {PieceType.WhiteQueen, _possibleQueenMovesFactory.Create(PieceColour.White)},
                {PieceType.NullPiece, new NullMoveGenerator()}
            };
        }
    }
}