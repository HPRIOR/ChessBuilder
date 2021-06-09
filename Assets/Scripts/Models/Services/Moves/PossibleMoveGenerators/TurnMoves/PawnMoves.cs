using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveGenerators.TurnMoves
{
    public class PawnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public PawnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        // TODO: refactor me
        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var potentialMoves = new List<BoardPosition>();

            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return potentialMoves; // allow to change piece

            if (_positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N)), boardState).CurrentPiece
                .Type == PieceType.NullPiece)
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.N)))
                );

            if (originPosition.X > 0)
            {
                var topLeftTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)), boardState);
                if (_tileEvaluator.OpposingPieceIn(topLeftTile))
                    potentialMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NW)))
                    );
            }

            if (originPosition.X < 7)
            {
                var topRightTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)), boardState);
                if (_tileEvaluator.OpposingPieceIn(topRightTile))
                    potentialMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NE)))
                    );
            }

            return potentialMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, PawnMoves>
        {
        }
    }
}