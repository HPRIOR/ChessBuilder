using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class TileEvaluatorTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            ResolveContainers();
            InstallBindings();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
        }

        private ITileEvaluatorFactory _tileEvaluatorFactory;


        private void ResolveContainers()
        {
            TileEvaluatorInstaller.Install(Container);
        }

        private void InstallBindings()
        {
            _tileEvaluatorFactory = Container.Resolve<ITileEvaluatorFactory>();
        }


        [Test]
        public void Identifies_NoPieceInTile_True()
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.White, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.NullPiece)};
            Assert.IsTrue(tileEval.NoPieceIn(tile));
        }

        [Test]
        public void Identifies_NoPieceInTile_False(
            [Values] PieceType pieceType
        )
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.White, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(pieceType)};
            if (pieceType != PieceType.NullPiece)
                Assert.IsFalse(tileEval.NoPieceIn(tile));
        }

        [Test]
        public void WithBlackPiece_IdentifiesFriendlyPieceInTile()
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.Black, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            Assert.IsTrue(tileEval.FriendlyPieceIn(tile));
        }

        [Test]
        public void WithWhitePiece_IdentifiesFriendlyPieceInTile()
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.White, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.WhiteBishop)};
            Assert.IsTrue(tileEval.FriendlyPieceIn(tile));
        }

        [Test]
        public void WithWhitePiece_IdentifiesOpposingPieceInTile()
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.White, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            Assert.IsTrue(tileEval.OpposingPieceIn(tile));
        }

        [Test]
        public void WithBlackPiece_IdentifiesOpposingPieceInTile()
        {
            var tileEval = _tileEvaluatorFactory.Create(PieceColour.Black, false);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.WhiteBishop)};
            Assert.IsTrue(tileEval.OpposingPieceIn(tile));
        }
    }
}