using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.Services.Build.BuildMoves
{
    public sealed class Builder : IBuilder
    {
        public void GenerateNewBoardState(BoardState boardState, Position buildPosition, PieceType piece)
        {
            // add build positions to active pieces 
            boardState.ActiveBuilds.Add(buildPosition);

            // modify board state 
            boardState.Board[buildPosition.X][buildPosition.Y].BuildTileState = new BuildTileState(piece);
        }
    }
}