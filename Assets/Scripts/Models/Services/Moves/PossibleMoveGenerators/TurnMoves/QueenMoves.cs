using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveGenerators.TurnMoves
{
    public class QueenMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public QueenMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            IBoardScannerFactory boardScannerFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.Turn);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            return Enum
                .GetValues(typeof(Direction))
                .Cast<Direction>()
                .ToList()
                .SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition, boardState));
        }

        public class Factory : PlaceholderFactory<PieceColour, QueenMoves>
        {
        }
    }
}