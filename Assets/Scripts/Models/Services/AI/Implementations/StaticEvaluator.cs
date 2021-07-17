using Models.Services.AI.Interfaces;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public class StaticEvaluator : IStaticEvaluator
    {
        public BoardScore Evaluate(GameState gameState)
        {
            var blackPoints = 0;
            var whitePoints = 0;
            var board = gameState.BoardState.Board;
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                var multiplier = 100;
                if (currentPiece.Colour == PieceColour.Black)
                    blackPoints += BuildPoints.PieceCost[currentPiece.Type] * multiplier;
                else
                    whitePoints += BuildPoints.PieceCost[currentPiece.Type] * multiplier;
            }

            return new BoardScore(blackPoints, whitePoints);
        }
    }
}