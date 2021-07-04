using System.Collections.Generic;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Game.Implementations;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
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

        private IPieceMoveGenerator _whiteRookTurnMoves;
        private IPieceMoveGenerator _blackRookTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var possibleMovesFactory = Container.Resolve<MovesFactory>();
            _whiteRookTurnMoves = possibleMovesFactory.Create(PieceType.WhiteRook, true);
            _blackRookTurnMoves = possibleMovesFactory.Create(PieceType.BlackRook, true);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_RookCanMoveLaterallyAndVertically()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteQueen, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5),
                new Position(4, 6),
                new Position(4, 7)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnEmptyBoard_Black_RookCanMoveLaterallyAndVertically()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackRook, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5),
                new Position(4, 6),
                new Position(4, 7)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithOpposingPiece_White_RookCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteRook, new Position(4, 4)),
                (PieceType.BlackRook, new Position(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5),
                new Position(4, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithOpposingPiece_Black_RookCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackRook, new Position(4, 4)),
                (PieceType.WhiteRook, new Position(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5),
                new Position(4, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_RookIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteRook, new Position(4, 4)),
                (PieceType.WhiteRook, new Position(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_Black_RookIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackRook, new Position(4, 4)),
                (PieceType.BlackRook, new Position(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 4), // horizontal
                new Position(1, 4),
                new Position(2, 4),
                new Position(3, 4),
                new Position(5, 4),
                new Position(6, 4),
                new Position(7, 4),
                new Position(4, 0), // vertical
                new Position(4, 1),
                new Position(4, 2),
                new Position(4, 3),
                new Position(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}