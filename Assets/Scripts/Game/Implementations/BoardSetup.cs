using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Game.Implementations
{
    public class BoardSetup
    {
        private readonly IBoardGenerator _boardGenerator;

        public BoardSetup(IBoardGenerator boardGenerator)
        {
            _boardGenerator = boardGenerator;
        }

        public BoardState SetupBoard(IEnumerable<(PieceType piece, BoardPosition boardPosition)> pieces)
        {
            var boardState = new BoardState(_boardGenerator.GenerateBoard());
            var board = boardState.Board;
            pieces.ToList().ForEach(tup =>
                board[tup.boardPosition.X, tup.boardPosition.Y].CurrentPiece = new Piece(tup.piece));
            return boardState;
        }
    }
}