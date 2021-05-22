using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using UnityEngine;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossibleKnightMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly IBoardMoveEval _boardMoveEval;

        public PossibleKnightMoves(IPositionTranslator positionTranslator, IBoardMoveEval boardMoveEval)
        {
            _positionTranslator = positionTranslator;
            _boardMoveEval = boardMoveEval;
        }

        public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
        {
            bool CoordInBounds((int X, int Y) coord) => 0 <= coord.X && coord.X <= 7 && 0 <= coord.Y && coord.Y <= 7;

            bool FriendlyPieceNotInTile((int X, int Y) coord) =>
                !_boardMoveEval.FriendlyPieceIn(
                    boardState.GetTileAt(_positionTranslator.GetRelativePosition(new BoardPosition(coord.X, coord.Y))));

            var moveCoords = GetMoveCoords(_positionTranslator.GetRelativePosition(originPosition))
                .Where(CoordInBounds)
                .Where(FriendlyPieceNotInTile)
                .Select(coord => new BoardPosition(coord.X, coord.Y))
                .Select(pos => _positionTranslator.GetRelativePosition(pos));

            return moveCoords;
        }

        private static IEnumerable<(int X, int Y)> GetMoveCoords(IBoardPosition boardPosition)
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
    }
}