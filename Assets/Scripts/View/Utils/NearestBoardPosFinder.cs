using Models.State.Board;
using UnityEngine;

namespace View.Utils
{
    public static class NearestBoardPosFinder
    {
        public static Position GetNearestBoardPosition(Vector2 position) =>
            new(ConvertAxisToNearestBoardIndex(position.x),
                ConvertAxisToNearestBoardIndex(position.y));

        private static int ConvertAxisToNearestBoardIndex(float axis)
        {
            if (axis > 7.5) return 7;
            if (axis < 0.5) return 0;
            return (int)axis;
        }
    }
}