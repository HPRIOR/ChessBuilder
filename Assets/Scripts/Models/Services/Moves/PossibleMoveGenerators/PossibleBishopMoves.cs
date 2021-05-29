using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossibleBishopMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public PossibleBishopMoves(PieceColour pieceColour, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, ScannerType.Normal);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);
            var possibleDirections = new List<Direction> {Direction.NE, Direction.NW, Direction.SE, Direction.SW};

            return possibleDirections.SelectMany(direction =>
                _boardScanner.ScanIn(direction, relativePosition, boardState));
        }

        public class Factory : PlaceholderFactory<PieceColour, PossibleBishopMoves>
        {
        }
    }
}