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

        private IPieceMoveGenerator _whiteKnightTurnMoves;
        private IPieceMoveGenerator _blackKnightTurnMoves;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            PossibleMovesFactoryInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardSetup = Container.Resolve<BoardSetup>();
            var possibleMovesFactory = Container.Resolve<MovesFactory>();
            _whiteKnightTurnMoves = possibleMovesFactory.Create(PieceType.WhiteKnight, true);
            _blackKnightTurnMoves = possibleMovesFactory.Create(PieceType.BlackKnight, true);
        }

        [Test]
        public void OnEmptyBoard_White_KnightCanMove()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OnEmptyBoard_Black_KnightCanMove()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_White_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(4, 4)),
                (PieceType.WhiteKnight, new Position(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithFriendlyPiece_Black_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(4, 4)),
                (PieceType.BlackKnight, new Position(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void OpposingPiece_White_KnightCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(4, 4)),
                (PieceType.BlackKnight, new Position(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void OpposingPiece_Black_KnightCanTake()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(4, 4)),
                (PieceType.WhiteKnight, new Position(5, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5),
                new Position(6, 3),
                new Position(5, 2),
                new Position(3, 2),
                new Position(2, 3),
                new Position(2, 5),
                new Position(3, 6)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceInCorner_White_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightTurnMoves.GetPossiblePieceMoves(new Position(7, 7), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceInCorner_Black_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(7, 7))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightTurnMoves.GetPossiblePieceMoves(new Position(7, 7), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(5, 6),
                new Position(6, 5)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceOnSide_White_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(7, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightTurnMoves.GetPossiblePieceMoves(new Position(7, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 6),
                new Position(5, 5),
                new Position(5, 3),
                new Position(6, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }


        [Test]
        public void WithPieceOnSide_Black_KnightIsRestricted()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(7, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightTurnMoves.GetPossiblePieceMoves(new Position(7, 4), boardState);
            var expectedMoves = new List<Position>
            {
                new Position(6, 6),
                new Position(5, 5),
                new Position(5, 3),
                new Position(6, 2)
            };

            Assert.That(possibleMoves, Is.EquivalentTo(expectedMoves));
        }
    }
}