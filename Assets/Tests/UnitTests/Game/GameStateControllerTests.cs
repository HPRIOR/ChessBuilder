using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bindings.Utils;
using Models.Services.Board;
using Models.Services.Game.Interfaces;
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

        private IGameStateController _gameStateControllerController;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BindAllDefault.InstallAllTo(Container);
        }

        private void ResolveContainer()
        {
            _gameStateControllerController = Container.Resolve<IGameStateController>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void HasNullGameState_WhenInitialised()
        {
            Assert.IsNull(_gameStateControllerController.CurrentGameState);
        }

        [Test]
        public void BoardStateIsUpdated_WhenPassedBoardState()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var initialBoardState = new BoardState(board,
                new HashSet<Position> {new Position(1, 1), new Position(7, 7)}, new HashSet<Position>());
            _gameStateControllerController.UpdateGameState(initialBoardState);

            Assert.AreSame(initialBoardState, _gameStateControllerController.CurrentGameState.BoardState);
        }

        [Test]
        public void EventInvoked_WhenStateUpdated()
        {
            var turnEventInvoker = _gameStateControllerController as ITurnEventInvoker;
            var count = 0;

            void MockFunc(BoardState prev, BoardState newState) => count += 1;

            Debug.Assert(turnEventInvoker != null, nameof(turnEventInvoker) + " != null");
            turnEventInvoker.GameStateChangeEvent += MockFunc;

            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var boardState = new BoardState(board,
                new HashSet<Position> {new Position(1, 1), new Position(7, 7)}, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(boardState);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void WhenInCheck_NoBuildMovesAvailable()
        {
            var board = _boardGenerator.GenerateBoard();

            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 4].CurrentPiece = new Piece(PieceType.BlackQueen);
            var initialBoardState =
                new BoardState(board, new HashSet<Position> {new Position(4, 4), new Position(6, 4)},
                    new HashSet<Position>());
            _gameStateControllerController.UpdateGameState(initialBoardState);
            _gameStateControllerController.UpdateGameState(initialBoardState);

            var expectedBuildMoves =
                new BuildMoves(ImmutableHashSet<Position>.Empty, ImmutableHashSet<PieceType>.Empty);
            Assert.That(_gameStateControllerController.CurrentGameState.PossibleBuildMoves.BuildPieces,
                Is.EquivalentTo(expectedBuildMoves.BuildPieces));
            Assert.That(_gameStateControllerController.CurrentGameState.PossibleBuildMoves.BuildPositions,
                Is.EquivalentTo(expectedBuildMoves.BuildPositions));
        }


        [Test]
        public void ResolvesBuildsOnBoard()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(0, PieceType.WhitePawn);

            var activePieces = new HashSet<Position> {new Position(1, 1), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            // Initialise board state
            _gameStateControllerController.InitializeGame(initialBoardState);

            //Make white turn
            _gameStateControllerController.UpdateGameState(initialBoardState);

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlocked_ByFriendlyPiece()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);

            var activePieces = new HashSet<Position> {new Position(4, 4), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateGameState(whiteTurn);

            // blackTurn
            _gameStateControllerController.UpdateGameState(whiteTurn.CloneWithDecrementBuildState());

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhiteKing));
        }


        [Test]
        public void BuildIsBlocked_ByOpposingPiece()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);

            var activePieces = new HashSet<Position> {new Position(4, 4), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.BlackKing));
        }


        [Test]
        public void BuildIsBlockedByOpposingPiece_ButRemainsInBuildingState()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);

            var activePieces = new HashSet<Position> {new Position(4, 4), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlockedByFriendlyPiece_ButRemainsInBuildingState()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);

            var activePieces = new HashSet<Position> {new Position(4, 4), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildStateIsReset_AfterResolving()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(0, PieceType.WhitePawn);
            var activePieces = new HashSet<Position> {new Position(0, 0), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialState);

            //iterate through game state
            var whiteTurn = initialState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.NullPiece));
        }

        [Test]
        public void BuildStateWillNotResolve_WhenOpposingTurn()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            var activePieces = new HashSet<Position> {new Position(0, 0), new Position(7, 7)};
            var activeBuilds = new HashSet<Position>();
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.InitializeGame(initialBoardState);

            //iterate through game state
            _gameStateControllerController.UpdateGameState(new Position(4, 4), PieceType.WhitePawn);

            _gameStateControllerController.UpdateGameState(new Position(0, 0), new Position(1, 1));

            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void BuildIsUnblocked_WhenBlockingPieceMoves()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);
            var activePieces = new HashSet<Position> {new Position(4, 4), new Position(7, 7)};
            var activeBuilds = new HashSet<Position> {new Position(4, 4)};
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);

            //generate initial game state
            _gameStateControllerController.UpdateGameState(initialBoardState);

            //iterate through game state

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, activeBuilds);
            _gameStateControllerController.UpdateGameState(whiteTurn);
            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].BuildTileState
                            .BuildingPiece ==
                        PieceType.WhitePawn);

            //move blocking piece
            activePieces = new HashSet<Position> {new Position(5, 5), new Position(7, 7)};
            var blackTurn = whiteTurn.CloneWithDecrementBuildState(activePieces, activeBuilds);
            blackTurn.Board[4, 4].CurrentPiece = new Piece();
            blackTurn.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteKing);
            _gameStateControllerController.UpdateGameState(blackTurn);

            //White makes move
            var secondWhiteTurn = blackTurn.CloneWithDecrementBuildState(activePieces, activeBuilds);
            _gameStateControllerController.UpdateGameState(secondWhiteTurn);


            Assert.That(_gameStateControllerController.CurrentGameState.BoardState.Board[4, 4].CurrentPiece.Type,
                Is.EqualTo(PieceType.WhitePawn));
        }

        [Test]
        public void CheckMate_WithTwoMajorPieces()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 6].CurrentPiece = new Piece(PieceType.WhiteRook);
            board[0, 5].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 7].CurrentPiece = new Piece(PieceType.BlackKing);

            var activePieces = new HashSet<Position>
                {new Position(1, 6), new Position(1, 1), new Position(0, 5), new Position(6, 7)};
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
                {new Position(1, 6), new Position(1, 1), new Position(0, 7), new Position(6, 7)};
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[0, 5].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[0, 7].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            Assert.That(_gameStateControllerController.CurrentGameState.PossiblePieceMoves.Count, Is.EqualTo(1));
            Assert.That(_gameStateControllerController.CurrentGameState.PossiblePieceMoves[new Position(6, 7)].Count,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentGameState.Check, Is.True);
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_OnBackRank()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[6, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            board[2, 0].CurrentPiece = new Piece(PieceType.WhiteRook);
            board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(6, 7), new Position(5, 6), new Position(6, 6), new Position(7, 6), new Position(2, 0),
                new Position(6, 0)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
            {
                new Position(6, 7), new Position(5, 6), new Position(6, 6), new Position(7, 6), new Position(2, 7),
                new Position(6, 0)
            };
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[2, 0].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[2, 7].CurrentPiece = new Piece(PieceType.WhiteRook);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));

            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }

        [Test]
        public void CheckMate_QueenAndKnight()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[4, 7].CurrentPiece = new Piece(PieceType.BlackKing);

            board[5, 4].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[7, 6].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(4, 7), new Position(5, 4), new Position(7, 6), new Position(6, 0)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
                {new Position(4, 7), new Position(5, 4), new Position(4, 6), new Position(6, 0)};
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[7, 6].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[4, 6].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_QueenAndBishop()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[6, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            board[2, 2].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(6, 7), new Position(5, 6), new Position(6, 6), new Position(7, 6), new Position(2, 2),
                new Position(1, 1), new Position(6, 0)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[2, 2].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[6, 6].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_TwoBishop()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);


            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[2, 4].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(7, 7), new Position(7, 6), new Position(3, 4), new Position(2, 4), new Position(6, 0)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
                {new Position(7, 7), new Position(7, 6), new Position(3, 4), new Position(3, 3), new Position(6, 0)};
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[2, 4].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[3, 3].CurrentPiece = new Piece(PieceType.WhiteBishop);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_BishopKnight()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[6, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6, 5].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[5, 7].CurrentPiece = new Piece(PieceType.BlackRook);

            board[5, 5].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[6, 3].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(6, 7), new Position(5, 6), new Position(6, 5), new Position(7, 6), new Position(5, 7),
                new Position(5, 5), new Position(6, 3), new Position(6, 0)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
            {
                new Position(6, 7), new Position(5, 6), new Position(6, 5), new Position(7, 6), new Position(5, 7),
                new Position(5, 5), new Position(7, 5), new Position(6, 0)
            };
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[6, 3].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[7, 5].CurrentPiece = new Piece(PieceType.WhiteKnight);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_KingPawn()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[3, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 2].CurrentPiece = new Piece(PieceType.BlackPawn);

            board[3, 6].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[2, 5].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[3, 5].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(3, 7), new Position(7, 2), new Position(3, 6), new Position(2, 5), new Position(3, 5)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
                {new Position(3, 7), new Position(7, 2), new Position(3, 6), new Position(2, 6), new Position(3, 5)};
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[2, 5].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[2, 6].CurrentPiece = new Piece(PieceType.WhitePawn);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_Smothered()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6, 7].CurrentPiece = new Piece(PieceType.BlackRook);

            board[6, 4].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[3, 5].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(7, 7), new Position(7, 6), new Position(6, 6), new Position(6, 7), new Position(6, 4),
                new Position(3, 5)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
            {
                new Position(7, 7), new Position(7, 6), new Position(6, 6), new Position(6, 7), new Position(5, 6),
                new Position(3, 5)
            };
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[6, 4].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[5, 6].CurrentPiece = new Piece(PieceType.WhiteKnight);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_Anastasia()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[6, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[5, 7].CurrentPiece = new Piece(PieceType.BlackRook);

            board[4, 6].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[3, 3].CurrentPiece = new Piece(PieceType.WhiteRook);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);

            // initialise game state
            var activePieces = new HashSet<Position>
            {
                new Position(7, 6), new Position(6, 6), new Position(5, 6), new Position(5, 7), new Position(4, 6),
                new Position(3, 3), new Position(1, 1)
            };
            var initialBoardState = new BoardState(board, activePieces, new HashSet<Position>());
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
            {
                new Position(7, 6), new Position(6, 6), new Position(5, 6),
                new Position(5, 7), new Position(4, 6),
                new Position(7, 3), new Position(1, 1)
            };
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, new HashSet<Position>());
            whiteTurn.Board[3, 3].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[7, 3].CurrentPiece = new Piece(PieceType.WhiteRook);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_MorphyWithBuild()
        {
            // setup board
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            board[4, 6].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 0].BuildTileState = new BuildTileState(0, PieceType.WhiteRook);

            var activePieces = new HashSet<Position>
            {
                new Position(7, 7), new Position(7, 6), new Position(5, 6), new Position(4, 6), new Position(1, 1)
            };
            var activeBuilds = new HashSet<Position> {new Position(6, 0)};
            // initialise game state
            var initialBoardState = new BoardState(board, activePieces, activeBuilds);
            _gameStateControllerController.InitializeGame(initialBoardState);

            activePieces = new HashSet<Position>
            {
                new Position(7, 7), new Position(7, 6), new Position(5, 6), new Position(5, 5),
                new Position(1, 1)
            };
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState(activePieces, activeBuilds);
            whiteTurn.Board[4, 6].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteBishop);
            _gameStateControllerController.UpdateGameState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CurrentGameState.CheckMate, Is.True);
        }
    }
}