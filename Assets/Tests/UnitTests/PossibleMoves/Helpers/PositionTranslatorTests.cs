using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class PositionTranslatorTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            InstallBindings();
            ResolveContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
        }

        private IPositionTranslatorFactory _positionTranslatorFactory;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            PositionTranslatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _positionTranslatorFactory = Container.Resolve<IPositionTranslatorFactory>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void WhenPieceColourIsBlack_PositionIsMirrored()
        {
            var positionTranslator = _positionTranslatorFactory.Create(PieceColour.Black);
            var mirroredPosOne = positionTranslator.GetRelativePosition(new Position(0, 0));
            var mirroredPosTwo = positionTranslator.GetRelativePosition(new Position(6, 1));
            Assert.AreEqual(new Position(7, 7), mirroredPosOne);
            Assert.AreEqual(new Position(1, 6), mirroredPosTwo);
        }

        [Test]
        public void WhenPieceColourIsWhite_PositionIsSame()
        {
            var positionTranslator = _positionTranslatorFactory.Create(PieceColour.White);
            var mirroredPosOne = positionTranslator.GetRelativePosition(new Position(0, 0));
            var mirroredPosTwo = positionTranslator.GetRelativePosition(new Position(6, 1));
            Assert.AreEqual(new Position(0, 0), mirroredPosOne);
            Assert.AreEqual(new Position(6, 1), mirroredPosTwo);
        }

        [Test]
        public void WhenPieceColourIsBlack_TilIsMirrored()
        {
            var positionTranslator = _positionTranslatorFactory.Create(PieceColour.Black);
            var board = new BoardState();
            board.Board[7, 7].CurrentPiece = new Piece(PieceType.BlackKnight);
            var mirroredTile = positionTranslator.GetRelativeTileAt(new Position(0, 0), board);
            Assert.AreEqual(PieceType.BlackKnight, mirroredTile.CurrentPiece.Type);
        }

        [Test]
        public void WhenPieceColourIsWhite_TilIsSame()
        {
            var positionTranslator = _positionTranslatorFactory.Create(PieceColour.White);
            var board = new BoardState();
            board.Board[7, 7].CurrentPiece = new Piece(PieceType.BlackKnight);
            var mirroredTile = positionTranslator.GetRelativeTileAt(new Position(7, 7), board);
            Assert.AreEqual(PieceType.BlackKnight, mirroredTile.CurrentPiece.Type);
        }
    }
}