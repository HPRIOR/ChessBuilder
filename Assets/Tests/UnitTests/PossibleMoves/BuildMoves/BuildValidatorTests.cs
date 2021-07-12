using System.Collections.Immutable;
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
            var possibleBuildMoves = ImmutableHashSet<Position>.Empty.ToBuilder();
            possibleBuildMoves.Add(new Position(1, 1));
            var possibleBuildPieces = ImmutableHashSet<PieceType>.Empty.ToBuilder();
            possibleBuildPieces.Add(PieceType.BlackKnight);
            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves.ToImmutable(),
                possibleBuildPieces.ToImmutable());

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.True);
        }

        [Test]
        public void WithBuildMovesAndNotPiece_CannotBuild()
        {
            var possibleBuildMoves = ImmutableHashSet<Position>.Empty.ToBuilder();
            possibleBuildMoves.Add(new Position(1, 1));
            var possibleBuildPieces = ImmutableHashSet<PieceType>.Empty.ToBuilder();
            possibleBuildPieces.Add(PieceType.WhiteKnight);

            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves.ToImmutable(),
                possibleBuildPieces.ToImmutable());

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.False);
        }


        [Test]
        public void WithNoBuildMovesAndPiece_CannotBuild()
        {
            var possibleBuildMoves = ImmutableHashSet<Position>.Empty.ToBuilder();
            possibleBuildMoves.Add(new Position(2, 2));
            var possibleBuildPieces = ImmutableHashSet<PieceType>.Empty.ToBuilder();
            possibleBuildPieces.Add(PieceType.BlackKnight);

            var buildMoves = new Models.State.BuildState.BuildMoves(possibleBuildMoves.ToImmutable(),
                possibleBuildPieces.ToImmutable());

            var result = _buildValidator.ValidateBuild(buildMoves, new Position(1, 1), PieceType.BlackKnight);

            Assert.That(result, Is.False);
        }
    }
}