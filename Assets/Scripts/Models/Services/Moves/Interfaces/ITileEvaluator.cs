using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface ITileEvaluator
    {
        bool OpposingPieceIn(ref Tile tile);

        bool FriendlyPieceIn(ref Tile tile);

        bool NoPieceIn(ref Tile tile);
    }
}