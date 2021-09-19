using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils.Scanners
{
    public class BoardScanner : IBoardScanner
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public BoardScanner(
            PieceColour pieceColour,
            ITileEvaluatorFactory tileEvaluatorFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     More confusing stuff due to the mirroring of black and white piece logic. Scanner will scan as though
        ///     the board perspective is white, then evaluate based on the colour given e.g. flip the board and return
        ///     the 'opposite' tile.
        /// </remarks>
        /// <param name="direction"></param>
        /// <param name="currentPosition"></param>
        /// <param name="boardState"></param>
        /// <param name="possibleMoves"></param>
        /// <returns></returns>
        public void ScanIn(Direction direction, Position currentPosition,
            BoardState boardState, List<Position> possibleMoves)
        {
            var iteratingPosition = currentPosition;


            while (true)
            {
                var newPosition = iteratingPosition.Add(Move.In(direction));
                var relativePosition = _positionTranslator.GetRelativePosition(newPosition);

                if (PieceCannotMoveTo(newPosition) || TileContainsFriendlyPieceAt(relativePosition, boardState)) break;
                if (TileContainsOpposingPieceAt(relativePosition, boardState))
                {
                    possibleMoves.Add(relativePosition);
                    break;
                }

                possibleMoves.Add(relativePosition);
                iteratingPosition = newPosition;
            }
        }

        private static bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }

        private bool TileContainsOpposingPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.OpposingPieceIn(boardState.Board[relativePosition.X, relativePosition.Y]);

        private bool TileContainsFriendlyPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.FriendlyPieceIn(boardState.Board[relativePosition.X, relativePosition.Y]);

        public class Factory : PlaceholderFactory<PieceColour, BoardScanner>
        {
        }
    }
}