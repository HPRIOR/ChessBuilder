using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public sealed class QueenMoves : IPieceMoveGenerator
    {
        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W, Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public QueenMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            IBoardScannerFactory boardScannerFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _boardScanner = boardScannerFactory.Create(pieceColour, Turn.Turn);
        }

        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

            var possibleMoves = new List<Position>();

            for (var index = 0; index < Directions.Length; index++)
            {
                var direction = Directions[index];
                _boardScanner.ScanIn(direction, relativePosition, boardState, possibleMoves);
            }

            return possibleMoves;
        }

        public sealed class Factory : PlaceholderFactory<PieceColour, QueenMoves>
        {
        }
    }
}