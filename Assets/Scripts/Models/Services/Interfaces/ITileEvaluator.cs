using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface ITileEvaluator
    {
        bool OpposingPieceIn(Tile tile);

        bool FriendlyPieceIn(Tile tile);

        bool NoPieceIn(Tile tile);
    }
}