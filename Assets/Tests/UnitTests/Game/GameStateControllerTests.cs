using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bindings.Utils;
using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
    [TestFixture]
    public class GameStateControllerTests : ZenjectUnitTestFixture
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

        private IGameState _gameStateController;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BindAllDefault.InstallAllTo(Container);
        }

        private void ResolveContainer()
        {
            _gameStateController = Container.Resolve<IGameState>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void HasNullBoardState_WhenInitialised()
        {
            Assert.IsNull(_gameStateController.CurrentBoardState);
        }

        [Test]
        public void BoardStateIsUpdated_WhenPassedBoardState()
        {
            var boardState = new BoardState();
            _gameStateController.UpdateBoardState(boardState);

            Assert.AreSame(boardState, _gameStateController.CurrentBoardState);
        }

        [Test]
        public void EventInvoked_WhenStateUpdated()
        {
            var turnEventInvoker = _gameStateController as ITurnEventInvoker;
            var count = 0;
            Action<BoardState, BoardState> mockFunc = (prev, newState) => { count += 1; };
            Debug.Assert(turnEventInvoker != null, nameof(turnEventInvoker) + " != null");
            turnEventInvoker.GameStateChangeEvent += mockFunc;

            var boardState = new BoardState();
            _gameStateController.UpdateBoardState(boardState);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void WhenInCheck_NoBuildMovesAvailable()
        {
            var board = _boardGenerator.GenerateBoard();

            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 4].CurrentPiece = new Piece(PieceType.BlackQueen);
            var boardState = new BoardState(board);
            _gameStateController.UpdateBoardState(boardState);

            var expectedBuildMoves = new BuildMoves(new HashSet<Position>(), new HashSet<PieceType>());
            Assert.That(_gameStateController.PossibleBuildMoves.BuildPieces,
                Is.EquivalentTo(expectedBuildMoves.BuildPieces));
            Assert.That(_gameStateController.PossibleBuildMoves.BuildPositions,
                Is.EquivalentTo(expectedBuildMoves.BuildPositions));
        }
    }
}