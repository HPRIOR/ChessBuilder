﻿using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class BoardScanner : IBoardScanner
    {
        private readonly IBoardMoveEval _boardMoveEval;
        private readonly IPositionTranslator _positionTranslator;

        public BoardScanner(
            PieceColour pieceColour,
            IBoardEvalFactory boardEvalFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardMoveEval = boardEvalFactory.Create(pieceColour);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition currentPosition, IBoardState boardState)
        {
            var newPosition = currentPosition.Add(Move.In(direction));
            if (PieceCannotMoveTo(newPosition, boardState))
                return new List<IBoardPosition>();
            if (TileContainsOpposingPieceAt(newPosition, boardState))
                return new List<IBoardPosition>() { _positionTranslator.GetRelativePosition(newPosition) };
            return ScanIn(direction, newPosition, boardState)
                .Concat(new List<IBoardPosition>() { _positionTranslator.GetRelativePosition(newPosition) });
        }

        private bool PieceCannotMoveTo(IBoardPosition boardPosition, IBoardState boardState)
        {
            var x = boardPosition.X; var y = boardPosition.Y;
            return 0 > x || x > 7 || 0 > y || y > 7 || TileContainsFriendlyPieceAt(boardPosition, boardState);
        }

        private bool TileContainsOpposingPieceAt(IBoardPosition boardPosition, IBoardState boardState) =>
            _boardMoveEval.OpposingPieceIn(_positionTranslator.GetRelativeTileAt(boardPosition, boardState));

        private bool TileContainsFriendlyPieceAt(IBoardPosition boardPosition, IBoardState boardState) =>
            _boardMoveEval.FriendlyPieceIn(_positionTranslator.GetRelativeTileAt(boardPosition, boardState));

        public class Factory : PlaceholderFactory<PieceColour, BoardScanner> { }
    }
}