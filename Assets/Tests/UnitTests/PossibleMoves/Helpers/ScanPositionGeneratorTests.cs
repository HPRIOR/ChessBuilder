using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;
using NUnit.Framework;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class ScanPositionGeneratorTests
    {
        [Test]
        public void GetPositionsNorth()
        {
            var init = new Position(1, 1);
            var dest = new Position(1, 5);
            var expected = new List<Position>
            {
                new Position(1, 2),
                new Position(1, 3),
                new Position(1, 4)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsSouth()
        {
            var init = new Position(1, 5);
            var dest = new Position(1, 1);
            var expected = new List<Position>
            {
                new Position(1, 4),
                new Position(1, 3),
                new Position(1, 2)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsEast()
        {
            var init = new Position(1, 1);
            var dest = new Position(5, 1);
            var expected = new List<Position>
            {
                new Position(2, 1),
                new Position(3, 1),
                new Position(4, 1)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsWest()
        {
            var init = new Position(5, 1);
            var dest = new Position(1, 1);
            var expected = new List<Position>
            {
                new Position(4, 1),
                new Position(3, 1),
                new Position(2, 1)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsNorthWest()
        {
            var init = new Position(5, 1);
            var dest = new Position(1, 5);
            var expected = new List<Position>
            {
                new Position(4, 2),
                new Position(3, 3),
                new Position(2, 4)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsNorthEast()
        {
            var init = new Position(1, 2);
            var dest = new Position(5, 6);
            var expected = new List<Position>
            {
                new Position(2, 3),
                new Position(3, 4),
                new Position(4, 5)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsSouthEast()
        {
            var init = new Position(3, 7);
            var dest = new Position(7, 3);
            var expected = new List<Position>
            {
                new Position(4, 6),
                new Position(5, 5),
                new Position(6, 4)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsSouthWest()
        {
            var init = new Position(5, 7);
            var dest = new Position(1, 3);
            var expected = new List<Position>
            {
                new Position(4, 6),
                new Position(3, 5),
                new Position(2, 4)
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AdjacentPointsReturnEmptyList([Values(3, 4)] int x, [Values(3, 4)] int y,
            [Values(0, 1, -1)] int xMod, [Values(-1, 1, 0)] int yMod
        )
        {
            var init = new Position(x, y);
            var dest = new Position(x + xMod, y + yMod);

            var expected = new List<Position>();
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SamePositionReturnsEmptyList()
        {
            var init = new Position(1, 1);
            var dest = new Position(1, 1);
            var expected = new List<Position>();
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }
    }
}