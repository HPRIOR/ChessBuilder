using System.Collections.Generic;
using Models.Services.Utils;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils.CacheUtils
{
    [TestFixture]
    public class ScanCacheBetweenTests : ZenjectUnitTestFixture
    {
        [Test]
        public void ScansBetweenTwoPositions()
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(5, 5);
            var expectedPositions = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4)
            };

            var result = ScanCache.ScanBetween(startPosition, endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }


        [Test]
        public void WithTwoPositionsNextToEachOther_ReturnsEmptyList()
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(1, 1);

            var expectedPositions = new List<Position>();
            var result = ScanCache.ScanBetween(startPosition, endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }
    }
}