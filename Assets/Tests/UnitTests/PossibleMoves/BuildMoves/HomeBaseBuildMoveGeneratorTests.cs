using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class HomeBaseBuildMoveGeneratorTests : ZenjectUnitTestFixture
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

        private IBuildMoveGenerator _homeBaseBuildGenerator;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _homeBaseBuildGenerator = Container.Resolve<IBuildMoveGenerator>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void OnEmptyBoard_BlackBuildZoneGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var blackBuildZone =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.Black, new PlayerState(100));
            var expectedPieces =
                new HashSet<PieceType>
                {
                    PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackPawn, PieceType.BlackQueen,
                    PieceType.BlackRook
                };

            var expectedResult = new Dictionary<Position, HashSet<PieceType>>
            {
                {new Position(7, 7), expectedPieces},
                {new Position(6, 7), expectedPieces},
                {new Position(5, 7), expectedPieces},
                {new Position(4, 7), expectedPieces},
                {new Position(3, 7), expectedPieces},
                {new Position(2, 7), expectedPieces},
                {new Position(1, 7), expectedPieces},
                {new Position(0, 7), expectedPieces},
                {new Position(7, 6), expectedPieces},
                {new Position(6, 6), expectedPieces},
                {new Position(5, 6), expectedPieces},
                {new Position(4, 6), expectedPieces},
                {new Position(3, 6), expectedPieces},
                {new Position(2, 6), expectedPieces},
                {new Position(1, 6), expectedPieces},
                {new Position(0, 6), expectedPieces}
            };

            Assert.That(blackBuildZone, Is.EquivalentTo(expectedResult));
        }


        [Test]
        public void OnEmptyBoard_WhiteBuildZoneGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var blackBuildZone =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(100));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteQueen,
                PieceType.WhiteRook
            };

            var expectedResult = new Dictionary<Position, HashSet<PieceType>>
            {
                {new Position(7, 1), expectedPieces},
                {new Position(6, 1), expectedPieces},
                {new Position(5, 1), expectedPieces},
                {new Position(4, 1), expectedPieces},
                {new Position(3, 1), expectedPieces},
                {new Position(2, 1), expectedPieces},
                {new Position(1, 1), expectedPieces},
                {new Position(0, 1), expectedPieces},
                {new Position(7, 0), expectedPieces},
                {new Position(6, 0), expectedPieces},
                {new Position(5, 0), expectedPieces},
                {new Position(4, 0), expectedPieces},
                {new Position(3, 0), expectedPieces},
                {new Position(2, 0), expectedPieces},
                {new Position(1, 0), expectedPieces},
                {new Position(0, 0), expectedPieces}
            };

            Assert.That(blackBuildZone, Is.EquivalentTo(expectedResult));
        }


        [Test]
        public void WithLimitedPoints_PieceCannotBeBuilt()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var blackBuildZone =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(8));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteRook
            };

            var expectedResult = new Dictionary<Position, HashSet<PieceType>>
            {
                {new Position(7, 1), expectedPieces},
                {new Position(6, 1), expectedPieces},
                {new Position(5, 1), expectedPieces},
                {new Position(4, 1), expectedPieces},
                {new Position(3, 1), expectedPieces},
                {new Position(2, 1), expectedPieces},
                {new Position(1, 1), expectedPieces},
                {new Position(0, 1), expectedPieces},
                {new Position(7, 0), expectedPieces},
                {new Position(6, 0), expectedPieces},
                {new Position(5, 0), expectedPieces},
                {new Position(4, 0), expectedPieces},
                {new Position(3, 0), expectedPieces},
                {new Position(2, 0), expectedPieces},
                {new Position(1, 0), expectedPieces},
                {new Position(0, 0), expectedPieces}
            };

            Assert.That(blackBuildZone, Is.EquivalentTo(expectedResult));
        }
    }
}