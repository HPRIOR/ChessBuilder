using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;
using NUnit.Framework;

namespace Tests.UnitTests.Utils.Extensions
{
    [TestFixture]
    public class DirectionToTests
    {
        [Test]
        public void DirectionNorth()
        {
            var result = new Position(1, 1).DirectionTo(new Position(1, 6));
            Assert.That(result, Is.EqualTo(Direction.N));
        }

        [Test]
        public void DirectionSouth()
        {
            var result = new Position(4, 4).DirectionTo(new Position(4, 1));
            Assert.That(result, Is.EqualTo(Direction.S));
        }

        [Test]
        public void DirectionSouth_Two()
        {
            var result = new Position(1, 6).DirectionTo(new Position(1, 1));
            Assert.That(result, Is.EqualTo(Direction.S));
        }

        [Test]
        public void DirectionEast()
        {
            var result = new Position(4, 4).DirectionTo(new Position(7, 4));
            Assert.That(result, Is.EqualTo(Direction.E));
        }


        [Test]
        public void DirectionWest()
        {
            var result = new Position(4, 4).DirectionTo(new Position(0, 4));
            Assert.That(result, Is.EqualTo(Direction.W));
        }

        [Test]
        public void DirectionNorthEast()
        {
            var result = new Position(4, 4).DirectionTo(new Position(5, 5));
            Assert.That(result, Is.EqualTo(Direction.Ne));
        }

        [Test]
        public void DirectionNorthWest()
        {
            var result = new Position(4, 4).DirectionTo(new Position(3, 5));
            Assert.That(result, Is.EqualTo(Direction.Nw));
        }

        [Test]
        public void DirectionSouthEast()
        {
            var result = new Position(4, 4).DirectionTo(new Position(5, 3));
            Assert.That(result, Is.EqualTo(Direction.Se));
        }

        [Test]
        public void DirectionSouthWest()
        {
            var result = new Position(4, 4).DirectionTo(new Position(2, 2));
            Assert.That(result, Is.EqualTo(Direction.Sw));
        }

        [Test]
        public void NoDirection_NE()
        {
            var result = new Position(4, 4).DirectionTo(new Position(5, 7));
            Assert.That(result, Is.EqualTo(Direction.Null));
        }


        [Test]
        public void NoDirection_NW()
        {
            var result = new Position(4, 4).DirectionTo(new Position(3, 7));
            Assert.That(result, Is.EqualTo(Direction.Null));
        }

        [Test]
        public void NoDirection_SE()
        {
            var result = new Position(4, 4).DirectionTo(new Position(5, 0));
            Assert.That(result, Is.EqualTo(Direction.Null));
        }

        [Test]
        public void NoDirection_SW()
        {
            var result = new Position(4, 4).DirectionTo(new Position(3, 0));
            Assert.That(result, Is.EqualTo(Direction.Null));
        }

        [Test]
        public void SamePositionThrowError()
        {
            Assert.Throws<DirectionException>(() => new Position(4, 4).DirectionTo(new Position(4, 4)));
        }
    }
}