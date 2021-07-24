using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class KnightMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public KnightMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            bool CoordInBounds((int X, int Y) coord) => 0 <= coord.X && coord.X <= 7 && 0 <= coord.Y && coord.Y <= 7;

            bool FriendlyPieceNotInTile((int X, int Y) coord) =>
                !_tileEvaluator.FriendlyPieceIn(
                    _positionTranslator.GetRelativeTileAt(new Position(coord.X, coord.Y), boardState));

            var possibleMoveCoords = GetMoveCoords(_positionTranslator.GetRelativePosition(originPosition));
            var result = new List<Position>();
            foreach (var possibleMoveCoord in possibleMoveCoords)
                if (CoordInBounds(possibleMoveCoord) && FriendlyPieceNotInTile(possibleMoveCoord))
                    result.Add(_positionTranslator.GetRelativePosition(
                        new Position(possibleMoveCoord.X, possibleMoveCoord.Y))
                    );
            return result;
        }

        private static IEnumerable<(int X, int Y)> GetMoveCoords(Position position)
        {
            var x = position.X;
            var y = position.Y;

            var lateralRightUp = (x + 2, y + 1);
            var lateralRightDown = (x + 2, y - 1);

            var lateralLeftUp = (x - 2, y + 1);
            var lateralLeftDown = (x - 2, y - 1);

            var verticalTopRight = (x + 1, y + 2);
            var verticalTopLeft = (x - 1, y + 2);

            var verticalBottomRight = (x + 1, y - 2);
            var verticalBottomLeft = (x - 1, y - 2);

            return new List<(int X, int Y)>
            {
                lateralRightUp,
                lateralRightDown,
                lateralLeftUp,
                lateralLeftDown,
                verticalTopRight,
                verticalTopLeft,
                verticalBottomRight,
                verticalBottomLeft
            };
        }

        public class Factory : PlaceholderFactory<PieceColour, KnightMoves>
        {
        }
    }
}