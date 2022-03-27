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
            Assert.That(tile.CurrentPiece, Is.EqualTo(PieceType.NullPiece));
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
            Assert.That(tile.BuildTileState, Is.EqualTo(new BuildTileState()));
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
            var tile = new Tile(new Position(1, 1)) { CurrentPiece = PieceType.BlackBishop };
            var tileClone = tile.Clone();

            Assert.That(tile.CurrentPiece, Is.EqualTo(tileClone.CurrentPiece));
        }

        [Test]
        public void TileCloned_WithSameBuildState()
        {
            var tile = new Tile(new Position(1, 1)) { CurrentPiece = PieceType.BlackBishop };
            var tileClone = tile.Clone();

            Assert.That(tile.BuildTileState, Is.EqualTo(tileClone.BuildTileState));
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
            var tileClone = tile.WithDecrementedBuildState(PieceColour.White);

            Assert.AreEqual(tile.BuildTileState, tileClone.BuildTileState);
        }


        [Test]
        public void TileClonedWithDecrementBuildState_Decrements_IfPieceIsBeingBuilt()
        {
            var tile = new Tile(new Position(1, 1), new BuildTileState(9, PieceType.WhiteQueen));
            var tileClone = tile.WithDecrementedBuildState(PieceColour.White);

            Assert.AreEqual(8, tileClone.BuildTileState.Turns);
        }


        [Test]
        public void TileClonedWithDecrementBuildState_RetainsWaitingBuild_IfPieceIsBeingBuilt_And_FullyDecremented()
        {
            var tile = new Tile(new Position(1, 1), new BuildTileState(0, PieceType.WhiteQueen));
            var tileClone = tile.WithDecrementedBuildState(PieceColour.White);

            Assert.AreEqual(tile.BuildTileState.Turns, tileClone.BuildTileState.Turns);
            Assert.AreEqual(tile.BuildTileState.BuildingPiece, tileClone.BuildTileState.BuildingPiece);
        }
    }
}