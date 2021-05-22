using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.BoardTests
{
    [TestFixture]
    public class BoardPositionTests : ZenjectUnitTestFixture
    {
        [Test]
        public void AddNorth()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(0, 1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(1, 2), boardPositionSum);
        }
    
        [Test]
        public void AddSouth()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(0, -1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(1, 0), boardPositionSum);
        }

        [Test]
        public void AddEast()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(1, 0);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(2, 1), boardPositionSum);
        }

        [Test]
        public void AddWest()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(-1, 0);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(0, 1), boardPositionSum);
        }

        [Test]
        public void AddNorthEast()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(1, 1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(2, 2), boardPositionSum);
        }

        [Test]
        public void AddNorthWest()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(-1, 1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(0, 2), boardPositionSum);
        }

        [Test]
        public void AddSouthEast()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(1, -1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(2, 0), boardPositionSum);
        }

        [Test]
        public void AddSouthWest()
        {
            var boardPositionOne = new BoardPosition(1, 1);
            var boardPositionTwo = new BoardPosition(-1, -1);
            var boardPositionSum = boardPositionOne.Add(boardPositionTwo);
            Assert.AreEqual(new BoardPosition(0, 0), boardPositionSum);
        }




    }
}