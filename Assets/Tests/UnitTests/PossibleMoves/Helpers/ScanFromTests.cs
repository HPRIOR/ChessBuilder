using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using NUnit.Framework;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class GenericBoardScannerTests
    {
        [Test]
        public void ScanNorth()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.N);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                    {new BoardPosition(4, 5), new BoardPosition(4, 6), new BoardPosition(4, 7)}));
        }

        [Test]
        public void ScanSouth()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.S);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(4, 3), new BoardPosition(4, 2), new BoardPosition(4, 1), new BoardPosition(4, 0)
                }));
        }

        [Test]
        public void ScanEast()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.E);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(5, 4), new BoardPosition(6, 4), new BoardPosition(7, 4)
                }));
        }

        [Test]
        public void ScanWest()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.W);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(3, 4), new BoardPosition(2, 4), new BoardPosition(1, 4), new BoardPosition(0, 4)
                }));
        }

        [Test]
        public void ScanNorthEast()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.NE);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(5, 5), new BoardPosition(6, 6), new BoardPosition(7, 7)
                }));
        }


        [Test]
        public void ScanNorthWest()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.NW);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(3, 5), new BoardPosition(2, 6), new BoardPosition(1, 7)
                }));
        }

        [Test]
        public void ScanSouthWest()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.SW);
            Assert.That(result.Count(), Is.EqualTo(4));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(3, 3), new BoardPosition(2, 2), new BoardPosition(1, 1), new BoardPosition(0, 0)
                }));
        }

        [Test]
        public void ScanSouthEast()
        {
            var result = ScanFrom.This(new BoardPosition(4, 4), Direction.SE);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result,
                Is.EquivalentTo(new List<BoardPosition>
                {
                    new BoardPosition(5, 3), new BoardPosition(6, 2), new BoardPosition(7, 1)
                }));
        }

        [Test]
        public void ScanOnEdge()
        {
            var result = ScanFrom.This(new BoardPosition(7, 7), Direction.NE);
            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}