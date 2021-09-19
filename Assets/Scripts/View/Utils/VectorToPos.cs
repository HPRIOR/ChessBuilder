using Models.State.Board;
using UnityEngine;

namespace View.Utils
{
    public static class VectorToPos
    {
        public static Vector2 GetVector(Position pos) => new Vector2(pos.X + 0.5f, pos.Y + 0.5f);
    }
}