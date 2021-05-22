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
        private readonly IBoardMoveEval _boardMoveEval;

        public PossibleKingMoves(IPositionTranslator positionTranslator, IBoardMoveEval boardMoveEval)
        {
            _positionTranslator = positionTranslator;
            _boardMoveEval = boardMoveEval;
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
                if (_boardMoveEval.OpposingPieceIn(potentialMoveTile) || _boardMoveEval.NoPieceIn(potentialMoveTile))
                    potentialMoves.Add(newRelativePosition);
            });

            return potentialMoves;
        }
    }
}