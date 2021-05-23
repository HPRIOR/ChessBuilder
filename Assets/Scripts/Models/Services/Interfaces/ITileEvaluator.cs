using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface ITileEvaluator
    {
        bool OpposingPieceIn(ITile tile);

        bool FriendlyPieceIn(ITile tile);

        bool NoPieceIn(ITile tile);
    }
}