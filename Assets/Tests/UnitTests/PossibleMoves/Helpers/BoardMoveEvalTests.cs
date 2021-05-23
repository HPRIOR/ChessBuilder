using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardMoveEvalTests : ZenjectUnitTestFixture
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

        private IBoardEvalFactory _boardEvalFactory;


        private void ResolveContainers()
        {
            BoardEvalInstaller.Install(Container);
        }

        private void InstallBindings()
        {
            _boardEvalFactory = Container.Resolve<IBoardEvalFactory>();
        }


        [Test]
        public void Identifies_NoPieceInTile_True()
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.White);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.NullPiece)};
            Assert.IsTrue(boardEval.NoPieceIn(tile));
        }

        [Test]
        public void Identifies_NoPieceInTile_False(
            [Values] PieceType pieceType
        )
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.White);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(pieceType)};
            if (pieceType != PieceType.NullPiece)
                Assert.IsFalse(boardEval.NoPieceIn(tile));
        }

        [Test]
        public void WithBlackPiece_IdentifiesFriendlyPieceInTile()
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.Black);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            Assert.IsTrue(boardEval.FriendlyPieceIn(tile));
        }

        [Test]
        public void WithWhitePiece_IdentifiesFriendlyPieceInTile()
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.White);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.WhiteBishop)};
            Assert.IsTrue(boardEval.FriendlyPieceIn(tile));
        }

        [Test]
        public void WithWhitePiece_IdentifiesOpposingPieceInTile()
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.White);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.BlackBishop)};
            Assert.IsTrue(boardEval.OpposingPieceIn(tile));
        }

        [Test]
        public void WithBlackPiece_IdentifiesOpposingPieceInTile()
        {
            var boardEval = _boardEvalFactory.Create(PieceColour.Black);
            var tile = new Tile(new BoardPosition(1, 1)) {CurrentPiece = new Piece(PieceType.WhiteBishop)};
            Assert.IsTrue(boardEval.OpposingPieceIn(tile));
        }
    }
}