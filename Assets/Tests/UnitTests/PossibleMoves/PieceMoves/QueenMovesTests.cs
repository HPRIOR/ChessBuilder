using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Game.Implementations;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class QueenMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _blackQueenTurnMoves;
        private IPieceMoveGenerator _whiteQueenTurnMoves;
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
            _blackQueenTurnMoves = possibleMovesFactory.Create(PieceType.BlackQueen, true);
            _whiteQueenTurnMoves = possibleMovesFactory.Create(PieceType.WhiteQueen, true);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_QueenCanMoveAnywhere()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteQueen, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
        public void OnEmptyBoard_Black_QueenCanMoveAnywhere()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackQueen, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
        public void WithOpposingPiece_White_QueenCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteQueen, new Position(4, 4)),
                (PieceType.BlackQueen, new Position(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
        public void WithOpposingPiece_Black_QueenCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackQueen, new Position(4, 4)),
                (PieceType.WhiteQueen, new Position(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
        public void WithFriendlyPiece_White_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteQueen, new Position(4, 4)),
                (PieceType.WhiteQueen, new Position(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
        public void WithFriendlyPiece_Black_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackQueen, new Position(4, 4)),
                (PieceType.BlackQueen, new Position(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0), // bottom-left to top-right diagonal
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(7, 1), // bottom-right to top-left diagonal
                new Position(6, 2),
                new Position(5, 3),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7),
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
    }
}