using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Controllers.PieceMovers
{
    public class PieceMover : IPieceMover
    {
        private readonly IBuildResolver _buildResolver;

        public PieceMover(IBuildResolver buildResolver)
        {
            _buildResolver = buildResolver;
        }

        public BoardState GenerateNewBoardState(BoardState originalBoardState, Position from,
            Position destination)
        {
            var newBoardState = originalBoardState.CloneWithDecrementBuildState();

            // get swapped pieces
            var destinationTile = newBoardState.Board[destination.X, destination.Y];
            var fromTile = newBoardState.Board[from.X, from.Y];

            // swap pieces
            destinationTile.CurrentPiece = originalBoardState.Board[from.X, from.Y].CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);
            _buildResolver.ResolveBuild(newBoardState);

            return newBoardState;
        }
    }
}