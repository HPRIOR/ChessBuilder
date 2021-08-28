using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;

namespace Models.Services.Utils
{
    public static class DirectionMap
    {
        private static readonly Dictionary<(Position p1, Position p2), Direction> Directions;

        static DirectionMap()
        {
            Directions = new Dictionary<(Position p1, Position p2), Direction>();
            var positions = GetPositions();
            foreach (var position1 in positions)
            foreach (var position2 in positions)
                if (position1 != position2)
                    Directions[(position1, position2)] = position1.DirectionTo(position2);
        }


        private static Position[,] GetPositions()
        {
            var positions = new Position[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                positions[i, j] = new Position(i, j);
            return positions;
        }

        public static Direction DirectionFrom(Position origin, Position target) => Directions[(origin, target)];
    }
}