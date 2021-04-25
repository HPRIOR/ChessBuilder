using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Interfaces;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossibleBishopMoves : IPieceMoveGenerator
    {
        private readonly IBoardScanner _boardScanner;
        private readonly IPositionTranslator _positionTranslator;

        public PossibleBishopMoves(IBoardScanner boardScanner, IPositionTranslator positionTranslator)
        {
            _boardScanner = boardScanner;
            _positionTranslator = positionTranslator;
        }

        public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
        {
            var relativePosition = _positionTranslator.GetRelativePosition(originPosition);
            var possibleDirections = new List<Direction>() { Direction.NE, Direction.NW, Direction.SE, Direction.SW };

            return possibleDirections.SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition, boardState));
        }
    }
}