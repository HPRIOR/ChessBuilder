using Models.State.Board;
using View.Interfaces;

namespace View.Prefab.Interfaces
{
    public interface IPieceSpawner
    {
        public Position Position { get; }
        public IPieceRenderInfo RenderInfo { get; }
    }
}