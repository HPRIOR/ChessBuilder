using Models.State.Board;
using UnityEngine;

namespace View.Utils
{
    public static class PosToVector
    {
        public static Vector2 GetVector(this Position pos) => new(pos.X + 0.5f, pos.Y + 0.5f);
    }
}