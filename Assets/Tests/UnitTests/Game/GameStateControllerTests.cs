using System;
using System.Collections.Generic;
using Bindings.Utils;
using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using UnityEngine;
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


        [Test]
        public void ResolvesBuildsOnBoard()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlocked_ByFriendlyPiece()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhiteKing));
        }


        [Test]
        public void BuildIsBlocked_ByOpposingPiece()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.BlackKing));
        }


        [Test]
        public void BuildIsBlockedByOpposingPiece_ButRemainsInBuildingState()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlockedByFriendlyPiece_ButRemainsInBuildingState()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildStateIsReset_AfterResolving()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            //iterate through game state
            _gameStateController.UpdateBoardState(boardState.CloneWithDecrementBuildState());

            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.BuildingPiece,
                Is.EqualTo(PieceType.NullPiece));
        }

        [Test]
        public void BuildStateWillNotResolve_WhenOpposingTurn()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            var intermediateBoardState = boardState.CloneWithDecrementBuildState();
            intermediateBoardState.Board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            //iterate through game state
            _gameStateController.UpdateBoardState(intermediateBoardState);

            var finalBoardState = intermediateBoardState.CloneWithDecrementBuildState();

            _gameStateController.UpdateBoardState(finalBoardState);

            Assert.That(_gameStateController.Turn == PieceColour.Black);
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void BuildIsUnblocked_WhenBlockingPieceMoves()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            var boardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(boardState);

            var secondBoardState = boardState.CloneWithDecrementBuildState();
            //iterate through game state
            _gameStateController.UpdateBoardState(secondBoardState);
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.BuildingPiece ==
                        PieceType.WhitePawn);

            //move blocking piece
            var thirdBoardState = secondBoardState.CloneWithDecrementBuildState();
            thirdBoardState.Board[4, 4].CurrentPiece = new Piece();
            thirdBoardState.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteKing);
            _gameStateController.UpdateBoardState(thirdBoardState);

            // wait for turn 
            var fourthBoardState = thirdBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(fourthBoardState);


            Assert.That(_gameStateController.Turn == PieceColour.White);
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhitePawn));
        }
    }
}