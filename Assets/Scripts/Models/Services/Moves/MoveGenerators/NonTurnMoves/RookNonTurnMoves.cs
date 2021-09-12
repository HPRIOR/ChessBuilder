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
        private static readonly Direction[] PossibleDirections = { Direction.N, Direction.E, Direction.S, Direction.W };
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
            return PossibleDirections.SelectMany(direction =>
                _boardScanner.ScanIn(direction, relativePosition, boardState));
        }

        public class Factory : PlaceholderFactory<PieceColour, RookNonTurnMoves>
        {
        }
    }
}