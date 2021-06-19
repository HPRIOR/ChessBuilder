using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class HomeBaseBuildMoveGeneratorTests : ZenjectUnitTestFixture
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

        private IBuildMoveGenerator _homeBaseBuildGenerator;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _homeBaseBuildGenerator = Container.Resolve<IBuildMoveGenerator>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void OnEmptyBoard_BlackBuildZoneGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var blackBuildZone = _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.Black);

            var expectedPositions = new List<Position>
            {
                new Position(7, 7),
                new Position(6, 7),
                new Position(5, 7),
                new Position(4, 7),
                new Position(3, 7),
                new Position(2, 7),
                new Position(1, 7),
                new Position(0, 7),
                new Position(7, 6), // second row
                new Position(6, 6),
                new Position(5, 6),
                new Position(4, 6),
                new Position(3, 6),
                new Position(2, 6),
                new Position(1, 6),
                new Position(0, 6)
            };

            Assert.That(blackBuildZone, Is.EquivalentTo(expectedPositions));
        }
    }
}