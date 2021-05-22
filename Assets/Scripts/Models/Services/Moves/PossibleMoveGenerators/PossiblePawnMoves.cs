using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossiblePawnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly IBoardMoveEval _boardMoveEval;

        public PossiblePawnMoves(IPositionTranslator boardPositionTranslator, IBoardMoveEval boardMoveEval)
        {
            _positionTranslator = boardPositionTranslator;
            _boardMoveEval = boardMoveEval;
        }

        public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
        {
            var potentialMoves = new List<IBoardPosition>();

            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return potentialMoves; // allow to change piece

            if (_positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N)), boardState).CurrentPiece.Type == PieceType.NullPiece)
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.N)))
                );

            if (originPosition.X > 0)
            {
                var topLeftTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)), boardState);
                if (_boardMoveEval.OpposingPieceIn(topLeftTile))
                    potentialMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NW)))
                    );
            }

            if (originPosition.X < 7)
            {
                var topRightTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)), boardState);
                if (_boardMoveEval.OpposingPieceIn(topRightTile))
                    potentialMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NE)))
                    );
            }

            return potentialMoves;
        }
    }
}