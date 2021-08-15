using System.Collections.Generic;
using System.Collections.Immutable;
using Bindings.Utils;
using Models.Services.Board;
using Models.Services.Game.Implementations;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
    /// <summary>
    ///     Note: invoking UpdateGameState with Black's turn when it seems as though it's white's may seem counter
    ///     intuitive. This is because it is setting up blacks turn after white's move (available moves, check state etc)
    /// </summary>
    [TestFixture]
    public class GameStateUpdaterTests : ZenjectUnitTestFixture
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

        private GameStateUpdaterFactory _gameStateUpdaterFactory;
        private IBoardGenerator _boardGenerator;
        private GameInitializer _gameStateInitializer;

        private void InstallBindings()
        {
            BindAllDefault.InstallAllTo(Container);
        }

        private void ResolveContainer()
        {
            _gameStateUpdaterFactory = Container.Resolve<GameStateUpdaterFactory>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _gameStateInitializer = Container.Resolve<GameInitializer>();
        }

        [Test]
        public void GameStateChange_Build_IsNullWithNoBuilds()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);

            Assert.That(sut.Build, Is.Null);
        }

        [Test]
        public void GameStateChange_Move_IsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);

            Assert.That(sut.Move.To, Is.EqualTo(new Position(2, 2)));
            Assert.That(sut.Move.From, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void GameStateChange_Move_IsNull_GivenBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.Black);

            Assert.That(sut.Move, Is.Null);
        }


        [Test]
        public void GameStateChange_Build_IsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.Black);

            Assert.That(sut.Build.At, Is.EqualTo(new Position(1, 1)));
            Assert.That(sut.Build.Piece, Is.EqualTo(PieceType.WhiteKnight));
        }


        [Test]
        public void GameStateChange_Check_IsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 4].CurrentPiece = new Piece(PieceType.WhiteQueen);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7), new Position(6, 4) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // move into check 
            gameStateUpdater.UpdateGameState(new Position(6, 4), new Position(5, 5), PieceColour.Black);

            // escape check
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(1, 2), PieceColour.Black);

            Assert.That(sut.Check);
        }

        [Test]
        public void GameStateChange_PossibleMovesAreSaved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(0, 0), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.Black);
            // black makes a move and its possible moves are saved
            var sut = gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.White);
            var expectedPossibleMoves = new Dictionary<Position, ImmutableHashSet<Position>>
            {
                {
                    new Position(0, 0),
                    new HashSet<Position> { new Position(0, 1), new Position(1, 1), new Position(1, 0) }
                        .ToImmutableHashSet()
                }
            }.ToImmutableDictionary();
            Assert.That(sut.PossiblePieceMoves, Is.EquivalentTo(expectedPossibleMoves));
        }

        [Test]
        public void GameStateChange_PossibleMovesAreSaved_AndAreNotChangedByReference()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(0, 0), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.Black);
            // black makes a move and its possible moves are saved
            var sut = gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.White);
            var expectedPossibleMoves = new Dictionary<Position, ImmutableHashSet<Position>>
            {
                {
                    new Position(0, 0),
                    new HashSet<Position> { new Position(0, 1), new Position(1, 1), new Position(1, 0) }
                        .ToImmutableHashSet()
                }
            }.ToImmutableDictionary();

            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(6, 6), new Position(5, 5), PieceColour.White);
            Assert.That(sut.PossiblePieceMoves, Is.EquivalentTo(expectedPossibleMoves));
        }

        [Test]
        public void GameStateChange_PossibleBuildsAreSaved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(0, 0), new Position(7, 7) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.Black);
            // black makes a move and its possible moves are saved
            var sut = gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.White);
            var buildPieces = new HashSet<PieceType>
            {
                PieceType.BlackBishop,
                PieceType.BlackKnight,
                PieceType.BlackPawn,
                PieceType.BlackRook,
                PieceType.BlackQueen
            }.ToImmutableHashSet();

            var buildPositions = new HashSet<Position>
            {
                new Position(0, 7),
                new Position(1, 7),
                new Position(2, 7),
                new Position(3, 7),
                new Position(4, 7),
                new Position(5, 7),
                new Position(6, 7),
                new Position(7, 7),
                new Position(0, 6),
                new Position(1, 6),
                new Position(2, 6),
                new Position(3, 6),
                new Position(4, 6),
                new Position(5, 6),
                new Position(6, 6),
                new Position(7, 6)
            };
            Assert.That(sut.BuildMoves.BuildPieces, Is.EquivalentTo(buildPieces));
            Assert.That(sut.BuildMoves.BuildPositions, Is.EquivalentTo(buildPositions));
        }


        [Test]
        public void GameStateChange_PlayerStateIsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7), new Position(6, 4) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var initialPlayerState = gameState.WhiteState;
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            gameStateUpdater.UpdateGameState(new Position(6, 4), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(6, 5), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(6, 7), PieceType.WhitePawn, PieceColour.Black);
            var sut = gameStateUpdater.UpdateGameState(new Position(4, 4), PieceType.BlackPawn, PieceColour.White);


            Assert.That(sut.BlackPlayerState.BuildPoints, Is.EqualTo(initialPlayerState.BuildPoints - 1));
            Assert.That(sut.WhitePlayerState.BuildPoints, Is.EqualTo(initialPlayerState.BuildPoints - 2));
        }


        [Test]
        public void GameStateChange_TurnIsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7), new Position(6, 4) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            var sut = gameStateUpdater.UpdateGameState(new Position(6, 4), PieceType.WhitePawn, PieceColour.Black);
            Assert.That(sut.Turn, Is.EqualTo(PieceColour.Black)); // see note at the top of test
        }


        [Test]
        public void GameStateChange_ResolvedBuildsAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            var activePieces = new HashSet<Position> { new Position(1, 1), new Position(7, 7), new Position(6, 4) };
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes builds move
            gameStateUpdater.UpdateGameState(new Position(6, 4), PieceType.WhitePawn, PieceColour.Black);
            // black makes a move
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.White);
            // white makes a move and stores which pieces have been resolved  on that move 
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            var expected = new List<(Position, PieceType)> { (new Position(6, 4), PieceType.WhitePawn) };
            Assert.That(sut.ResolvedBuilds, Is.EquivalentTo(expected)); // see note at the top of test
        }
    }
}