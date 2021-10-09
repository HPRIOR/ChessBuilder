using System.Collections.Generic;
using System.Linq;
using Models.Services.Board;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public sealed class BoardSetup
    {
        private readonly IBoardGenerator _boardGenerator;

        public BoardSetup(IBoardGenerator boardGenerator)
        {
            _boardGenerator = boardGenerator;
        }

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