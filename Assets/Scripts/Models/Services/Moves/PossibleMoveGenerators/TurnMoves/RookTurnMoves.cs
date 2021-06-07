﻿using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveGenerators.TurnMoves
{
    public class RookTurnMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public RookTurnMoves(PieceColour pieceColour, bool turnMove, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, turnMove);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);
            var possibleDirections = new List<Direction> {Direction.N, Direction.E, Direction.S, Direction.W};

            return possibleDirections.SelectMany(direction =>
                _boardScanner.ScanIn(direction, relativePosition, boardState));
        }

        public class Factory : PlaceholderFactory<PieceColour, bool, RookTurnMoves>
        {
        }
    }
}