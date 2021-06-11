using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Game.Implementations;
using Models.Services.Interfaces;
using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class PawnNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _blackNonTurnPawnMoves;
        private IPieceMoveGenerator _whiteNonTurnPawnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardSetup = Container.Resolve<BoardSetup>();
            var possibleMovesFactory = Container.Resolve<PossibleMovesFactory>();

            _blackNonTurnPawnMoves = possibleMovesFactory.Create(PieceType.BlackPawn, false);
            _whiteNonTurnPawnMoves = possibleMovesFactory.Create(PieceType.WhitePawn, false);
        }

        [Test]
        public void OnEmptyBoard_White_PawnGeneratesMovesOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 5)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 5)));
        }

        [Test]
        public void OnEmptyBoard_White_PawnDoesNotGenerateMoveForward()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 5)));
        }

        [Test]
        public void OnEmptyBoard_White_PawnDoesNotGenerateOutOfBounds()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(5, 5)),
                (PieceType.WhitePawn, new BoardPosition(3, 5))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMovesOne = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(7, 3), boardState)
                .ToList();

            var possibleMovesTwo = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(0, 3), boardState)
                .ToList();

            Assert.That(possibleMovesOne, Does.Contain(new BoardPosition(6, 4)));
            Assert.That(possibleMovesOne.Count(), Is.EqualTo(1));

            Assert.That(possibleMovesTwo, Does.Contain(new BoardPosition(1, 4)));
            Assert.That(possibleMovesTwo.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WithFriendlyPieceOnBoard_White_PawnCanDefendOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(3, 5)),
                (PieceType.WhitePawn, new BoardPosition(5, 5))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 5)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 5)));
        }

        [Test]
        public void WithOpposingPieceOnBoard_White_PawnIsNotBlockedOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(3, 5)),
                (PieceType.BlackPawn, new BoardPosition(5, 5))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 5)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 5)));
        }


        [Test]
        public void OnEmptyBoard_Black_PawnGeneratesMovesOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 4))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 3)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 3)));
        }

        [Test]
        public void OnEmptyBoard_Black_PawnDoesNotGenerateMoveForward()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 3)));
        }

        [Test]
        public void OnEmptyBoard_Black_PawnDoesNotGenerateOutOfBounds()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackPawn, new BoardPosition(7, 3)),
                (PieceType.BlackPawn, new BoardPosition(0, 3))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMovesOne = _blackNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(7, 3), boardState)
                .ToList();

            var possibleMovesTwo = _blackNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(0, 3), boardState)
                .ToList();

            Assert.That(possibleMovesOne, Does.Contain(new BoardPosition(6, 2)));
            Assert.That(possibleMovesOne.Count(), Is.EqualTo(1));

            Assert.That(possibleMovesTwo, Does.Contain(new BoardPosition(1, 2)));
            Assert.That(possibleMovesTwo.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WithFriendlyPieceOnBoard_Black_PawnCanDefendOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(3, 3)),
                (PieceType.BlackPawn, new BoardPosition(5, 3))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 3)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 3)));
        }

        [Test]
        public void WithOpposingPieceOnBoard_Black_PawnIsNotBlockedOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(3, 3)),
                (PieceType.WhitePawn, new BoardPosition(5, 3))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 3)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 3)));
        }
    }
}