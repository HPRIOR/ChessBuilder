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
    public class KingMovesTests : ZenjectUnitTestFixture
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

        private KingTurnMoves _whiteKingTurnMoves;
        private KingTurnMoves _blackKingTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            KingTurnMovesInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var kingMoveFactory = Container.Resolve<PossibleKingMovesFactory>();
            _whiteKingTurnMoves = kingMoveFactory.Create(PieceColour.White);
            _blackKingTurnMoves = kingMoveFactory.Create(PieceColour.Black);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_KingCanMoveAroundItself()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 5),
                new BoardPosition(4, 3),
                new BoardPosition(3, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OnEmptyBoard_Black_KingCanMoveAroundItself()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 5),
                new BoardPosition(4, 3),
                new BoardPosition(3, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnCorner_White_KingMovesRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(7, 7), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 7),
                new BoardPosition(6, 6),
                new BoardPosition(7, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnCorner_Black_KingMovesRestricted()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(7, 7), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(6, 7),
                new BoardPosition(6, 6),
                new BoardPosition(7, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithEnemies_White_KingCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(4, 5)),
                (PieceType.BlackPawn, new BoardPosition(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 5),
                new BoardPosition(4, 3),
                new BoardPosition(3, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithEnemies_Black_KingCanTake()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5)),
                (PieceType.WhitePawn, new BoardPosition(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 5),
                new BoardPosition(4, 3),
                new BoardPosition(3, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriends_White_KingIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5)),
                (PieceType.WhitePawn, new BoardPosition(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriends_Black_KingIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(4, 5)),
                (PieceType.BlackPawn, new BoardPosition(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
            var expectedMoves = new List<BoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(4, 3),
                new BoardPosition(5, 4),
                new BoardPosition(5, 3),
                new BoardPosition(3, 4),
                new BoardPosition(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}