using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using UnityEngine;
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

            var blackBuildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.Black, new PlayerState(100));
            var expectedPieces =
                new HashSet<PieceType>
                {
                    PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackPawn, PieceType.BlackQueen,
                    PieceType.BlackRook
                };

            var expectedPositions = new HashSet<Position>
            {
                new Position(7, 7),
                new Position(6, 7),
                new Position(5, 7),
                new Position(4, 7),
                new Position(3, 7),
                new Position(2, 7),
                new Position(1, 7),
                new Position(0, 7),
                new Position(7, 6),
                new Position(6, 6),
                new Position(5, 6),
                new Position(4, 6),
                new Position(3, 6),
                new Position(2, 6),
                new Position(1, 6),
                new Position(0, 6)
            };


            Assert.That(blackBuildMoves.BuildPositions, Is.EquivalentTo(expectedPositions));
            Assert.That(blackBuildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }


        [Test]
        public void OnEmptyBoard_WhiteBuildZoneGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var whiteBuildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(100));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteQueen,
                PieceType.WhiteRook
            };

            var expectedPositions = new HashSet<Position>
            {
                new Position(7, 1),
                new Position(6, 1),
                new Position(5, 1),
                new Position(4, 1),
                new Position(3, 1),
                new Position(2, 1),
                new Position(1, 1),
                new Position(0, 1),
                new Position(7, 0),
                new Position(6, 0),
                new Position(5, 0),
                new Position(4, 0),
                new Position(3, 0),
                new Position(2, 0),
                new Position(1, 0),
                new Position(0, 0)
            };

            whiteBuildMoves.BuildPositions.ToList().ForEach(x => Debug.Log(x));
            Assert.That(whiteBuildMoves.BuildPositions, Is.EquivalentTo(expectedPositions));
            Assert.That(whiteBuildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }


        [Test]
        public void WithLimitedPoints_PieceCannotBeBuilt()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var buildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(8));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteRook
            };

            var expectPositions = new HashSet<Position>
            {
                new Position(7, 1),
                new Position(6, 1),
                new Position(5, 1),
                new Position(4, 1),
                new Position(3, 1),
                new Position(2, 1),
                new Position(1, 1),
                new Position(0, 1),
                new Position(7, 0),
                new Position(6, 0),
                new Position(5, 0),
                new Position(4, 0),
                new Position(3, 0),
                new Position(2, 0),
                new Position(1, 0),
                new Position(0, 0)
            };

            Assert.That(buildMoves.BuildPositions, Is.EquivalentTo(expectPositions));
            Assert.That(buildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }


        [Test]
        public void WithOnePoint_OnlyPawnCanBeBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var buildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(1));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhitePawn
            };


            Assert.That(buildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }


        [Test]
        public void WithThreePoints_PawnBishopKnightCanBeBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var buildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(3));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhitePawn, PieceType.WhiteBishop, PieceType.WhiteKnight
            };

            Assert.That(buildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }

        [Test]
        public void WithFivePoints_PawnBishopKnightRookCanBeBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);

            var buildMoves =
                _homeBaseBuildGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(5));
            var expectedPieces = new HashSet<PieceType>
            {
                PieceType.WhitePawn, PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhiteRook
            };

            Assert.That(buildMoves.BuildPieces, Is.EquivalentTo(expectedPieces));
        }
    }
}