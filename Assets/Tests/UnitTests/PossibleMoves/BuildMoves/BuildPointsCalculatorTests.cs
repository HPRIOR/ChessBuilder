using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuildPointsCalculatorTests : ZenjectUnitTestFixture
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

        private IBuildPointsCalculator _buildPointsCalculator;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BuildPointsCalculatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _buildPointsCalculator = Container.Resolve<IBuildPointsCalculator>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void PointsAreSubtracted_ByBuildingPieces()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(PieceType.BlackQueen);
            var boardState = new BoardState(board);

            var playerState = _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, boardState, 10);

            Assert.That(playerState, Is.EqualTo(new PlayerState(1)));
        }
    }
}