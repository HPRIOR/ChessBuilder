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
    public class QueenNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteQueenNonTurnMoves;
        private IPieceMoveGenerator _blackQueenNonTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var possibleMovesFactory = Container.Resolve<PossibleMovesFactory>();
            _whiteQueenNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteQueen, false);
            _blackQueenNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackQueen, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void WithFriendlyPiece_White_QueenCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 6)));
        }


        [Test]
        public void WithEnemyPiece_White_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteQueen, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteQueenNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 7)));
        }

        [Test]
        public void WithFriendlyPiece_Black_QueenCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 6)));
        }


        [Test]
        public void WithEnemyPiece_Black_QueenIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackQueen, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(4, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackQueenNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(4, 7)));
        }
    }
}