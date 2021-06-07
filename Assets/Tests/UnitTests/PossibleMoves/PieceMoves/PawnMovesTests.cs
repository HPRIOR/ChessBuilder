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
    public class PawnMovesTests : ZenjectUnitTestFixture
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

        private PawnTurnMoves _whitePawnTurnMoves;
        private PawnTurnMoves _blackPawnTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            PawnTurnMovesInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var pawnMovesFactory = Container.Resolve<PossiblePawnMovesFactory>();
            _whitePawnTurnMoves = pawnMovesFactory.Create(PieceColour.White);
            _blackPawnTurnMoves = pawnMovesFactory.Create(PieceColour.Black);

            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void White_PawnCanMoveForward()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 1))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 1), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(4, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnCanMoveForward()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 6), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(4, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void White_PawnCanTakeDiagonally()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 1)),
                (PieceType.BlackPawn, new BoardPosition(5, 2)),
                (PieceType.BlackPawn, new BoardPosition(3, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 1), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(4, 2),
                new BoardPosition(5, 2),
                new BoardPosition(3, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnCanTakeDiagonally()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 6)),
                (PieceType.WhitePawn, new BoardPosition(3, 5)),
                (PieceType.WhitePawn, new BoardPosition(5, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 6), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(4, 5),
                new BoardPosition(5, 5),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void White_PawnIsBlockedByFriendlyPiece()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 1)),
                (PieceType.WhitePawn, new BoardPosition(4, 2))
            };


            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whitePawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 1), boardState);
            var expectedMoves = new List<BoardPosition>();

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void Black_PawnIsBlockedByFriendlyPiece()
        {
            var pieces = new List<(PieceType, BoardPosition )>
            {
                (PieceType.BlackPawn, new BoardPosition(4, 6)),
                (PieceType.BlackPawn, new BoardPosition(4, 5))
            };


            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackPawnTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 6), boardState);
            var expectedMoves = new List<BoardPosition>();

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}