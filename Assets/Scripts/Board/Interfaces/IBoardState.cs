public interface IBoardState
{
    ITile GetTileAt(IBoardPosition boardPosition);

    ITile GetMirroredTileAt(IBoardPosition boardPosition);
}