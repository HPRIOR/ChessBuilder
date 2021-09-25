using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Utils;
using Models.Services.Utils;
using Models.State.Board;
using NUnit.Framework;

namespace Tests.UnitTests.Utils
{
    [TestFixture]
    public class ScanMapTests
    {
        [Test]
        public void ScanNorth()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.N);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                    {new Position(4, 5), new Position(4, 6), new Position(4, 7)}));
        }

        [Test]
        public void ScanSouth()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.S);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(4, 3), new Position(4, 2), new Position(4, 1), new Position(4, 0)
                }));
        }

        [Test]
        public void ScanEast()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.E);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(5, 4), new Position(6, 4), new Position(7, 4)
                }));
        }

        [Test]
        public void ScanWest()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.W);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(3, 4), new Position(2, 4), new Position(1, 4), new Position(0, 4)
                }));
        }

        [Test]
        public void ScanNorthEast()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.NE);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(5, 5), new Position(6, 6), new Position(7, 7)
                }));
        }


        [Test]
        public void ScanNorthWest()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.NW);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(3, 5), new Position(2, 6), new Position(1, 7)
                }));
        }

        [Test]
        public void ScanSouthWest()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.SW);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(3, 3), new Position(2, 2), new Position(1, 1), new Position(0, 0)
                }));
        }

        [Test]
        public void ScanSouthEast()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.SE);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<Position>
                {
                    new Position(5, 3), new Position(6, 2), new Position(7, 1)
                }));
        }

        [Test]
        public void ScanOnEdge()
        {
            var result = ScanCache.Scan(new Position(7, 7), Direction.NE);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ScanResultsInCorrectOrder_StartToFinish()
        {
            var result = ScanCache.Scan(new Position(4, 4), Direction.SE);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EqualTo(new List<Position>
                {
                    new Position(5, 3), new Position(6, 2), new Position(7, 1)
                }));
        }
    }
}