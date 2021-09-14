using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Build.BuildMoves
{
    public class Builder : IBuilder
    {
        public void GenerateNewBoardState(BoardState boardState, Position buildPosition, PieceType piece)
        {
            // add build positions to active pieces 
            boardState.ActiveBuilds.Add(buildPosition);

            if (piece.Colour() == PieceColour.Black)
                boardState.ActiveBlackBuilds.Add(buildPosition);
            if (piece.Colour() == PieceColour.White)
                boardState.ActiveWhiteBuilds.Add(buildPosition);

            // modify board state 
            boardState.Board[buildPosition.X, buildPosition.Y].BuildTileState = new BuildTileState(piece);
        }
    }
}