using Models.Services.Board;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.Services.Build.BuildMoves
{
    public class Builder : IBuilder
    {
        private readonly BuildStateDecrementor _buildStateDecrementor;

        public Builder(BuildStateDecrementor buildStateDecrementor)
        {
            _buildStateDecrementor = buildStateDecrementor;
        }

        public void GenerateNewBoardState(BoardState boardState, Position buildPosition, PieceType piece)
        {
            // add build positions to active pieces 
            boardState.ActiveBuilds.Add(buildPosition);

            // decrement builds 
            _buildStateDecrementor.DecrementBuilds(boardState);

            // modify board state 
            boardState.Board[buildPosition.X, buildPosition.Y].BuildTileState = new BuildTileState(piece);
        }
    }
}