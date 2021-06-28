using Models.State.Board;
using Models.State.Interfaces;

namespace View.Utils.Prefab.Interfaces
{
    public interface IPieceSpawner
    {
        public Position Position { get; }
        public IPieceRenderInfo RenderInfo { get; }
    }
}