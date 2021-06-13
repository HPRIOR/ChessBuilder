﻿using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
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

            var moveCoords = GetMoveCoords(_positionTranslator.GetRelativePosition(originPosition))
                .Where(CoordInBounds)
                .Where(FriendlyPieceNotInTile)
                .Select(coord => new Position(coord.X, coord.Y))
                .Select(pos => _positionTranslator.GetRelativePosition(pos));

            return moveCoords;
        }

        private static IEnumerable<(int X, int Y)> GetMoveCoords(Position position)
        {
            var x = position.X;
            var y = position.Y;
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

        public class Factory : PlaceholderFactory<PieceColour, KnightMoves>
        {
        }
    }
}