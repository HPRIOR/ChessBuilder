using System;
using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.NonTurnMoves
{
    public class KingNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;

        public KingNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
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
                    potentialMoves.Add(newRelativePosition);
            }

            return potentialMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, KingNonTurnMoves>
        {
        }
    }
}