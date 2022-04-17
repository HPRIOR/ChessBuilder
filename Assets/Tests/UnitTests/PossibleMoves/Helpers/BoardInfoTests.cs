using System.Collections.Generic;
using System.Linq;
using Models.Services.Board;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Tests.UnitTests.PossibleMoves.PieceMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardInfoTests : ZenjectUnitTestFixture
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

        private IBoardInfo _boardInfo;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            PossibleMovesBinder.InstallBindings(Container);
        }

        private void ResolveContainer()
        {
            _boardInfo = Container.Resolve<IBoardInfo>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void BoardEval_IsInstalled()
        {
            Assert.IsNotNull(_boardInfo);
        }

        [Test]
        public void OnBlackTurn_MovesAreDividedBetweenTurn(
        )
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 1), PieceType.WhiteKing },
                { new Position(1, 2), PieceType.WhitePawn },
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(7, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            _boardInfo.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardInfo.TurnMoves, Contains.Key(new Position(7, 7)));
            Assert.That(_boardInfo.TurnMoves, Contains.Key(new Position(7, 6)));
            Assert.That(_boardInfo.EnemyMoves, Contains.Key(new Position(1, 1)));
            Assert.That(_boardInfo.EnemyMoves, Contains.Key(new Position(1, 2)));
            Assert.That(_boardInfo.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardInfo.EnemyMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnWhiteTurn_MovesAreDividedBetweenTurn()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {

                { new Position(1, 1), PieceType.WhiteKing },
                { new Position(1, 2), PieceType.WhitePawn },
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(7, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.TurnMoves, Contains.Key(new Position(1, 1)));
            Assert.That(_boardInfo.TurnMoves, Contains.Key(new Position(1, 2)));
            Assert.That(_boardInfo.EnemyMoves, Contains.Key(new Position(7, 7)));
            Assert.That(_boardInfo.EnemyMoves, Contains.Key(new Position(7, 6)));
            Assert.That(_boardInfo.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardInfo.EnemyMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnBlackTurn_FriendlyKingIsFound()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {

                { new Position(1, 1), PieceType.WhiteKing },
                { new Position(1, 2), PieceType.WhitePawn },
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(7, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            _boardInfo.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new Position(7, 7)));
        }


        [Test]
        public void OnWhiteTurn_FriendlyKingIsFound()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {

                { new Position(1, 1), PieceType.WhiteKing },
                { new Position(1, 2), PieceType.WhitePawn },
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(7, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void KingPositionNullIsOutOfBound()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {

                { new Position(1, 2), PieceType.WhitePawn },
                { new Position(7, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            _boardInfo.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardInfo.KingPosition, Is.EqualTo(new Position(8, 8)));
        }
    }
}