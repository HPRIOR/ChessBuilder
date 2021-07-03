using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuildPointsCalculatorTests : ZenjectUnitTestFixture
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

        private IBuildPointsCalculator _buildPointsCalculator;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BuildPointsCalculatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _buildPointsCalculator = Container.Resolve<IBuildPointsCalculator>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void PointsAreSubtracted_ByBuildingPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(PieceType.BlackQueen);
            var boardState = new BoardState(board);

            const int maxPoints = 10;
            var playerState = _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, boardState, maxPoints);

            var expectedPoints = maxPoints - BuildPoints.PieceCost[PieceType.BlackQueen];
            Assert.That(playerState, Is.EqualTo(new PlayerState(expectedPoints)));
        }


        [Test]
        public void PointsAreSubtracted_ByBuildPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackQueen);
            var boardState = new BoardState(board);

            const int maxPoints = 10;
            var playerState = _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, boardState, maxPoints);

            var expectedPoints = maxPoints - BuildPoints.PieceCost[PieceType.BlackQueen];
            Assert.That(playerState, Is.EqualTo(new PlayerState(expectedPoints)));
        }


        [Test]
        public void PointsAreSubtracted_ByBuiltPiece_AndBuildingPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(PieceType.BlackQueen);
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackPawn);
            var boardState = new BoardState(board);

            const int maxPoints = 10;
            var playerState = _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, boardState, maxPoints);

            var expectedPoints = maxPoints - BuildPoints.PieceCost[PieceType.BlackQueen] -
                                 BuildPoints.PieceCost[PieceType.BlackPawn];
            Assert.That(playerState, Is.EqualTo(new PlayerState(expectedPoints)));
        }

        [Test]
        public void PointsAreSubtracted_ByBuiltPiece_AndBuildingPiece_ForBothColours([Values] PieceColour pieceColour)
        {
            var pawn = pieceColour == PieceColour.Black ? PieceType.BlackPawn : PieceType.WhitePawn;
            var queen = pieceColour == PieceColour.Black ? PieceType.BlackQueen : PieceType.WhiteQueen;

            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildState = new BuildState(queen);
            board[2, 2].CurrentPiece = new Piece(pawn);
            var boardState = new BoardState(board);

            const int maxPoints = 10;
            var playerState = _buildPointsCalculator.CalculateBuildPoints(pieceColour, boardState, maxPoints);

            var expectedPoints = maxPoints - BuildPoints.PieceCost[queen] -
                                 BuildPoints.PieceCost[pawn];
            Assert.That(playerState, Is.EqualTo(new PlayerState(expectedPoints)));
        }

        [Test]
        public void KingPieceDoesNotCount()
        {
            var board = _boardGenerator.GenerateBoard();
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackKing);
            var boardState = new BoardState(board);

            const int maxPoints = 10;
            var playerState = _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, boardState, maxPoints);

            Assert.That(playerState, Is.EqualTo(new PlayerState(maxPoints)));
        }
    }
}