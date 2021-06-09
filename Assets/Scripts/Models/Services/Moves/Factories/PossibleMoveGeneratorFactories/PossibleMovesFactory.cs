using System;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories.PossibleMoveGeneratorFactories
{
    public class PossibleMovesFactory
    {
        private readonly BishopMoves.Factory _bishopMovesFactory;
        private readonly BishopNonTurnMoves.Factory _bishopNonTurnMovesFactory;
        private readonly KingNonTurnMoves.Factory _kingNonTurnMovesFactory;
        private readonly KingMoves.Factory _kingTurnMovesFactory;
        private readonly KnightMoves.Factory _knightMovesFactory;
        private readonly KingNonTurnMoves.Factory _knightNonTurnMovesFactory;
        private readonly PawnMoves.Factory _pawnMovesFactory;
        private readonly PawnNonTurnMoves.Factory _pawnNonMovesFactory;
        private readonly QueenNonTurnMoves.Factory _queenNonTurnMovesFactory;
        private readonly QueenMoves.Factory _queenTurnMovesFactory;
        private readonly RookTurnMoves.Factory _rookMovesFactory;
        private readonly RookNonTurnMoves.Factory _rookNonTurnMovesFactory;

        public PossibleMovesFactory(
            PawnMoves.Factory pawnMovesFactory,
            PawnNonTurnMoves.Factory pawnNonMovesFactory,
            BishopMoves.Factory bishopMovesFactory,
            BishopNonTurnMoves.Factory bishopNonTurnMovesFactory,
            RookTurnMoves.Factory rookMovesFactory,
            RookNonTurnMoves.Factory rookNonTurnMovesFactory,
            KnightMoves.Factory knightMovesFactory,
            KingNonTurnMoves.Factory knightNonTurnMovesFactory,
            KingMoves.Factory kingTurnMovesFactory,
            KingNonTurnMoves.Factory kingNonTurnMovesFactory,
            QueenMoves.Factory queenTurnMovesFactory,
            QueenNonTurnMoves.Factory queenNonTurnMovesFactory)
        {
            _pawnMovesFactory = pawnMovesFactory;
            _pawnNonMovesFactory = pawnNonMovesFactory;
            _bishopMovesFactory = bishopMovesFactory;
            _bishopNonTurnMovesFactory = bishopNonTurnMovesFactory;
            _rookMovesFactory = rookMovesFactory;
            _rookNonTurnMovesFactory = rookNonTurnMovesFactory;
            _knightMovesFactory = knightMovesFactory;
            _knightNonTurnMovesFactory = knightNonTurnMovesFactory;
            _kingTurnMovesFactory = kingTurnMovesFactory;
            _kingNonTurnMovesFactory = kingNonTurnMovesFactory;
            _queenTurnMovesFactory = queenTurnMovesFactory;
            _queenNonTurnMovesFactory = queenNonTurnMovesFactory;
        }

        public IPieceMoveGenerator Create(PieceType pieceType, bool turnMove)
        {
            return pieceType switch
            {
                PieceType.NullPiece => new NullMoveGenerator(),
                PieceType.BlackKing => turnMove switch
                {
                    true => _kingTurnMovesFactory.Create(PieceColour.Black),
                    false => _kingNonTurnMovesFactory.Create(PieceColour.Black)
                },
                PieceType.BlackQueen => turnMove switch
                {
                    true => _queenTurnMovesFactory.Create(PieceColour.Black),
                    false => _queenNonTurnMovesFactory.Create(PieceColour.Black)
                },
                PieceType.BlackRook => turnMove switch
                {
                    true => _rookMovesFactory.Create(PieceColour.Black),
                    false => _rookNonTurnMovesFactory.Create(PieceColour.Black)
                },
                PieceType.BlackBishop => turnMove switch
                {
                    true => _bishopMovesFactory.Create(PieceColour.Black),
                    false => _bishopNonTurnMovesFactory.Create(PieceColour.Black)
                },
                PieceType.BlackKnight => turnMove switch
                {
                    true => _knightMovesFactory.Create(PieceColour.Black),
                    false => _knightNonTurnMovesFactory.Create(PieceColour.Black)
                },
                PieceType.BlackPawn => turnMove switch
                {
                    true => _pawnMovesFactory.Create(PieceColour.Black),
                    false => _pawnNonMovesFactory.Create(PieceColour.Black)
                },
                PieceType.WhiteKing => turnMove switch
                {
                    true => _kingTurnMovesFactory.Create(PieceColour.White),
                    false => _kingNonTurnMovesFactory.Create(PieceColour.White)
                },
                PieceType.WhiteQueen => turnMove switch
                {
                    true => _queenTurnMovesFactory.Create(PieceColour.White),
                    false => _queenNonTurnMovesFactory.Create(PieceColour.White)
                },
                PieceType.WhiteRook => turnMove switch
                {
                    true => _rookMovesFactory.Create(PieceColour.White),
                    false => _rookNonTurnMovesFactory.Create(PieceColour.White)
                },
                PieceType.WhiteBishop => turnMove switch
                {
                    true => _bishopMovesFactory.Create(PieceColour.White),
                    false => _bishopNonTurnMovesFactory.Create(PieceColour.White)
                },
                PieceType.WhiteKnight => turnMove switch
                {
                    true => _knightMovesFactory.Create(PieceColour.White),
                    false => _knightNonTurnMovesFactory.Create(PieceColour.White)
                },
                PieceType.WhitePawn => turnMove switch
                {
                    true => _pawnMovesFactory.Create(PieceColour.White),
                    false => _pawnNonMovesFactory.Create(PieceColour.White)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
            };
        }
    }
}