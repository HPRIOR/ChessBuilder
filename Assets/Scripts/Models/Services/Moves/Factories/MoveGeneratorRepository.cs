using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

//TODO rename to repository
namespace Models.Services.Moves.Factories
{
    public class MoveGeneratorRepository : IMoveGeneratorRepository
    {
        private readonly Dictionary<PieceType, IPieceMoveGenerator> _pieceMoveGenerators;
        private readonly PossibleMovesFactory _possibleMovesFactory;

        public MoveGeneratorRepository(
            PossibleMovesFactory possibleMovesFactory
        )
        {
            _possibleMovesFactory = possibleMovesFactory;
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
                {PieceType.BlackPawn, _possibleMovesFactory.Create(PieceType.BlackPawn, true)},
                {PieceType.WhitePawn, _possibleMovesFactory.Create(PieceType.WhitePawn, true)},
                {PieceType.BlackBishop, _possibleMovesFactory.Create(PieceType.BlackBishop, true)},
                {PieceType.WhiteBishop, _possibleMovesFactory.Create(PieceType.WhiteBishop, true)},
                {PieceType.BlackKnight, _possibleMovesFactory.Create(PieceType.BlackKnight, true)},
                {PieceType.WhiteKnight, _possibleMovesFactory.Create(PieceType.WhiteKnight, true)},
                {PieceType.BlackRook, _possibleMovesFactory.Create(PieceType.BlackRook, true)},
                {PieceType.WhiteRook, _possibleMovesFactory.Create(PieceType.WhiteRook, true)},
                {PieceType.BlackKing, _possibleMovesFactory.Create(PieceType.BlackKing, true)},
                {PieceType.WhiteKing, _possibleMovesFactory.Create(PieceType.WhiteKing, true)},
                {PieceType.BlackQueen, _possibleMovesFactory.Create(PieceType.BlackQueen, true)},
                {PieceType.WhiteQueen, _possibleMovesFactory.Create(PieceType.WhiteQueen, true)},
                {PieceType.NullPiece, new NullMoveGenerator()}
            };
        }
    }
}