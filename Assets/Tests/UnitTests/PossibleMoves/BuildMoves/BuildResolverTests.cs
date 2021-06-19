using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
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
            board[1, 1].BuildState = new BuildState(0, PieceType.WhiteQueen);
            var boardState = new BoardState(board);
            _buildResolver.ResolveBuild(boardState);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.WhiteQueen));
        }

        [Test]
        public void WillResetBuildState_WhenPieceIsBuilt()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(0, PieceType.WhiteQueen);
            var boardState = new BoardState(board);
            _buildResolver.ResolveBuild(boardState);

            Assert.That(board[1, 1].BuildState, Is.EqualTo(new BuildState()));
        }

        [Test]
        public void WillNotBuildPiece_WhenBuildStateCountGreaterThanZero()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(1, PieceType.WhiteQueen);
            var boardState = new BoardState(board);
            _buildResolver.ResolveBuild(boardState);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void WillNotBuildPiece_WhenBuildStatePieceTypeIsNull()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(0);
            var boardState = new BoardState(board);
            _buildResolver.ResolveBuild(boardState);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void WillNotBuildPiece_WhenTileContainsPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKnight);
            board[1, 1].BuildState = new BuildState(0, PieceType.WhiteQueen);
            var boardState = new BoardState(board);
            _buildResolver.ResolveBuild(boardState);

            Assert.That(board[1, 1].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKnight));
        }
    }
}