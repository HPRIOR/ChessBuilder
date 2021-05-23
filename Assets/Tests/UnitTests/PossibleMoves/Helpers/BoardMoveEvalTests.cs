using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardMoveEvalTests : ZenjectUnitTestFixture
    {
        private IBoardMoveEval _boardEval;
        public void ResolveContainers()
        {
            BoardEvalInstaller.Install(Container);
        }

        public void InstallBindings()
        {
            _boardEval = Container.Resolve<IBoardMoveEval>();

        }
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

        [Test]
        public void Identifies_NoPieceInTile()
        {
            ITile tile = new Tile(new BoardPosition(1,1));
        }
    }
}