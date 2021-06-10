using System.Collections.Generic;
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
    public class KingNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteKingNonTurnMoves;
        private IPieceMoveGenerator _blackKingNonTurnMoves;
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
            var possibleMovesFactory = Container.Resolve<PossibleMovesFactory>();
            _whiteKingNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteKing, false);
            _blackKingNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackKing, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void WithFriendlyPiece_White_KingCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
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
        public void WithEnemyPiece_White_KingIsNotBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKingNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
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
        public void WithFriendlyPiece_Black_KingCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
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
        public void WithEnemyPiece_Black_KingIsNotBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKingNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState);
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
    }
}