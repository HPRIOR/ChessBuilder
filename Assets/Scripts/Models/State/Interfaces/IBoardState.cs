using System;

namespace Models.State.Interfaces
{
    public interface IBoardState : ICloneable
    {
        ITile[,] Board { get; }

        ITile GetTileAt(IBoardPosition boardPosition);

        ITile GetMirroredTileAt(IBoardPosition boardPosition);
    }
}