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

        private QueenTurnMoves _blackQueenTurnMoves;
        private QueenTurnMoves _whiteQueenTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            QueenTurnMovesInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var queenMovesFactory = Container.Resolve<PossibleQueenMovesFactory>();
            _blackQueenTurnMoves = queenMovesFactory.Create(PieceColour.Black);
            _whiteQueenTurnMoves = queenMovesFactory.Create(PieceColour.White);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_QueenCanMoveAnywhere()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
        public void OnEmptyBoard_Black_QueenCanMoveAnywhere()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
        public void WithOpposingPiece_White_QueenCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4)),
                (PieceType.BlackQueen, new BoardPosition(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
        public void WithOpposingPiece_Black_QueenCanTakeAndIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(4, 4)),
                (PieceType.WhiteQueen, new BoardPosition(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
        public void WithFriendlyPiece_White_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4)),
                (PieceType.WhiteQueen, new BoardPosition(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
        public void WithFriendlyPiece_Black_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(4, 4)),
                (PieceType.BlackQueen, new BoardPosition(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0), // bottom-left to top-right diagonal
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(7, 1), // bottom-right to top-left diagonal
                new BoardPosition(6, 2),
                new BoardPosition(5, 3),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7),
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
    }
}