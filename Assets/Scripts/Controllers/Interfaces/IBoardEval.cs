public interface IBoardEval
{
    bool OpposingPieceIn(ITile tile);

    bool FriendlyPieceIn(ITile tile);

    bool NoPieceIn(ITile tile);
}