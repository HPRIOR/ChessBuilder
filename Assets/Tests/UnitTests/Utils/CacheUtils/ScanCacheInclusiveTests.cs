﻿using System.Collections.Generic;
using Models.Services.Utils;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils.CacheUtils
{
    [TestFixture]
    public class ScanCacheInclusiveTests : ZenjectUnitTestFixture
    {
        [Test]
        public void ScansBetweenTwoPositions()
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(5, 5);
            var expectedPositions = new List<Position>
            {
                new Position(0,0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5)
            };

            var result = ScanCache.ScanInclusiveTo(startPosition, endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }


        [Test]
        public void WithTwoPositionsNextToEachOther_ReturnsSingleItemList()
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(1, 1);

            var expectedPositions = new List<Position> {new Position(1, 1)};
            var result = ScanCache.ScanTo(startPosition, endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }
    }
}