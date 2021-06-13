using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class PawnNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public PawnNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var potentialMoves = new List<Position>();
            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return potentialMoves;

            if (originPosition.X > 0)
            {
                var topLeftTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)), boardState);

                potentialMoves.Add(topLeftTile.Position);
            }

            if (originPosition.X < 7)
            {
                var topRightTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)), boardState);
                potentialMoves.Add(topRightTile.Position);
            }

            return potentialMoves;
        }


        public class Factory : PlaceholderFactory<PieceColour, PawnNonTurnMoves>
        {
        }
    }
}