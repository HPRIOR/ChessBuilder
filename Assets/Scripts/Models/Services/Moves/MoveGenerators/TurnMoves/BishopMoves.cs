using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class BishopMoves : IPieceMoveGenerator
    {
        private static readonly Direction[] Directions =
            { Direction.NE, Direction.NW, Direction.SE, Direction.SW };

        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public BishopMoves(PieceColour pieceColour, IBoardScannerFactory boardScannerFactory,
            IPositionTranslatorFactory positionTranslatorFactory)
        {
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.Turn);
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
        }

        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            var possibleMoves = new List<Position>();

            foreach (var direction in Directions)
                _boardScanner.ScanIn(direction, relativePosition, boardState, possibleMoves);

            return possibleMoves;
        }

        public class Factory : PlaceholderFactory<PieceColour, BishopMoves>
        {
        }
    }
}