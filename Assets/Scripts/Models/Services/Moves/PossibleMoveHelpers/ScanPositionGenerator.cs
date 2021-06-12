using System;
using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public static class ScanPositionGenerator
    {
        public static IEnumerable<BoardPosition> GetPositionsBetween(BoardPosition init, BoardPosition dest)
        {
            if (init.Equals(dest))
                return new List<BoardPosition>();
            if (init.X == dest.X && init.Y != dest.Y)
                return GetExclusivePositionsWithConstant(init.Y, dest.Y, init.X, true);
            if (init.Y == dest.Y && init.X != dest.X)
                return GetExclusivePositionsWithConstant(init.X, dest.X, init.Y, false);
            var xs = GetExclusiveValuesAccordingToPosition(init.X, dest.X, GetValues);
            var ys = GetExclusiveValuesAccordingToPosition(init.Y, dest.Y, GetValues);
            return xs
                .Zip(ys, (x, y) => new BoardPosition(x, y))
                .ToList();
        }

        private static IEnumerable<BoardPosition> GetExclusivePositionsWithConstant(int init, int dest, int constant,
            bool xConstant)
        {
            static BoardPosition XConstantFunc(int y, int x) => new BoardPosition(x, y);

            static BoardPosition YConstantFunc(int x, int y) => new BoardPosition(x, y);

            var constantList = Enumerable.Range(0, Math.Abs(init - dest)).Select(x => constant).ToList();
            return
                GetExclusiveValuesAccordingToPosition(init, dest, GetValues)
                    .Zip(constantList, xConstant ? (Func<int, int, BoardPosition>) XConstantFunc : YConstantFunc)
                    .ToList();
        }


        private static IEnumerable<int> GetValues(int init, int dest)
        {
            if (init == dest) return new List<int>();
            return init > dest
                ? GetValues(init, dest + 1).Concat(new List<int> {dest})
                : GetValues(init, dest - 1).Concat(new List<int> {dest});
        }

        private static IEnumerable<int> GetExclusiveValuesAccordingToPosition(
            int init, int dest,
            Func<int, int, IEnumerable<int>> getValueFunc) =>
            init > dest ? getValueFunc(init, dest + 1) : getValueFunc(init, dest - 1);
    }
}