using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.PieceInstallers;
using Bindings.Installers.PossibleMoveInstallers;
using Game.Implementations;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class RookMovesTests : ZenjectUnitTestFixture
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

        private PossibleRookMoves _whiteRookMoves;
        private PossibleRookMoves _blackRookMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleRookMovesInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var rookMovesFactory = Container.Resolve<PossibleRookMovesFactory>();
            _whiteRookMoves = rookMovesFactory.Create(PieceColour.White);
            _blackRookMoves = rookMovesFactory.Create(PieceColour.Black);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_RookCanMoveLaterallyAndVertically()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5),
                new BoardPosition(4, 6),
                new BoardPosition(4, 7)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnEmptyBoard_Black_RookCanMoveLaterallyAndVertically()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackRook, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5),
                new BoardPosition(4, 6),
                new BoardPosition(4, 7)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithOpposingPiece_White_RookCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteRook, new BoardPosition(4, 4)),
                (PieceType.BlackRook, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5),
                new BoardPosition(4, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithOpposingPiece_Black_RookCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackRook, new BoardPosition(4, 4)),
                (PieceType.WhiteRook, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5),
                new BoardPosition(4, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_RookIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteRook, new BoardPosition(4, 4)),
                (PieceType.WhiteRook, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_Black_RookIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackRook, new BoardPosition(4, 4)),
                (PieceType.BlackRook, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 4), // horizontal
                new BoardPosition(1, 4),
                new BoardPosition(2, 4),
                new BoardPosition(3, 4),
                new BoardPosition(5, 4),
                new BoardPosition(6, 4),
                new BoardPosition(7, 4),
                new BoardPosition(4, 0), // vertical
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}