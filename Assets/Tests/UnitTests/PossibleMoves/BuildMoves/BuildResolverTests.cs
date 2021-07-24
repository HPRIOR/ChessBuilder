using System.Collections.Generic;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Models.Services.Board;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuildResolverTests : ZenjectUnitTestFixture
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

        private IBuildResolver _buildResolver;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BuildResolverInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _buildResolver = Container.Resolve<IBuildResolver>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void WillBuildPiece_WhenBuildStateCountIsZero()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(0, PieceType.WhiteQueen);
            var activePieces = new HashSet<Position>();
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.White);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.WhiteQueen));
        }

        [Test]
        public void WillResetBuildState_WhenPieceIsBuilt()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(0, PieceType.WhiteQueen);
            var activePieces = new HashSet<Position>();
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.White);

            Assert.That(board[1, 1].BuildTileState, Is.EqualTo(new BuildTileState()));
        }

        [Test]
        public void WillNotBuildPiece_WhenBuildStateCountGreaterThanZero()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(1, PieceType.WhiteQueen);
            var activePieces = new HashSet<Position>();
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.White);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void WillNotBuildPiece_WhenBuildStatePieceTypeIsNull()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(0);
            var activePieces = new HashSet<Position>();
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.White);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void WillNotBuildPiece_WhenTileContainsPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKnight);
            board[1, 1].BuildTileState = new BuildTileState(0, PieceType.WhiteQueen);
            var activePieces = new HashSet<Position> {new Position(1, 1)};
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.White);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKnight));
        }


        [Test]
        public void WillNotBuildPiece_WhenNotTurn()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(0, PieceType.WhiteQueen);
            var activePieces = new HashSet<Position>();
            var activeBuilds = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, activeBuilds);
            _buildResolver.ResolveBuilds(boardState, PieceColour.Black);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }
    }
}