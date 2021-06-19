using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuildStateTests : ZenjectUnitTestFixture
    {
        [Test]
        public void DefaultBuildState_HasNullPiece()
        {
            var buildState = new BuildState();
            Assert.That(buildState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void DefaultBuildState_HasZeroTurns()
        {
            var buildState = new BuildState();
            Assert.That(buildState.Turns, Is.EqualTo(0));
        }

        [Test]
        public void Decrement_ProducesNewState()
        {
            var buildState = new BuildState(10);
            var newBuildState = buildState.Decrement();
            Assert.AreNotSame(newBuildState, newBuildState);
        }


        [Test]
        public void DecrementAtZero_ProducesSameState()
        {
            var buildState = new BuildState();
            var newBuildState = buildState.Decrement();
            Assert.AreEqual(buildState.Turns, newBuildState.Turns);
        }

        [Test]
        public void Decrement_DecrementsTurnsByOne()
        {
            var buildState = new BuildState(10);
            var newBuildState = buildState.Decrement();
            Assert.That(newBuildState.Turns, Is.EqualTo(9));
        }
    }
}