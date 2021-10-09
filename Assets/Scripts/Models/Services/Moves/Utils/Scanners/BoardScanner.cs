using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils.Scanners
{
    public sealed class BoardScanner : IBoardScanner
    {
        private readonly ITileEvaluator _tileEvaluator;
        private readonly PieceColour _turn;

        public BoardScanner(
            PieceColour pieceColour,
            ITileEvaluatorFactory tileEvaluatorFactory
        )
        {
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
            _turn = pieceColour;
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
        public void ScanIn(Direction direction,
            Position currentPosition, // make me return Span instead of possible moves
            BoardState boardState, List<Position> possibleMoves)
        {
            var possibleMovePositions = _turn == PieceColour.White
                ? ScanCache.GetPositionsToEndOfBoard(currentPosition, direction)
                : RelativePositionScanCache.GetPositionsToEndOfBoard(currentPosition, direction);

            for (var i = 0; i < possibleMovePositions.Length; i++)
            {
                var position = possibleMovePositions[i];
                if (TileContainsFriendlyPieceAt(position, boardState)) break;
                possibleMoves.Add(position);
                if (TileContainsOpposingPieceAt(position, boardState)) break;
            }
        }

        private bool TileContainsOpposingPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.OpposingPieceIn(ref boardState.GetTileAt(relativePosition));

        private bool TileContainsFriendlyPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.FriendlyPieceIn(ref boardState.GetTileAt(relativePosition));

        public sealed class Factory : PlaceholderFactory<PieceColour, BoardScanner>
        {
        }
    }
}