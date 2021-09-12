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
        private static readonly Direction[] _directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private readonly IPositionTranslator _positionTranslator;

        public KingNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var potentialMoves = new List<Position>();
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            foreach (var direction in _directions)
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