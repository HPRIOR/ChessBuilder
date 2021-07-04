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
            var initialBoardState = new BoardState();
            _gameStateController.UpdateBoardState(initialBoardState);

            Assert.AreSame(initialBoardState, _gameStateController.CurrentBoardState);
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
            var initialBoardState = new BoardState(board);
            _gameStateController.UpdateBoardState(initialBoardState);

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
            board[4, 4].BuildState = new BuildState(0, PieceType.WhitePawn);
            var initialBoardState = new BoardState(board);

            // Initialise board state
            _gameStateController.UpdateBoardState(initialBoardState);

            //Make white turn
            _gameStateController.UpdateBoardState(initialBoardState);

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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);

            // blackTurn
            _gameStateController.UpdateBoardState(whiteTurn.CloneWithDecrementBuildState());

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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);

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
            var intitialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(intitialBoardState);

            //iterate through game state
            var whiteTurn = intitialBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);

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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);

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
            board[4, 4].BuildState = new BuildState(0, PieceType.WhitePawn);
            var initialState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialState);

            //iterate through game state
            var whiteTurn = initialState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);

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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[4, 4].BuildState = new BuildState(PieceType.WhitePawn);
            _gameStateController.UpdateBoardState(whiteTurn);

            var blackTurn = whiteTurn.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(blackTurn);


            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.Turns, Is.EqualTo(0));
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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(whiteTurn);
            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].BuildState.BuildingPiece ==
                        PieceType.WhitePawn);

            //move blocking piece
            var blackTurn = whiteTurn.CloneWithDecrementBuildState();
            blackTurn.Board[4, 4].CurrentPiece = new Piece();
            blackTurn.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteKing);
            _gameStateController.UpdateBoardState(blackTurn);

            //White makes move
            var secondWhiteTurn = blackTurn.CloneWithDecrementBuildState();
            _gameStateController.UpdateBoardState(secondWhiteTurn);


            Assert.That(_gameStateController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhitePawn));
        }
    }
}