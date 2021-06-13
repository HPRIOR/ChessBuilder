using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class KnightNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;

        public KnightNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            bool CoordInBounds((int X, int Y) coord) => 0 <= coord.X && coord.X <= 7 && 0 <= coord.Y && coord.Y <= 7;

            var moveCoords = GetMoveCoords(_positionTranslator.GetRelativePosition(originPosition))
                .Where(CoordInBounds)
                .Select(coord => new BoardPosition(coord.X, coord.Y))
                .Select(pos => _positionTranslator.GetRelativePosition(pos));

            return moveCoords;
        }

        private static IEnumerable<(int X, int Y)> GetMoveCoords(BoardPosition boardPosition)
        {
            var x = boardPosition.X;
            var y = boardPosition.Y;
            var squareXs = new List<int> {x + 2, x - 2};
            var squareYs = new List<int> {y + 2, y - 2};

            var lateralMoves =
                squareXs.SelectMany(
                    x => Enumerable
                        .Range(0, 2)
                        .Select(num => num == 0 ? (x, y + 1) : (x, y - 1))
                );

            var verticalMoves
                = squareYs.SelectMany(
                    y => Enumerable
                        .Range(0, 2)
                        .Select(num => num == 0 ? (x + 1, y) : (x - 1, y))
                );

            return lateralMoves.Concat(verticalMoves);
        }

        public class Factory : PlaceholderFactory<PieceColour, KnightNonTurnMoves>
        {
        }
    }
}