using System.Collections.Generic;
using System.Collections.Immutable;
using Bindings.Installers.ModelInstallers.Move;
using Controllers.Interfaces;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Controllers.PieceMovers
{
    [TestFixture]
    public class MoveValidatorTests : ZenjectUnitTestFixture
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

        private IMoveValidator _moveValidator;

        private void InstallBindings()
        {
            MoveValidatorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _moveValidator = Container.Resolve<IMoveValidator>();
        }

        [Test]
        public void RejectsIf_FromEqualsDestination()
        {
            var possibleMoves =
                ImmutableDictionary<Position, ImmutableHashSet<Position>>.Empty;
            var a = new Position(1, 1);
            var b = new Position(1, 1);
            Assert.IsFalse(_moveValidator.ValidateMove(possibleMoves, a, b));
        }

        [Test]
        public void RejectsIf_NoMovesFound()
        {
            var possibleMoves =
                new Dictionary<Position, HashSet<Position>>
                {
                    {new Position(1, 1), new HashSet<Position> {new Position(7, 7)}}
                }.ToImmutableDictionary(keyVal => keyVal.Key, keyVal => ImmutableHashSet.CreateRange(keyVal.Value));
            var a = new Position(1, 1);
            var b = new Position(2, 2);
            Assert.IsFalse(_moveValidator.ValidateMove(possibleMoves, a, b));
        }

        [Test]
        public void AcceptsIf_MovesFound()
        {
            var possibleMoves =
                new Dictionary<Position, HashSet<Position>>
                {
                    {new Position(1, 1), new HashSet<Position> {new Position(2, 2)}}
                }.ToImmutableDictionary(keyVal => keyVal.Key, keyValue => ImmutableHashSet.CreateRange(keyValue.Value));
            var a = new Position(1, 1);
            var b = new Position(2, 2);
            Assert.IsTrue(_moveValidator.ValidateMove(possibleMoves, a, b));
        }
    }
}