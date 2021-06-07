using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.PieceInstallers;
using Bindings.Installers.PossibleMoveInstallers;
using Game.Implementations;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class BishopMovesTests : ZenjectUnitTestFixture
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

        private BishopTurnMoves _whiteBishopTurnMoves;
        private BishopTurnMoves _blackBishopTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            BishopTurnMovesInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var bishopMovesFactory = Container.Resolve<PossibleBishopMovesFactory>();
            _whiteBishopTurnMoves = bishopMovesFactory.Create(PieceColour.White);
            _blackBishopTurnMoves = bishopMovesFactory.Create(PieceColour.Black);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_BishopCanMove()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteBishop, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OnEmptyBoard_Black_BishopCanMove()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackBishop, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithOpposingPiece_White_BishopCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteBishop, new BoardPosition(4, 4)),
                (PieceType.BlackKnight, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithOpposingPiece_Black_BishopCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackBishop, new BoardPosition(4, 4)),
                (PieceType.WhiteKnight, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_BishopIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteBishop, new BoardPosition(4, 4)),
                (PieceType.WhiteBishop, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithFriendlyPiece_Black_BishopIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackBishop, new BoardPosition(4, 4)),
                (PieceType.BlackBishop, new BoardPosition(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(5, 5),
                new BoardPosition(5, 3),
                new BoardPosition(6, 2),
                new BoardPosition(7, 1),
                new BoardPosition(3, 5),
                new BoardPosition(2, 6),
                new BoardPosition(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}