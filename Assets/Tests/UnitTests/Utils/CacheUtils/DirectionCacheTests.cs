using Models.Services.Moves.Utils;
using Models.Services.Utils;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils
{
    [TestFixture]
    public class DirectionMapTests : ZenjectUnitTestFixture
    {
        [Test]
        public void DirectionNorth()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(4, 6));
            Assert.That(result, Is.EqualTo(Direction.N));
        }

        [Test]
        public void DirectionSouth()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(4, 1));
            Assert.That(result, Is.EqualTo(Direction.S));
        }

        [Test]
        public void DirectionEast()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(7, 4));
            Assert.That(result, Is.EqualTo(Direction.E));
        }


        [Test]
        public void DirectionWest()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(0, 4));
            Assert.That(result, Is.EqualTo(Direction.W));
        }

        [Test]
        public void DirectionNorthEast()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(5, 5));
            Assert.That(result, Is.EqualTo(Direction.NE));
        }

        [Test]
        public void DirectionNorthWest()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(3, 5));
            Assert.That(result, Is.EqualTo(Direction.NW));
        }

        [Test]
        public void DirectionSouthEast()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(5, 3));
            Assert.That(result, Is.EqualTo(Direction.SE));
        }

        [Test]
        public void DirectionSouthWest()
        {
            var result = DirectionCache.DirectionFrom(new Position(4, 4), new Position(2, 2));
            Assert.That(result, Is.EqualTo(Direction.SW));
        }
    }
}