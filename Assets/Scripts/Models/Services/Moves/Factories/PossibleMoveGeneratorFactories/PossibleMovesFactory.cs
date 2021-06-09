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
        private readonly PawnNonTurnMoves.Factory _pawnNonMovesFactory;
        private readonly QueenMoves.Factory _queenTurnMovesFactory;
        private readonly RookTurnMoves.Factory _rookMovesFactory;

        public PossibleMovesFactory(
            PawnMoves.Factory pawnMovesFactory,
            PawnNonTurnMoves.Factory pawnNonMovesFactory,
            BishopMoves.Factory bishopMovesFactory,
            RookTurnMoves.Factory rookMovesFactory,
            KnightMoves.Factory knightMovesFactory,
            KingMoves.Factory kingTurnMovesFactory,
            QueenMoves.Factory queenTurnMovesFactory)
        {
            _pawnMovesFactory = pawnMovesFactory;
            _pawnNonMovesFactory = pawnNonMovesFactory;
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
                PieceType.BlackKing => _kingTurnMovesFactory.Create(PieceColour.Black),
                PieceType.BlackQueen => _queenTurnMovesFactory.Create(PieceColour.Black),
                PieceType.BlackRook => _rookMovesFactory.Create(PieceColour.Black),
                PieceType.BlackBishop => _bishopMovesFactory.Create(PieceColour.Black),
                PieceType.BlackKnight => _knightMovesFactory.Create(PieceColour.Black),
                PieceType.BlackPawn => turnMove switch
                {
                    true => _pawnMovesFactory.Create(PieceColour.Black),
                    false => _pawnNonMovesFactory.Create(PieceColour.Black)
                },
                PieceType.WhiteKing => _kingTurnMovesFactory.Create(PieceColour.White),
                PieceType.WhiteQueen => _queenTurnMovesFactory.Create(PieceColour.White),
                PieceType.WhiteRook => _rookMovesFactory.Create(PieceColour.White),
                PieceType.WhiteBishop => _bishopMovesFactory.Create(PieceColour.White),
                PieceType.WhiteKnight => _knightMovesFactory.Create(PieceColour.White),
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