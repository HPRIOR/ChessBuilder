using Bindings.Installers.ModelInstallers.Board;
using Models.Services.Interfaces;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.BoardTests
{
    [TestFixture]
    public class BoardGeneratorTests : ZenjectUnitTestFixture
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

        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void BoardIsGenerated_WithNullPiecesInTiles()
        {
            var board = _boardGenerator.GenerateBoard();
            foreach (var tile in board) Assert.AreEqual(PieceType.NullPiece, tile.CurrentPiece.Type);
        }
    }
}