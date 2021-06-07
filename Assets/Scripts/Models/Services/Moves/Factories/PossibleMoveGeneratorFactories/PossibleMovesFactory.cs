using System;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleMovesFactory
    {
        private readonly BishopMoves.Factory _bishopMovesFactory;
        private readonly KingMoves.Factory _kingTurnMovesFactory;
        private readonly KnightMoves.Factory _knightMovesFactory;
        private readonly PawnMoves.Factory _pawnMovesFactory;
        private readonly QueenMoves.Factory _queenTurnMovesFactory;
        private readonly RookTurnMoves.Factory _rookMovesFactory;

        public PossibleMovesFactory(
            PawnMoves.Factory pawnMovesFactory,
            BishopMoves.Factory bishopMovesFactory,
            RookTurnMoves.Factory rookMovesFactory,
            KnightMoves.Factory knightMovesFactory,
            KingMoves.Factory kingTurnMovesFactory,
            QueenMoves.Factory queenTurnMovesFactory)
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
            return pieceType switch
            {
                PieceType.NullPiece => new NullMoveGenerator(),
                PieceType.BlackKing => _kingTurnMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.BlackQueen => _queenTurnMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.BlackRook => _rookMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.BlackBishop => _bishopMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.BlackKnight => _knightMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.BlackPawn => _pawnMovesFactory.Create(PieceColour.Black, turnMove),
                PieceType.WhiteKing => _kingTurnMovesFactory.Create(PieceColour.White, turnMove),
                PieceType.WhiteQueen => _queenTurnMovesFactory.Create(PieceColour.White, turnMove),
                PieceType.WhiteRook => _rookMovesFactory.Create(PieceColour.White, turnMove),
                PieceType.WhiteBishop => _bishopMovesFactory.Create(PieceColour.White, turnMove),
                PieceType.WhiteKnight => _knightMovesFactory.Create(PieceColour.White, turnMove),
                PieceType.WhitePawn => _pawnMovesFactory.Create(PieceColour.White, turnMove),
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
            };
        }
    }
}