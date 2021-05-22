using System.Collections.Generic;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.Interfaces;
using NUnit.Framework;

namespace Tests.UnitTests.PieceMoveTests.Helpers
{
    [TestFixture]
    public class ScanPositionGeneratorTests
    {

        [Test]
        public void GetPositionsNorth()
        {
            var init = new BoardPosition(1, 1);
            var dest = new BoardPosition(1, 5);
            var expected = new List<BoardPosition>() {
                new BoardPosition(1,2),
                new BoardPosition(1,3),
                new BoardPosition(1,4),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void GetPositionsSouth()
        {

            var init = new BoardPosition(1, 5);
            var dest = new BoardPosition(1, 1);
            var expected = new List<BoardPosition>() {
                new BoardPosition(1,4),
                new BoardPosition(1,3),
                new BoardPosition(1,2),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void GetPositionsEast()
        {
            var init = new BoardPosition(1, 1);
            var dest = new BoardPosition(5, 1);
            var expected = new List<BoardPosition>() {
                new BoardPosition(2, 1),
                new BoardPosition(3,1),
                new BoardPosition(4,1),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsWest()
        {
            var init = new BoardPosition(5, 1);
            var dest = new BoardPosition(1, 1);
            var expected = new List<BoardPosition>() {
                new BoardPosition(4,1),
                new BoardPosition(3,1),
                new BoardPosition(2, 1),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void GetPositionsNorthWest()
        {

            var init = new BoardPosition(5, 1);
            var dest = new BoardPosition(1, 5);
            var expected = new List<BoardPosition>() {
                new BoardPosition(4,2),
                new BoardPosition(3,3),
                new BoardPosition(2,4),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void GetPositionsNorthEast()
        { 
            var init = new BoardPosition(1, 2);
            var dest = new BoardPosition(5, 6);
            var expected = new List<BoardPosition>() {
                new BoardPosition(2,3),
                new BoardPosition(3,4),
                new BoardPosition(4,5),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionsSouthEast()
        {
       
            var init = new BoardPosition(3, 7);
            var dest = new BoardPosition(7,3);
            var expected = new List<BoardPosition>() {
                new BoardPosition(4,6),
                new BoardPosition(5,5),
                new BoardPosition(6,4),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void GetPositionsSouthWest()
        {
            var init = new BoardPosition(5, 7);
            var dest = new BoardPosition(1,3);
            var expected = new List<BoardPosition>() {
                new BoardPosition(4,6),
                new BoardPosition(3,5),
                new BoardPosition(2,4),
            };
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void AdjacentPointsReturnEmptyList([Values(3,4)] int x, [Values(3,4)]int y,
            [Values(0,1,-1)] int xMod, [Values(-1, 1, 0)] int yMod 
        )
        {
            var init = new BoardPosition(x, y);
            var dest = new BoardPosition(x + xMod, y + yMod);

            var expected = new List<BoardPosition>();
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SamePositionReturnsEmptyList()
        {
            var init = new BoardPosition(1, 1);
            var dest = new BoardPosition(1,1);
            var expected = new List<IBoardPosition>();
            var actual = ScanPositionGenerator.GetPositionsBetween(init, dest);
            Assert.AreEqual(expected, actual);
        }
    }
}