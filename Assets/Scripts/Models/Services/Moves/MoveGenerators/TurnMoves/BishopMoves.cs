using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class BishopMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public BishopMoves(PieceColour pieceColour, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.Turn);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            var result = GetMovesIn(Direction.NE, relativePosition, boardState).ToList();
            var northWestMoves = GetMovesIn(Direction.NW, relativePosition, boardState).ToList();
            var southEastMoves = GetMovesIn(Direction.SE, relativePosition, boardState).ToList();
            var southWestMoves = GetMovesIn(Direction.SW, relativePosition, boardState).ToList();

            result.AddRange(northWestMoves);
            result.AddRange(southEastMoves);
            result.AddRange(southWestMoves);
            return result;
        }

        private IEnumerable<Position> GetMovesIn(Direction d, Position relativePosition, BoardState boardState) =>
            _boardScanner.ScanIn(d, relativePosition, boardState);

        public class Factory : PlaceholderFactory<PieceColour, BishopMoves>
        {
        }
    }
}