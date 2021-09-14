using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();

            Assert.That(sut.Build, Is.Null);
        }

        [Test]
        public void GameStateChange_Move_IsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();

            Assert.That(sut.Move.To, Is.EqualTo(new Position(2, 2)));
            Assert.That(sut.Move.From, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void GameStateChange_Move_IsNull_GivenBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();

            Assert.That(sut.Move, Is.Null);
        }


        [Test]
        public void GameStateChange_Build_IsCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);
            gameStateUpdater.UpdateGameState(new Position(1, 1), PieceType.WhiteKnight, PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();

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

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // move into check 
            gameStateUpdater.UpdateGameState(new Position(6, 4), new Position(5, 5), PieceColour.Black);

            // escape check
            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(1, 2), PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();

            Assert.That(sut.Check);
        }

        [Test]
        public void GameStateChange_PossibleMovesAreSaved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.Black);
            // black makes a move and its possible moves are saved
            gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.White);
            var sut = gameStateUpdater.StateHistory.Peek();
            var expectedPossibleMoves = new Dictionary<Position, HashSet<Position>>
            {
                {
                    new Position(0, 0),
                    new HashSet<Position> { new Position(0, 1), new Position(1, 1), new Position(1, 0) }
                }
            };
            Assert.That(sut.PossiblePieceMoves.Keys, Is.EquivalentTo(expectedPossibleMoves.Keys));
            Assert.That(sut.PossiblePieceMoves.Values.First(), Is.EquivalentTo(expectedPossibleMoves.Values.First()));
        }

        [Test]
        public void
            GameStateChange_PossibleMovesAreSaved_AndAreNotChangedByReference() // this will break when further mutability is introduced
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.Black);
            // black makes a move and its possible moves are saved
            gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.White);
            var sut = gameStateUpdater.StateHistory.Peek();
            var expectedPossibleMoves = new Dictionary<Position, HashSet<Position>>
            {
                {
                    new Position(0, 0),
                    new HashSet<Position> { new Position(0, 1), new Position(1, 1), new Position(1, 0) }
                }
            };

            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(6, 6), new Position(5, 5), PieceColour.White);
            Assert.That(sut.PossiblePieceMoves.Keys, Is.EquivalentTo(expectedPossibleMoves.Keys));
            Assert.That(sut.PossiblePieceMoves.Values.First(), Is.EquivalentTo(expectedPossibleMoves.Values.First()));
        }

        [Test]
        public void GameStateChange_PossibleBuildsAreSaved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes a move 
            gameStateUpdater.UpdateGameState(new Position(0, 0), new Position(1, 1), PieceColour.Black);
            // black makes a move and its possible moves are saved
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.White);
            var sut = gameStateUpdater.StateHistory.Peek();
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

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var initialPlayerState = gameState.PlayerState;
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            gameStateUpdater.UpdateGameState(new Position(6, 4), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(6, 5), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(6, 7), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(4, 4), PieceType.BlackPawn, PieceColour.White);
            var sut = gameStateUpdater.StateHistory.Peek();

            Assert.That(sut.PlayerState.BuildPoints, Is.EqualTo(initialPlayerState.BuildPoints - 1));
        }


        [Test]
        public void GameStateChange_ResolvedBuildsAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            // white makes builds move
            gameStateUpdater.UpdateGameState(new Position(6, 4), PieceType.WhitePawn, PieceColour.Black);
            // black makes a move
            gameStateUpdater.UpdateGameState(new Position(7, 7), new Position(6, 6), PieceColour.White);
            // white makes a move and stores which pieces have been resolved  on that move 
            gameStateUpdater.UpdateGameState(new Position(1, 1), new Position(2, 2), PieceColour.Black);
            var sut = gameStateUpdater.StateHistory.Peek();
            var expected = new List<(Position, PieceType)> { (new Position(6, 4), PieceType.WhitePawn) };
            Assert.That(sut.ResolvedBuilds, Is.EquivalentTo(expected));
        }

        [Test]
        public void RevertingGameState_CorrectlyReversePossibleMoves()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            var initialPossibleMoves =
                new Dictionary<Position, HashSet<Position>>(gameState.PossiblePieceMoves);

            gameStateUpdater.UpdateGameState(new Position(1, 0), new Position(1, 2), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(1, 7), new Position(2, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 2), new Position(1, 3), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(2, 7), new Position(3, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 3), new Position(1, 4), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), new Position(4, 7), PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            Assert.That(gameStateUpdater.GameState.PossiblePieceMoves, Is.EquivalentTo(initialPossibleMoves));
        }


        [Test]
        public void RevertingGameState_CorrectlyReversesPossibleBuilds()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);

            gameStateUpdater.UpdateGameState(new Position(2, 3), PieceType.WhiteQueen, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 3), PieceType.BlackQueen, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 4), PieceType.WhiteQueen, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 5), PieceType.BlackQueen, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 6), PieceType.WhiteQueen, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), PieceType.BlackQueen, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(4, 4), PieceType.WhiteQueen, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(4, 5), PieceType.BlackQueen, PieceColour.White);


            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState;
            Assert.That(
                sut.PossibleBuildMoves.BuildPieces, Is.EquivalentTo(
                    new HashSet<PieceType>
                    {
                        PieceType.WhiteBishop,
                        PieceType.WhiteKnight,
                        PieceType.WhitePawn,
                        PieceType.WhiteQueen,
                        PieceType.WhiteRook
                    }
                )
            );
        }

        [Test]
        public void RevertingGameState_CorrectlyReversesResolvedBuilds()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(2, 3), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 3), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 4), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 5), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 6), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), PieceType.BlackPawn, PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState;
            Assert.That(sut.BoardState.Board[2, 3].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 3].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 4].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 5].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 6].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 7].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void RevertingGameState_CorrectlyReversesBuildState_AfterRevertingBuilds()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(2, 3), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 3), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 4), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 5), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 6), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), PieceType.BlackPawn, PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState;
            Assert.That(sut.BoardState.Board[2, 3].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 3].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 4].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 5].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 6].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
            Assert.That(sut.BoardState.Board[3, 7].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));

            Assert.That(sut.BoardState.Board[2, 3].BuildTileState.Turns, Is.EqualTo(0));
            Assert.That(sut.BoardState.Board[3, 3].BuildTileState.Turns, Is.EqualTo(0));
            Assert.That(sut.BoardState.Board[3, 4].BuildTileState.Turns, Is.EqualTo(0));
            Assert.That(sut.BoardState.Board[3, 5].BuildTileState.Turns, Is.EqualTo(0));
            Assert.That(sut.BoardState.Board[3, 6].BuildTileState.Turns, Is.EqualTo(0));
            Assert.That(sut.BoardState.Board[3, 7].BuildTileState.Turns, Is.EqualTo(0));
        }


        [Test]
        public void RevertingGameState_CorrectlyReversesDecrements()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(2, 3), PieceType.WhiteQueen, PieceColour.Black); // 9
            gameStateUpdater.UpdateGameState(new Position(3, 3), PieceType.BlackPawn, PieceColour.White); // 8
            gameStateUpdater.UpdateGameState(new Position(3, 4), PieceType.WhitePawn, PieceColour.Black); // 7
            gameStateUpdater.UpdateGameState(new Position(3, 5), PieceType.BlackPawn, PieceColour.White); // 6
            gameStateUpdater.UpdateGameState(new Position(3, 6), PieceType.WhitePawn, PieceColour.Black); // 5
            gameStateUpdater.UpdateGameState(new Position(3, 7), PieceType.BlackPawn, PieceColour.White); // 4

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState;
            Assert.That(sut.BoardState.Board[2, 3].BuildTileState.Turns, Is.EqualTo(7));
        }


        [Test]
        public void RevertingGameState_DoesNotIncrementBlockedBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(0, 0), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 3), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 4), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 5), PieceType.BlackPawn, PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(3, 6), PieceType.WhitePawn, PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), PieceType.BlackPawn, PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState;
            Assert.That(sut.BoardState.Board[0, 0].BuildTileState.Turns, Is.EqualTo(0));
        }


        [Test]
        public void RevertingGameState_CorrectlyReversesBoardPieces()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(1, 0), new Position(1, 2), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(1, 7), new Position(2, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 2), new Position(1, 3), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(2, 7), new Position(3, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 3), new Position(1, 4), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), new Position(4, 7), PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState.BoardState.Board;
            Assert.That(sut[0, 0].CurrentPiece.Type, Is.EqualTo(PieceType.WhiteKing));
            Assert.That(sut[1, 0].CurrentPiece.Type, Is.EqualTo(PieceType.WhitePawn));

            Assert.That(sut[0, 7].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKing));
            Assert.That(sut[1, 7].CurrentPiece.Type, Is.EqualTo(PieceType.BlackPawn));
        }


        [Test]
        public void RevertingGameState_CorrectlyReversesActivePieces()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 0].CurrentPiece = new Piece(PieceType.WhitePawn);

            board[0, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 7].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);
            var gameState = _gameStateInitializer.InitialiseGame(boardState);
            var gameStateUpdater = _gameStateUpdaterFactory.Create(gameState);


            gameStateUpdater.UpdateGameState(new Position(1, 0), new Position(1, 2), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(1, 7), new Position(2, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 2), new Position(1, 3), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(2, 7), new Position(3, 7), PieceColour.White);
            gameStateUpdater.UpdateGameState(new Position(1, 3), new Position(1, 4), PieceColour.Black);
            gameStateUpdater.UpdateGameState(new Position(3, 7), new Position(4, 7), PieceColour.White);

            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();
            gameStateUpdater.RevertGameState();

            var sut = gameStateUpdater.GameState.BoardState.ActivePieces;
            Assert.That(sut, Is.EquivalentTo(new HashSet<Position>
            {
                new Position(1, 2),
                new Position(2, 7),
                new Position(0, 0),
                new Position(0, 7)
            }));
        }
    }
}