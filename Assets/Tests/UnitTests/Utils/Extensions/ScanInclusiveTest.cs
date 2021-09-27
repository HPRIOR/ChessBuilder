using System.Collections.Generic;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils.Extensions
{
    [TestFixture]
    public class ScanInclusiveTest : ZenjectUnitTestFixture
    {
        [Test]
        public void ScanIncludesStartAndDestination()
        {
            var start = new Position(0, 0);
            var end = new Position(5, 5);
            var expected = new List<Position>()
            {
                new Position(0,0),
                new Position(1,1),
                new Position(2,2),
                new Position(3,3),
                new Position(4,4),
                new Position(5,5)
            };
            Assert.That(start.ScanInclusiveTo(end), Is.EquivalentTo(expected));
        }
    }
}