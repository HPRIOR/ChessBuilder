using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.NonTurnMoves
{
    public class BishopNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public BishopNonTurnMoves(PieceColour pieceColour, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.NonTurn);
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


        public class Factory : PlaceholderFactory<PieceColour, BishopNonTurnMoves>
        {
        }
    }
}