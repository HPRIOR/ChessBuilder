using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class KingMoves : IPieceMoveGenerator
    {
        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public KingMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        public HashSet<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var possibleMoves = new HashSet<Position>();
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            foreach (var direction in Directions)
            {
                var newPosition = relativePosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if (!(0 > newPosition.X || newPosition.X > 7
                                        || 0 > newPosition.Y || newPosition.Y > 7))
                {
                    var potentialMoveTile = _positionTranslator.GetRelativeTileAt(newPosition, boardState);
                    if (_tileEvaluator.OpposingPieceIn(potentialMoveTile) ||
                        potentialMoveTile.CurrentPiece.Type == PieceType.NullPiece)
                        possibleMoves.Add(newRelativePosition);
                }
            }

            return possibleMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, KingMoves>
        {
        }
    }
}