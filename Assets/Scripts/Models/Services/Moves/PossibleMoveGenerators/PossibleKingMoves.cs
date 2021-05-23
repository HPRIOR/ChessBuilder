using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Interfaces;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossibleKingMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public PossibleKingMoves(IPositionTranslator positionTranslator, ITileEvaluator tileEvaluator)
        {
            _positionTranslator = positionTranslator;
            _tileEvaluator = tileEvaluator;
        }

        public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
        {
            var potentialMoves = new List<IBoardPosition>();
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
    }
}