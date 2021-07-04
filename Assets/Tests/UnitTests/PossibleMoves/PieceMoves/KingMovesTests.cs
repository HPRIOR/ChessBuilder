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

        private IPieceMoveGenerator _whiteKingTurnMoves;
        private IPieceMoveGenerator _blackKingTurnMoves;
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
            var possibleMovesFactory = Container.Resolve<MovesFactory>();
            _whiteKingTurnMoves = possibleMovesFactory.Create(PieceType.WhiteKing, true);
            _blackKingTurnMoves = possibleMovesFactory.Create(PieceType.BlackKing, true);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_KingCanMoveAroundItself()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKing, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 5),
                new Position(4, 3),
                new Position(3, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OnEmptyBoard_Black_KingCanMoveAroundItself()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKing, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 5),
                new Position(4, 3),
                new Position(3, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnCorner_White_KingMovesRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKing, new Position(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new Position(7, 7), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 7),
                new Position(6, 6),
                new Position(7, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnCorner_Black_KingMovesRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKing, new Position(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new Position(7, 7), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 7),
                new Position(6, 6),
                new Position(7, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithEnemies_White_KingCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKing, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(4, 5)),
                (PieceType.BlackPawn, new Position(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 5),
                new Position(4, 3),
                new Position(3, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithEnemies_Black_KingCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKing, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(4, 5)),
                (PieceType.WhitePawn, new Position(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 5),
                new Position(4, 3),
                new Position(3, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriends_White_KingIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKing, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(4, 5)),
                (PieceType.WhitePawn, new Position(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriends_Black_KingIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKing, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(4, 5)),
                (PieceType.BlackPawn, new Position(3, 3))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 5),
                new Position(4, 3),
                new Position(5, 4),
                new Position(5, 3),
                new Position(3, 4),
                new Position(3, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}