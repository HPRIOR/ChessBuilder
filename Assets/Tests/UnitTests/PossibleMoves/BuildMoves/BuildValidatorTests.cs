using System.Collections.Generic;
using Bindings.Installers.ModelInstallers.Build;
using Controllers.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuildValidatorTests : ZenjectUnitTestFixture
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

        private IBuildValidator _buildValidator;

        private void InstallBindings()
        {
            BuildValidatorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _buildValidator = Container.Resolve<IBuildValidator>();
        }

        [Test]
        public void WithBuildMovesAndPieces_CanBuild()
        {
            var possibleBuildMoves = new HashSet<Position> {new Position(1, 1)};
            var possibleBuildPieces = new HashSet<PieceType> {PieceType.BlackKnight};
            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves, possibleBuildPieces);

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.True);
        }

        [Test]
        public void WithBuildMovesAndNotPiece_CannotBuild()
        {
            var possibleBuildMoves = new HashSet<Position> {new Position(1, 1)};
            var possibleBuildPieces = new HashSet<PieceType> {PieceType.WhiteKnight};
            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves, possibleBuildPieces);

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.False);
        }


        [Test]
        public void WithNoBuildMovesAndPiece_CannotBuild()
        {
            var possibleBuildMoves = new HashSet<Position> {new Position(2, 2)};
            var possibleBuildPieces = new HashSet<PieceType> {PieceType.BlackKnight};
            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves, possibleBuildPieces);

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.False);
        }
    }
}