using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils
{
    public class NonTurnBoardScanner : IBoardScanner
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public NonTurnBoardScanner(
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
        /// <returns></returns>
        public IEnumerable<Position> ScanIn(Direction direction, Position currentPosition,
            BoardState boardState)
        {
            var result = new List<Position>();
            var iteratingPosition = currentPosition;

            while (true)
            {
                var newPosition = iteratingPosition.Add(Move.In(direction));
                if (PieceCannotMoveTo(newPosition)) break;
                if (TileContainsOpposingPieceAt(newPosition, boardState) ||
                    TileContainsFriendlyPieceAt(newPosition, boardState))
                {
                    result.Add(_positionTranslator.GetRelativePosition(newPosition));
                    break;
                }

                result.Add(_positionTranslator.GetRelativePosition(newPosition));
                iteratingPosition = newPosition;
            }

            return result;
        }

        private bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }

        // TODO refactor so that position translator result is passed through instead of calculated each time
        private bool TileContainsOpposingPieceAt(Position position, BoardState boardState) =>
            _tileEvaluator.OpposingPieceIn(_positionTranslator.GetRelativeTileAt(position, boardState));

        private bool TileContainsFriendlyPieceAt(Position position, BoardState boardState) =>
            _tileEvaluator.FriendlyPieceIn(_positionTranslator.GetRelativeTileAt(position, boardState));

        public class Factory : PlaceholderFactory<PieceColour, NonTurnBoardScanner>
        {
        }
    }
}