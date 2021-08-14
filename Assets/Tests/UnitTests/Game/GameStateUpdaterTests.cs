using System.Collections.Generic;
using Bindings.Utils;
using Models.Services.Board;
using Models.Services.Game.Implementations;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
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
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.White);

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
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.White);

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
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.White);

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
            var sut = gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.White);

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
    }
}