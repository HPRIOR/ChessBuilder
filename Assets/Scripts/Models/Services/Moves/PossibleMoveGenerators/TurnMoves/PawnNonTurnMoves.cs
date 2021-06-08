using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveGenerators.TurnMoves
{
    public class PawnNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public PawnNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour, true);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var potentialMoves = new List<BoardPosition>();
            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return potentialMoves;

            if (originPosition.X > 0)
            {
                var topLeftTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)), boardState);

                if (_tileEvaluator.FriendlyPieceIn(topLeftTile) || _tileEvaluator.NoPieceIn(topLeftTile))
                    potentialMoves.Add(
                        topLeftTile.BoardPosition
                    );
            }

            if (originPosition.X < 7)
            {
                var topRightTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)), boardState);
                if (_tileEvaluator.FriendlyPieceIn(topRightTile) || _tileEvaluator.NoPieceIn(topRightTile))
                    potentialMoves.Add(
                        topRightTile.BoardPosition
                    );
            }

            return potentialMoves;
        }


        public class Factory : PlaceholderFactory<PieceColour, PawnNonTurnMoves>
        {
        }
    }
}