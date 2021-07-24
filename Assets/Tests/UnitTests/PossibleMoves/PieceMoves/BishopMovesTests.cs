using System.Collections.Generic;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Game.Implementations;
using Models.Services.Moves.Factories;
using Models.Services.Moves.Interfaces;
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

        private IPieceMoveGenerator _whiteBishopTurnMoves;
        private IPieceMoveGenerator _blackBishopTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            var bishopMovesFactory = Container.Resolve<MovesFactory>();
            _whiteBishopTurnMoves = bishopMovesFactory.Create(PieceType.WhiteBishop, true);
            _blackBishopTurnMoves = bishopMovesFactory.Create(PieceType.BlackBishop, true);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void OnEmptyBoard_White_BishopCanMove()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteBishop, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OnEmptyBoard_Black_BishopCanMove()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackBishop, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithOpposingPiece_White_BishopCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteBishop, new Position(4, 4)),
                (PieceType.BlackKnight, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithOpposingPiece_Black_BishopCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackBishop, new Position(4, 4)),
                (PieceType.WhiteKnight, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(6, 6),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_BishopIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteBishop, new Position(4, 4)),
                (PieceType.WhiteBishop, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WithFriendlyPiece_Black_BishopIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackBishop, new Position(4, 4)),
                (PieceType.BlackBishop, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(5, 5),
                new Position(5, 3),
                new Position(6, 2),
                new Position(7, 1),
                new Position(3, 5),
                new Position(2, 6),
                new Position(1, 7)
            };
            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}