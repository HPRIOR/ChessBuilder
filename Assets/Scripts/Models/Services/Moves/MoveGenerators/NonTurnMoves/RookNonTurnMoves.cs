using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.NonTurnMoves
{
    public class RookNonTurnMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public RookNonTurnMoves(PieceColour pieceColour, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.NonTurn);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            var result = GetMovesIn(Direction.N, relativePosition, boardState).ToList();
            var southMoves = GetMovesIn(Direction.S, relativePosition, boardState).ToList();
            var eastMoves = GetMovesIn(Direction.E, relativePosition, boardState).ToList();
            var westMoves = GetMovesIn(Direction.W, relativePosition, boardState).ToList();

            result.AddRange(southMoves);
            result.AddRange(eastMoves);
            result.AddRange(westMoves);

            return result;
        }

        private IEnumerable<Position> GetMovesIn(Direction d, Position relativePosition, BoardState boardState) =>
            _boardScanner.ScanIn(d, relativePosition, boardState);

        public class Factory : PlaceholderFactory<PieceColour, RookNonTurnMoves>
        {
        }
    }
}