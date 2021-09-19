using Bindings.Installers.ModelInstallers.Board;
using Models.Services.Board;
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
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var tile = board[i][j];
                Assert.AreEqual(PieceType.NullPiece, tile.CurrentPiece.Type);
            }
        }
    }
}