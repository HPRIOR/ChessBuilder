using Models.Services.Board;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public class PieceMover : IPieceMover
    {
        private readonly BuildStateDecrementor _buildStateDecrementor;

        public PieceMover(BuildStateDecrementor buildStateDecrementor)
        {
            _buildStateDecrementor = buildStateDecrementor;
        }

        public void ModifyBoardState(BoardState boardState, Position from,
            Position destination)
        {
            // modify active pieces 
            boardState.ActivePieces.Remove(from);
            boardState.ActivePieces.Add(destination);

            _buildStateDecrementor.DecrementBuilds(boardState);


            // modify board state
            var destinationTile = boardState.Board[destination.X, destination.Y];
            var fromTile = boardState.Board[from.X, from.Y];

            // swap pieces
            destinationTile.CurrentPiece = fromTile.CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);

            // return nothing 
        }
    }
}