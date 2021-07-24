using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.NonTurnMoves
{
    public class QueenNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public QueenNonTurnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            IBoardScannerFactory boardScannerFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.NonTurn);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            var result = GetMovesIn(Direction.N, relativePosition, boardState).ToList();
            var southMoves = GetMovesIn(Direction.S, relativePosition, boardState).ToList();
            var eastMoves = GetMovesIn(Direction.E, relativePosition, boardState).ToList();
            var westMoves = GetMovesIn(Direction.W, relativePosition, boardState).ToList();
            var northWestMoves = GetMovesIn(Direction.NW, relativePosition, boardState).ToList();
            var northEastMoves = GetMovesIn(Direction.NE, relativePosition, boardState).ToList();
            var southWestMoves = GetMovesIn(Direction.SW, relativePosition, boardState).ToList();
            var southEastMoves = GetMovesIn(Direction.SE, relativePosition, boardState).ToList();
            result.AddRange(southMoves);
            result.AddRange(eastMoves);
            result.AddRange(westMoves);
            result.AddRange(northWestMoves);
            result.AddRange(northEastMoves);
            result.AddRange(southWestMoves);
            result.AddRange(southEastMoves);
            return result;
        }

        private IEnumerable<Position> GetMovesIn(Direction d, Position relativePosition, BoardState boardState) =>
            _boardScanner.ScanIn(d, relativePosition, boardState);

        public class Factory : PlaceholderFactory<PieceColour, QueenNonTurnMoves>
        {
        }
    }
}