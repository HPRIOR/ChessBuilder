using System.Collections.Generic;
using System.Linq;
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
    public class RookNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _whiteRookNonTurnMoves;
        private IPieceMoveGenerator _blackRookNonTurnMoves;
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
            _whiteRookNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteRook, false);
            _blackRookNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackRook, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void WithFriendlyPiece_White_RookCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteRook, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();


            Assert.That(possibleMoves, Does.Contain(new Position(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(4, 6)));
        }


        [Test]
        public void WithEnemyPiece_White_RookIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteRook, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteRookNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();


            Assert.That(possibleMoves, Does.Contain(new Position(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(4, 6)));
        }

        [Test]
        public void WithFriendlyPiece_Black_RookCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackRook, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();


            Assert.That(possibleMoves, Does.Contain(new Position(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(4, 6)));
        }


        [Test]
        public void WithEnemyPiece_Black_RookIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackRook, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(4, 5))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackRookNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();


            Assert.That(possibleMoves, Does.Contain(new Position(4, 5)));
            Assert.That(possibleMoves, Does.Not.Contains(new Position(4, 6)));
        }
    }
}