using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveHelpers
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
        /// <returns></returns>
        public IEnumerable<Position> ScanIn(Direction direction, Position currentPosition,
            BoardState boardState)
        {
            var newPosition = currentPosition.Add(Move.In(direction));
            if (PieceCannotMoveTo(newPosition, boardState))
                return new List<Position>();
            if (TileContainsOpposingPieceAt(newPosition, boardState))
                return new List<Position> {_positionTranslator.GetRelativePosition(newPosition)};
            return ScanIn(direction, newPosition, boardState)
                .Concat(new List<Position> {_positionTranslator.GetRelativePosition(newPosition)});
        }

        private bool PieceCannotMoveTo(Position position, BoardState boardState)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7 || TileContainsFriendlyPieceAt(position, boardState);
        }

        // TODO refactor so that position translator result is passed through instead of calculated each time
        private bool TileContainsOpposingPieceAt(Position position, BoardState boardState) =>
            _tileEvaluator.OpposingPieceIn(_positionTranslator.GetRelativeTileAt(position, boardState));

        private bool TileContainsFriendlyPieceAt(Position position, BoardState boardState) =>
            _tileEvaluator.FriendlyPieceIn(_positionTranslator.GetRelativeTileAt(position, boardState));

        public class Factory : PlaceholderFactory<PieceColour, BoardScanner>
        {
        }
    }
}