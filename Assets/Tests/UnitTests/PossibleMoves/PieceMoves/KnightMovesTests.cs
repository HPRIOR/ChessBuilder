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
    public class KnightMovesTests : ZenjectUnitTestFixture
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

        private PossibleKnightMoves _whiteKnightMoves;
        private PossibleKnightMoves _blackKnightMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleKnightMovesInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardSetup = Container.Resolve<BoardSetup>();
            var knightMovesFactory = Container.Resolve<PossibleKnightMovesFactory>();
            _whiteKnightMoves = knightMovesFactory.Create(PieceColour.White);
            _blackKnightMoves = knightMovesFactory.Create(PieceColour.Black);
        }

        [Test]
        public void OnEmptyBoard_White_KnightCanMove()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnEmptyBoard_Black_KnightCanMove()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(4, 4)),
                (PieceType.WhiteKnight, new BoardPosition(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_Black_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(4, 4)),
                (PieceType.BlackKnight, new BoardPosition(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OpposingPiece_White_KnightCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(4, 4)),
                (PieceType.BlackKnight, new BoardPosition(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OpposingPiece_Black_KnightCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(4, 4)),
                (PieceType.WhiteKnight, new BoardPosition(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5),
                new BoardPosition(6, 3),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2),
                new BoardPosition(2, 3),
                new BoardPosition(2, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceInCorner_White_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightMoves.GetPossiblePieceMoves(new BoardPosition(7, 7), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceInCorner_Black_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightMoves.GetPossiblePieceMoves(new BoardPosition(7, 7), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 6),
                new BoardPosition(6, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceOnSide_White_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(7, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightMoves.GetPossiblePieceMoves(new BoardPosition(7, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 6),
                new BoardPosition(5, 5),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceOnSide_Black_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(7, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightMoves.GetPossiblePieceMoves(new BoardPosition(7, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 6),
                new BoardPosition(5, 5),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}