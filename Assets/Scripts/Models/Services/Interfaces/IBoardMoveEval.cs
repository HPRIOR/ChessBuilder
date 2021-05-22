using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IBoardMoveEval
    {
        bool OpposingPieceIn(ITile tile);

        bool FriendlyPieceIn(ITile tile);

        bool NoPieceIn(ITile tile);
    }
}