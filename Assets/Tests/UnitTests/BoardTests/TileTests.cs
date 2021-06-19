using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.BoardTests
{
    [TestFixture]
    public class TileTests : ZenjectUnitTestFixture
    {
        [Test]
        public void TileConstructed_WithNullPiece()
        {
            var tile = new Tile(new Position(1, 1));
            Assert.That(tile.CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }

        [Test]
        public void TileConstructed_WithCorrectPosition()
        {
            var tile = new Tile(new Position(1, 1));
            Assert.That(tile.Position, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void TileConstructed_WithCorrectBuildState()
        {
            var tile = new Tile(new Position(1, 1));
            Assert.That(tile.BuildState, Is.EqualTo(new BuildState()));
        }

        [Test]
        public void TileCloned_WithSamePosition()
        {
            var tile = new Tile(new Position(1, 1));
            var tileClone = tile.Clone();

            Assert.That(tile.Position, Is.EqualTo(tileClone.Position));
        }

        [Test]
        public void TileCloned_WithSamePiece()
        {
            var tile = new Tile(new Position(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            var tileClone = tile.Clone();

            Assert.That(tile.CurrentPiece, Is.EqualTo(tileClone.CurrentPiece));
        }

        [Test]
        public void TileCloned_WithSameBuildState()
        {
            var tile = new Tile(new Position(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            var tileClone = tile.Clone();

            Assert.That(tile.BuildState, Is.EqualTo(tileClone.BuildState));
        }

        [Test]
        public void TileCloned_IsNotSame()
        {
            var tile = new Tile(new Position(1, 1));
            var tileClone = tile.Clone();

            Assert.AreNotSame(tile, tileClone);
        }

        [Test]
        public void TileClonedWithDecrementBuildState_DoesNotChange_IfNullPieceInBuildState()
        {
            var tile = new Tile(new Position(1, 1));
            var tileClone = tile.CloneWithDecrementBuildState();

            Assert.AreEqual(tile.BuildState, tileClone.BuildState);
        }


        [Test]
        public void TileClonedWithDecrementBuildState_Decrements_IfPieceIsBeingBuilt()
        {
            var tile = new Tile(new Position(1, 1), new BuildState(9, new Piece(PieceType.WhiteQueen)));
            var tileClone = tile.CloneWithDecrementBuildState();

            Assert.AreEqual(8, tileClone.BuildState.Turns);
        }


        [Test]
        public void TileClonedWithDecrementBuildState_RetainsWaitingBuild_IfPieceIsBeingBuilt_And_FullyDecremented()
        {
            var tile = new Tile(new Position(1, 1), new BuildState(0, new Piece(PieceType.WhiteQueen)));
            var tileClone = tile.CloneWithDecrementBuildState();

            Assert.AreEqual(tile.BuildState.Turns, tileClone.BuildState.Turns);
            Assert.AreEqual(tile.BuildState.BuildingPiece, tileClone.BuildState.BuildingPiece);
        }
    }
}