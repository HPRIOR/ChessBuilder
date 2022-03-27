using System.Collections.Generic;
using System.Linq;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public sealed class BoardSetup
    {
        public BoardState SetupBoardWith(IEnumerable<(PieceType piece, Position boardPosition)> pieces)
        {
            var pieceDict = pieces.ToDictionary(x => x.boardPosition, x => x.piece);
            return new BoardState(pieceDict);
        }
    }
}