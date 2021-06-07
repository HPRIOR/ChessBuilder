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
    public class KingMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public KingMoves(PieceColour pieceColour, bool turnMove, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour, false);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var potentialMoves = new List<BoardPosition>();
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList().ForEach(direction =>
            {
                var newPosition = relativePosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if (0 > newPosition.X || newPosition.X > 7
                                      || 0 > newPosition.Y || newPosition.Y > 7) return;
                var potentialMoveTile = _positionTranslator.GetRelativeTileAt(newPosition, boardState);
                if (_tileEvaluator.OpposingPieceIn(potentialMoveTile) || _tileEvaluator.NoPieceIn(potentialMoveTile))
                    potentialMoves.Add(newRelativePosition);
            });

            return potentialMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, bool, KingMoves>
        {
        }
    }
}