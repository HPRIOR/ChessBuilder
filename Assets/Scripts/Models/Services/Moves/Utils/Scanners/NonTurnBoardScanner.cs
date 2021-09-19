﻿using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils.Scanners
{
    public class NonTurnBoardScanner : IBoardScanner
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;
        private readonly PieceColour _turn;

        public NonTurnBoardScanner(
            PieceColour pieceColour,
            ITileEvaluatorFactory tileEvaluatorFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
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
        public void ScanIn(Direction direction, Position currentPosition,
            BoardState boardState, List<Position> possibleMoves)
        {
            var possibleMovePositions = _turn == PieceColour.White
                ? ScanCache.GetPositionsToEndOfBoard(currentPosition, direction)
                : RelativePositionScanCache.GetPositionsToEndOfBoard(currentPosition, direction);

            for (var i = 0; i < possibleMovePositions.Length; i++)
            {
                var position = possibleMovePositions[i];
                possibleMoves.Add(position);
                if (TileContainsFriendlyPieceAt(position, boardState) ||
                    TileContainsOpposingPieceAt(position, boardState)) break;
            }
        }

        private bool TileContainsOpposingPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.OpposingPieceIn(boardState.Board[relativePosition.X][relativePosition.Y]);

        private bool TileContainsFriendlyPieceAt(Position relativePosition, BoardState boardState) =>
            _tileEvaluator.FriendlyPieceIn(boardState.Board[relativePosition.X][relativePosition.Y]);

        public class Factory : PlaceholderFactory<PieceColour, NonTurnBoardScanner>
        {
        }
    }
}