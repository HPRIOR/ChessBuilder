using System.Collections.Generic;
using Models.Services.Moves.Factories;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.MoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public class MovesGeneratorRepository : IMovesGeneratorRepository
    {
        private readonly MovesFactory _movesFactory;
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _nonMovePieceGenerator;
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _pieceMoveGenerators;

        public MovesGeneratorRepository(
            MovesFactory movesFactory
        )
        {
            _movesFactory = movesFactory;
            _pieceMoveGenerators = GetPieceMoveGenerators(true);
            _nonMovePieceGenerator = GetPieceMoveGenerators(false);
        }

        public IPieceMoveGenerator GetPossibleMoveGenerator(Piece piece, bool turnMove) =>
            turnMove ? _pieceMoveGenerators[piece.Type] : _nonMovePieceGenerator[piece.Type];

        private Dictionary<PieceType, IPieceMoveGenerator> GetPieceMoveGenerators(bool turnMoveType) =>
            new Dictionary<PieceType, IPieceMoveGenerator>(new PieceTypeComparer())
            {
                { PieceType.BlackPawn, _movesFactory.Create(PieceType.BlackPawn, turnMoveType) },
                { PieceType.WhitePawn, _movesFactory.Create(PieceType.WhitePawn, turnMoveType) },
                { PieceType.BlackBishop, _movesFactory.Create(PieceType.BlackBishop, turnMoveType) },
                { PieceType.WhiteBishop, _movesFactory.Create(PieceType.WhiteBishop, turnMoveType) },
                { PieceType.BlackKnight, _movesFactory.Create(PieceType.BlackKnight, turnMoveType) },
                { PieceType.WhiteKnight, _movesFactory.Create(PieceType.WhiteKnight, turnMoveType) },
                { PieceType.BlackRook, _movesFactory.Create(PieceType.BlackRook, turnMoveType) },
                { PieceType.WhiteRook, _movesFactory.Create(PieceType.WhiteRook, turnMoveType) },
                { PieceType.BlackKing, _movesFactory.Create(PieceType.BlackKing, turnMoveType) },
                { PieceType.WhiteKing, _movesFactory.Create(PieceType.WhiteKing, turnMoveType) },
                { PieceType.BlackQueen, _movesFactory.Create(PieceType.BlackQueen, turnMoveType) },
                { PieceType.WhiteQueen, _movesFactory.Create(PieceType.WhiteQueen, turnMoveType) },
                { PieceType.NullPiece, new NullMoveGenerator() }
            };
    }
}