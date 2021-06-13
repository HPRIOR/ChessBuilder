using System;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPos;
using NUnit.Framework;

namespace Tests.UnitTests.Utils.Extensions
{
    [TestFixture]
    public class DirectionToTests
    {
        [Test]
        public void DirectionNorth()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(4, 6));
            Assert.That(result, Is.EqualTo(Direction.N));
        }

        [Test]
        public void DirectionSouth()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(4, 1));
            Assert.That(result, Is.EqualTo(Direction.S));
        }

        [Test]
        public void DirectionEast()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(7, 4));
            Assert.That(result, Is.EqualTo(Direction.E));
        }

        [Test]
        public void DirectionWest()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(0, 4));
            Assert.That(result, Is.EqualTo(Direction.W));
        }

        [Test]
        public void DirectionNorthEast()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(5, 5));
            Assert.That(result, Is.EqualTo(Direction.NE));
        }

        [Test]
        public void DirectionNorthWest()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(3, 5));
            Assert.That(result, Is.EqualTo(Direction.NW));
        }

        [Test]
        public void DirectionSouthEast()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(5, 3));
            Assert.That(result, Is.EqualTo(Direction.SE));
        }

        [Test]
        public void DirectionSouthWest()
        {
            var result = new BoardPosition(4, 4).DirectionTo(new BoardPosition(2, 2));
            Assert.That(result, Is.EqualTo(Direction.SW));
        }

        [Test]
        public void SamePositionThrowError()
        {
            Assert.Throws<Exception>(() => new BoardPosition(4, 4).DirectionTo(new BoardPosition(4, 4)));
        }
    }
}