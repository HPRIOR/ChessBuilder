using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.MoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public class MoveGeneratorRepository : IMoveGeneratorRepository
    {
        private readonly MovesFactory _movesFactory;
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _nonMovePieceGenerator;
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _pieceMoveGenerators;

        public MoveGeneratorRepository(
            MovesFactory movesFactory
        )
        {
            _movesFactory = movesFactory;
            _pieceMoveGenerators = GetPieceMoveGenerators(true);
            _nonMovePieceGenerator = GetPieceMoveGenerators(false);
        }

        public IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece, bool turnMove) =>
            turnMove ? _pieceMoveGenerators[piece.Type] : _nonMovePieceGenerator[piece.Type];

        private Dictionary<PieceType, IPieceMoveGenerator> GetPieceMoveGenerators(bool turnMove) =>
            new Dictionary<PieceType, IPieceMoveGenerator>
            {
                {PieceType.BlackPawn, _movesFactory.Create(PieceType.BlackPawn, turnMove)},
                {PieceType.WhitePawn, _movesFactory.Create(PieceType.WhitePawn, turnMove)},
                {PieceType.BlackBishop, _movesFactory.Create(PieceType.BlackBishop, turnMove)},
                {PieceType.WhiteBishop, _movesFactory.Create(PieceType.WhiteBishop, turnMove)},
                {PieceType.BlackKnight, _movesFactory.Create(PieceType.BlackKnight, turnMove)},
                {PieceType.WhiteKnight, _movesFactory.Create(PieceType.WhiteKnight, turnMove)},
                {PieceType.BlackRook, _movesFactory.Create(PieceType.BlackRook, turnMove)},
                {PieceType.WhiteRook, _movesFactory.Create(PieceType.WhiteRook, turnMove)},
                {PieceType.BlackKing, _movesFactory.Create(PieceType.BlackKing, turnMove)},
                {PieceType.WhiteKing, _movesFactory.Create(PieceType.WhiteKing, turnMove)},
                {PieceType.BlackQueen, _movesFactory.Create(PieceType.BlackQueen, turnMove)},
                {PieceType.WhiteQueen, _movesFactory.Create(PieceType.WhiteQueen, turnMove)},
                {PieceType.NullPiece, new NullMoveGenerator()}
            };
    }
}