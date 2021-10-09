using Models.Services.AI.Interfaces;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.AI.Implementations
{
    public sealed class StaticEvaluator : IStaticEvaluator
    {
        public BoardScore Evaluate(GameState gameState)
        {
            var blackPoints = 0;
            var whitePoints = 0;
            var activePieces = gameState.BoardState.ActivePieces;
            for (var index = 0; index < activePieces.Count; index++)
            {
                var position = activePieces[index];
                ref var tile = ref gameState.BoardState.GetTileAt(position);
                var currentPiece = tile.CurrentPiece;
                var multiplier = 100;
                if (currentPiece.Colour() == PieceColour.Black)
                    blackPoints += currentPiece.Value() * multiplier;
                else
                    whitePoints += currentPiece.Value() * multiplier;
            }

            return new BoardScore(blackPoints, whitePoints);
        }
    }
}