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
            var buildState = new BuildTileState();
            Assert.That(buildState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void DefaultBuildState_HasZeroTurns()
        {
            var buildState = new BuildTileState();
            Assert.That(buildState.Turns, Is.EqualTo(0));
        }

        [Test]
        public void Decrement_ProducesNewState()
        {
            var buildState = new BuildTileState(10);
            var newBuildState = buildState.Decrement();
            Assert.AreNotSame(newBuildState, newBuildState);
        }


        [Test]
        public void DecrementAtZero_ProducesSameState()
        {
            var buildState = new BuildTileState();
            var newBuildState = buildState.Decrement();
            Assert.AreEqual(buildState.Turns, newBuildState.Turns);
        }

        [Test]
        public void Decrement_DecrementsTurnsByOne()
        {
            var buildState = new BuildTileState(10);
            var newBuildState = buildState.Decrement();
            Assert.That(newBuildState.Turns, Is.EqualTo(9));
        }

        [Test]
        public void InitialBuildState_HasCorrectPieceType([Values] PieceType pieceType)
        {
            var buildState = new BuildTileState(pieceType);
            Assert.That(buildState.BuildingPiece, Is.EqualTo(pieceType));
        }

        //Queen
        [Test]
        public void InitialBlackQueenBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackQueen);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(9));
        }


        [Test]
        public void InitialWhiteQueenBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.WhiteQueen);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(9));
        }


        //Rook  
        [Test]
        public void InitialBlackRookBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackRook);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(5));
        }


        [Test]
        public void InitialWhiteRookBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.WhiteRook);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(5));
        }

        //Bishop  
        [Test]
        public void InitialBlackBishopBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackBishop);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(3));
        }


        [Test]
        public void InitialWhiteBishopBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.WhiteBishop);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(3));
        }


        //Knight  
        [Test]
        public void InitialBlackKnightBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackKnight);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(3));
        }


        [Test]
        public void InitialWhiteKnightBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackKnight);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(3));
        }

        //Pawn
        [Test]
        public void InitialBlackPawnBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.BlackPawn);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(1));
        }


        [Test]
        public void InitialWhitePawnBuildState_HasCorrectTurns()
        {
            var blackQueenBuildState = new BuildTileState(PieceType.WhitePawn);
            Assert.That(blackQueenBuildState.Turns, Is.EqualTo(1));
        }


        //TODO finish tests for each piece, white/black/ turns/pieceType
    }
}