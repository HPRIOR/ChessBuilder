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
            var boardState = new BoardState();
            var board = boardState.Board;
            pieces.ToList().ForEach(tup =>
                board[tup.boardPosition.X][tup.boardPosition.Y].CurrentPiece = tup.piece);
            return boardState;
        }
    }
}