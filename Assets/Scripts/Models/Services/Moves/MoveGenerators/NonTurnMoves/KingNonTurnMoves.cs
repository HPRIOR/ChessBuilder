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
        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W, Direction.Ne, Direction.Nw, Direction.Se, Direction.Sw
        };

        private readonly IPositionTranslator _positionTranslator;

        public KingNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var possibleMoves = new List<Position>();
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            for (var index = 0; index < Directions.Length; index++)
            {
                var direction = Directions[index];
                var newPosition = relativePosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if (!(0 > newPosition.X || newPosition.X > 7
                                        || 0 > newPosition.Y || newPosition.Y > 7))
                    possibleMoves.Add(newRelativePosition);
            }

            return possibleMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, KingNonTurnMoves>
        {
        }
    }
}