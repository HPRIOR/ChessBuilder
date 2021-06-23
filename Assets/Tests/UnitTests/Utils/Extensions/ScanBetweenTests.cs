﻿using System.Collections.Generic;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPos;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils.Extensions
{
    [TestFixture]
    public class ScanBetweenTests : ZenjectUnitTestFixture
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

            var result = startPosition.ScanBetween(endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }


        [Test]
        public void WithTwoPositionsNextToEachOther_ReturnsEmptyList()
        {
            var startPosition = new Position(0, 0);
            var endPosition = new Position(1, 1);

            var expectedPositions = new List<Position>();
            var result = startPosition.ScanBetween(endPosition);

            Assert.That(result, Is.EquivalentTo(expectedPositions));
        }
    }
}