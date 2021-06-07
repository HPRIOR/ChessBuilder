using System;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleMovesFactory
    {
        private readonly BishopTurnMoves.Factory _bishopMovesFactory;
        private readonly KingTurnMoves.Factory _kingTurnMovesFactory;
        private readonly KnightTurnMoves.Factory _knightMovesFactory;
        private readonly PawnTurnMoves.Factory _pawnMovesFactory;
        private readonly QueenTurnMoves.Factory _queenTurnMovesFactory;
        private readonly RookTurnMoves.Factory _rookMovesFactory;

        public PossibleMovesFactory(
            PawnTurnMoves.Factory pawnMovesFactory,
            BishopTurnMoves.Factory bishopMovesFactory,
            RookTurnMoves.Factory rookMovesFactory,
            KnightTurnMoves.Factory knightMovesFactory,
            KingTurnMoves.Factory kingTurnMovesFactory,
            QueenTurnMoves.Factory queenTurnMovesFactory)
        {
            _pawnMovesFactory = pawnMovesFactory;
            _bishopMovesFactory = bishopMovesFactory;
            _rookMovesFactory = rookMovesFactory;
            _knightMovesFactory = knightMovesFactory;
            _kingTurnMovesFactory = kingTurnMovesFactory;
            _queenTurnMovesFactory = queenTurnMovesFactory;
        }

        public IPieceMoveGenerator Create(PieceType pieceType, bool turnMove)
        {
            var isReversed = !turnMove;
            return pieceType switch
            {
                PieceType.NullPiece => new NullMoveGenerator(),
                PieceType.BlackKing => _kingTurnMovesFactory.Create(PieceColour.Black),
                PieceType.BlackQueen => _queenTurnMovesFactory.Create(PieceColour.Black),
                PieceType.BlackRook => _rookMovesFactory.Create(PieceColour.Black),
                PieceType.BlackBishop => _bishopMovesFactory.Create(PieceColour.Black),
                PieceType.BlackKnight => _knightMovesFactory.Create(PieceColour.Black),
                PieceType.BlackPawn => _pawnMovesFactory.Create(PieceColour.Black),
                PieceType.WhiteKing => _kingTurnMovesFactory.Create(PieceColour.White),
                PieceType.WhiteQueen => _queenTurnMovesFactory.Create(PieceColour.White),
                PieceType.WhiteRook => _rookMovesFactory.Create(PieceColour.White),
                PieceType.WhiteBishop => _bishopMovesFactory.Create(PieceColour.White),
                PieceType.WhiteKnight => _knightMovesFactory.Create(PieceColour.White),
                PieceType.WhitePawn => _pawnMovesFactory.Create(PieceColour.White),
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
            };
        }
    }
}