using System;
using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;
using UnityEngine;

namespace Models.Services.Utils
{
    public static class DirectionCache
    {
        private static readonly Dictionary<(Position p1, Position p2), Direction> Directions;

        static DirectionCache()
        {
            Directions = new Dictionary<(Position p1, Position p2), Direction>();
            var positions = GetPositions();
            for (var index0 = 0; index0 < positions.GetLength(0); index0++)
            for (var index1 = 0; index1 < positions.GetLength(1); index1++)
            {
                var position1 = positions[index0, index1];
                foreach (var position2 in positions)
                    if (position1 != position2)
                        Directions[(position1, position2)] = position1.DirectionTo(position2);
            }
        }


        private static Position[,] GetPositions()
        {
            var positions = new Position[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                positions[i, j] = new Position(i, j);
            return positions;
        }

        public static Direction DirectionFrom(Position origin, Position target) => 
            Directions[(origin, target)];
    }
}