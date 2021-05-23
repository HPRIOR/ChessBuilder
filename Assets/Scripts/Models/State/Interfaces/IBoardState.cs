using System;

namespace Models.State.Interfaces
{
    public interface IBoardState : ICloneable
    {
        ITile[,] Board { get; }
    }
}