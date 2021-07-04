using System.Collections.Generic;
using System.Linq;
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
    public class BishopNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteBishopNonTurnMoves;
        private IPieceMoveGenerator _blackBishopNonTurnMoves;
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
            var possibleMovesFactory = Container.Resolve<MovesFactory>();
            _whiteBishopNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteBishop, false);
            _blackBishopNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackBishop, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }


        [Test]
        public void WithFriendlyPiece_White_BishopCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteBishop, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(7, 7)));
        }


        [Test]
        public void WithFriendlyPiece_Black_BishopCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackBishop, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(7, 7)));
        }


        [Test]
        public void WithOpposingPiece_White_BishopIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteBishop, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteBishopNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(7, 7)));
        }


        [Test]
        public void WithOpposingPiece_Black_BishopIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackBishop, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(6, 6))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackBishopNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(6, 6)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(7, 7)));
        }
    }
}