using System.Linq;
using Models.Services.Moves.Utils;
using Models.Services.Moves.Utils.Scanners;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class ScanCacheTests : ZenjectUnitTestFixture
    {
        [Test]
        public void DoesNotIncludeParameterPosition()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.N);
            Assert.That(!sut.Contains(new Position(5, 5)));
        }


        [Test]
        public void IncludesCorrectPositionsNorth()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.N);
            Assert.That(sut, Is.EquivalentTo(new[] { new Position(5, 6), new Position(5, 7) }));
        }

        [Test]
        public void IncludesCorrectPositionsSouth()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.S);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(5, 4), new Position(5, 3), new Position(5, 2), new Position(5, 1), new Position(5, 0)
                }));
        }

        [Test]
        public void IncludesCorrectPositionsEast()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.E);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(6, 5), new Position(7, 5)
                }));
        }


        [Test]
        public void IncludesCorrectPositionsWest()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.W);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(4, 5), new Position(3, 5), new Position(2, 5), new Position(1, 5), new Position(0, 5)
                }));
        }


        [Test]
        public void IncludesCorrectPositionsNorthEast()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.NE);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(6, 6), new Position(7, 7)
                }));
        }


        [Test]
        public void IncludesCorrectPositionsNorthWest()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.NW);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(4, 6), new Position(3, 7)
                }));
        }

        [Test]
        public void IncludesCorrectPositionsSouthWest()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.SW);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(4, 4), new Position(3, 3), new Position(2, 2), new Position(1, 1), new Position(0, 0)
                }));
        }

        [Test]
        public void IncludesCorrectPositionsSouthEast()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(5, 5), Direction.SE);
            Assert.That(sut,
                Is.EquivalentTo(new[]
                {
                    new Position(6, 4), new Position(7, 3)
                }));
        }

        [Test]
        public void OnEdgeEmptyListReturned()
        {
            var sut = ScanCache.GetPositionsToEndOfBoard(new Position(7, 0), Direction.SE);
            Assert.That(sut,
                Is.EquivalentTo(new Position[] { }));
        }
    }
}