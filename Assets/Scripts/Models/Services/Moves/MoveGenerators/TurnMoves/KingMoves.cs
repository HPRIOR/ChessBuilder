using System;
using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class KingMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public KingMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var potentialMoves = new List<Position>();
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                var newPosition = relativePosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if (!(0 > newPosition.X || newPosition.X > 7
                                        || 0 > newPosition.Y || newPosition.Y > 7))
                {
                    var potentialMoveTile = _positionTranslator.GetRelativeTileAt(newPosition, boardState);
                    if (_tileEvaluator.OpposingPieceIn(potentialMoveTile) ||
                        potentialMoveTile.CurrentPiece.Type == PieceType.NullPiece)
                        potentialMoves.Add(newRelativePosition);
                }
            }

            return potentialMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, KingMoves>
        {
        }
    }
}