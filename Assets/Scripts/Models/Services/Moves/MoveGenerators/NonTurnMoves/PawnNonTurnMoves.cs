using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.NonTurnMoves
{
    public sealed class PawnNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;

        public PawnNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var possibleMoves = new List<Position>();
            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return possibleMoves;

            if (originPosition.X > 0)
            {
                var topLeftTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.Nw)), boardState);

                possibleMoves.Add(topLeftTile.Position);
            }

            if (originPosition.X < 7)
            {
                var topRightTile =
                    _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.Ne)), boardState);
                possibleMoves.Add(topRightTile.Position);
            }

            return possibleMoves;
        }


        public class Factory : PlaceholderFactory<PieceColour, PawnNonTurnMoves>
        {
        }
    }
}