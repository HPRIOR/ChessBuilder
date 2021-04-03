using System;

public interface IBoardState : ICloneable
{
    ITile[,] Board { get; }
    ITile[,] MirroredBoard { get; }
    ITile GetTileAt(IBoardPosition boardPosition);

    ITile GetMirroredTileAt(IBoardPosition boardPosition);
}