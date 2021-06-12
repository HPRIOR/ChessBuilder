using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class MoveGeneratorRepository : IMoveGeneratorRepository
    {
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _nonMovePieceGenerator;
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _pieceMoveGenerators;
        private readonly PossibleMovesFactory _possibleMovesFactory;

        public MoveGeneratorRepository(
            PossibleMovesFactory possibleMovesFactory
        )
        {
            _possibleMovesFactory = possibleMovesFactory;
            _pieceMoveGenerators = GetPieceMoveGenerators(true);
            _nonMovePieceGenerator = GetPieceMoveGenerators(false);
        }

        public IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece, bool turnMove) =>
            turnMove ? _pieceMoveGenerators[piece.Type] : _nonMovePieceGenerator[piece.Type];

        private Dictionary<PieceType, IPieceMoveGenerator> GetPieceMoveGenerators(bool turnMove) =>
            new Dictionary<PieceType, IPieceMoveGenerator>
            {
                {PieceType.BlackPawn, _possibleMovesFactory.Create(PieceType.BlackPawn, turnMove)},
                {PieceType.WhitePawn, _possibleMovesFactory.Create(PieceType.WhitePawn, turnMove)},
                {PieceType.BlackBishop, _possibleMovesFactory.Create(PieceType.BlackBishop, turnMove)},
                {PieceType.WhiteBishop, _possibleMovesFactory.Create(PieceType.WhiteBishop, turnMove)},
                {PieceType.BlackKnight, _possibleMovesFactory.Create(PieceType.BlackKnight, turnMove)},
                {PieceType.WhiteKnight, _possibleMovesFactory.Create(PieceType.WhiteKnight, turnMove)},
                {PieceType.BlackRook, _possibleMovesFactory.Create(PieceType.BlackRook, turnMove)},
                {PieceType.WhiteRook, _possibleMovesFactory.Create(PieceType.WhiteRook, turnMove)},
                {PieceType.BlackKing, _possibleMovesFactory.Create(PieceType.BlackKing, turnMove)},
                {PieceType.WhiteKing, _possibleMovesFactory.Create(PieceType.WhiteKing, turnMove)},
                {PieceType.BlackQueen, _possibleMovesFactory.Create(PieceType.BlackQueen, turnMove)},
                {PieceType.WhiteQueen, _possibleMovesFactory.Create(PieceType.WhiteQueen, turnMove)},
                {PieceType.NullPiece, new NullMoveGenerator()}
            };
    }
}