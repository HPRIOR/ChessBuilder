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
    public class KnightNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteKnightNonTurnMoves;
        private IPieceMoveGenerator _blackKnightNonTurnMoves;
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
            _whiteKnightNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteKnight, false);
            _blackKnightNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackKnight, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }


        [Test]
        public void WithFriendlyPiece_White_KnightCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 2)));
        }


        [Test]
        public void WithFriendlyPiece_Black_BishopCanDefend()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 2)));
        }


        [Test]
        public void WithOpposingPiece_White_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKnight, new BoardPosition(4, 4)),
                (PieceType.BlackPawn, new BoardPosition(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(5, 2)));
        }


        [Test]
        public void WithOpposingPiece_Black_KnightIsBlocked()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKnight, new BoardPosition(4, 4)),
                (PieceType.WhitePawn, new BoardPosition(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightNonTurnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Not.Contains(new BoardPosition(5, 2)));
        }
    }
}