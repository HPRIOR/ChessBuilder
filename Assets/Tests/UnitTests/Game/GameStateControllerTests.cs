using System.Collections.Immutable;
using System.Linq;
using Bindings.Utils;
using Models.Services.Game.Interfaces;
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
        public void HasNullBoardState_WhenInitialised()
        {
            Assert.IsNull(_gameStateControllerController.CurrentBoardState);
        }

        [Test]
        public void BoardStateIsUpdated_WhenPassedBoardState()
        {
            var initialBoardState = new BoardState();
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            Assert.AreSame(initialBoardState, _gameStateControllerController.CurrentBoardState);
        }

        [Test]
        public void EventInvoked_WhenStateUpdated()
        {
            var turnEventInvoker = _gameStateControllerController as ITurnEventInvoker;
            var count = 0;

            void MockFunc(BoardState prev, BoardState newState) => count += 1;

            Debug.Assert(turnEventInvoker != null, nameof(turnEventInvoker) + " != null");
            turnEventInvoker.GameStateChangeEvent += MockFunc;

            var boardState = new BoardState();
            _gameStateControllerController.UpdateBoardState(boardState);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void WhenInCheck_NoBuildMovesAvailable()
        {
            var board = _boardGenerator.GenerateBoard();

            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 4].CurrentPiece = new Piece(PieceType.BlackQueen);
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var expectedBuildMoves =
                new BuildMoves(ImmutableHashSet<Position>.Empty, ImmutableHashSet<PieceType>.Empty);
            Assert.That(_gameStateControllerController.PossibleBuildMoves.BuildPieces,
                Is.EquivalentTo(expectedBuildMoves.BuildPieces));
            Assert.That(_gameStateControllerController.PossibleBuildMoves.BuildPositions,
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
            var initialBoardState = new BoardState(board);

            // Initialise board state
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //Make white turn
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            // blackTurn
            _gameStateControllerController.UpdateBoardState(whiteTurn.CloneWithDecrementBuildState());

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
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
            var intitialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(intitialBoardState);

            //iterate through game state
            var whiteTurn = intitialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.BuildingPiece,
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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.BuildingPiece,
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
            var initialState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(initialState);

            //iterate through game state
            var whiteTurn = initialState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.BuildingPiece,
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
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[4, 4].BuildTileState = new BuildTileState(PieceType.WhitePawn);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            var blackTurn = whiteTurn.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(blackTurn);


            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
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
            var initialBoardState = new BoardState(board);

            //generate initial game state
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            //iterate through game state
            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(whiteTurn);
            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].BuildTileState.BuildingPiece ==
                        PieceType.WhitePawn);

            //move blocking piece
            var blackTurn = whiteTurn.CloneWithDecrementBuildState();
            blackTurn.Board[4, 4].CurrentPiece = new Piece();
            blackTurn.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteKing);
            _gameStateControllerController.UpdateBoardState(blackTurn);

            //White makes move
            var secondWhiteTurn = blackTurn.CloneWithDecrementBuildState();
            _gameStateControllerController.UpdateBoardState(secondWhiteTurn);


            Assert.That(_gameStateControllerController.CurrentBoardState.Board[4, 4].CurrentPiece.Type,
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

            // initialise game state
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[0, 5].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[0, 7].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            Assert.That(_gameStateControllerController.PossiblePieceMoves.Count, Is.EqualTo(1));
            Assert.That(_gameStateControllerController.PossiblePieceMoves[new Position(6, 7)].Count, Is.EqualTo(0));
            Assert.That(_gameStateControllerController.Check, Is.True);
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[2, 0].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[2, 7].CurrentPiece = new Piece(PieceType.WhiteRook);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));

            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[7, 6].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[4, 6].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[2, 2].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[6, 6].CurrentPiece = new Piece(PieceType.WhiteQueen);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[2, 4].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[3, 3].CurrentPiece = new Piece(PieceType.WhiteBishop);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[6, 3].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[7, 5].CurrentPiece = new Piece(PieceType.WhiteKnight);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[2, 5].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[2, 6].CurrentPiece = new Piece(PieceType.WhitePawn);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[6, 4].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[5, 6].CurrentPiece = new Piece(PieceType.WhiteKnight);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[3, 3].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[7, 3].CurrentPiece = new Piece(PieceType.WhiteRook);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
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

            // initialise game state
            var initialBoardState = new BoardState(board);
            _gameStateControllerController.UpdateBoardState(initialBoardState);

            var whiteTurn = initialBoardState.CloneWithDecrementBuildState();
            whiteTurn.Board[4, 6].CurrentPiece = new Piece(PieceType.NullPiece);
            whiteTurn.Board[5, 5].CurrentPiece = new Piece(PieceType.WhiteBishop);
            _gameStateControllerController.UpdateBoardState(whiteTurn);

            Assert.That(_gameStateControllerController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateControllerController.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateControllerController.CheckMate, Is.True);
        }
    }
}