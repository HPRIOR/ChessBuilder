using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controllers.Moves.PossibleMoveHelpers
{
    public static class ScanPositionGenerator
    {
        public static IEnumerable<IBoardPosition> GetPositionsBetween(IBoardPosition init, IBoardPosition dest)
        {
            if (init.Equals(dest))
                return new List<IBoardPosition>();
            else if (init.X == dest.X && init.Y != dest.Y)
                return GetExlusivePositionsWithConstant(init.Y, dest.Y, init.X, true);
            else if (init.Y == dest.Y && init.X != dest.X)
                return GetExlusivePositionsWithConstant(init.X, dest.X, init.Y, false);
            else
            {
                var xs = GetExlusiveValuesAccordingToPosition(init.X, dest.X, GetValues);
                var ys = GetExlusiveValuesAccordingToPosition(init.Y, dest.Y, GetValues);
                return xs
                    .Zip(ys, (x, y) => new BoardPosition(x, y))
                    .Cast<IBoardPosition>()
                    .ToList();
            }
        }

        private static IEnumerable<IBoardPosition> GetExlusivePositionsWithConstant(int init, int dest, int constant, bool xConstant)
        {
            Func<int, int, IBoardPosition> xConstantFunc = (y, x) => new BoardPosition(x, y);
            Func<int, int, IBoardPosition> yConstantFunc = (x, y) => new BoardPosition(x, y);
            var constantList = Enumerable.Range(0, Math.Abs(init - dest)).Select(x => constant).ToList();
            return
                GetExlusiveValuesAccordingToPosition(init, dest, GetValues)
                .Zip(constantList, xConstant ? xConstantFunc : yConstantFunc)
                .Cast<IBoardPosition>()
                .ToList();
        }


        private static IEnumerable<int> GetValues(int init, int dest)
        {
            if (init == dest) return new List<int>();
            return init > dest
                ? GetValues(init, dest + 1).Concat(new List<int>() { dest })
                : GetValues(init, dest - 1).Concat(new List<int>() { dest });
        }

        private static IEnumerable<int> GetExlusiveValuesAccordingToPosition(
            int init, int dest,
            Func<int, int, IEnumerable<int>> GetValueFunc) =>
            init > dest ? GetValueFunc(init, dest + 1) : GetValueFunc(init, dest - 1);

    }
}
