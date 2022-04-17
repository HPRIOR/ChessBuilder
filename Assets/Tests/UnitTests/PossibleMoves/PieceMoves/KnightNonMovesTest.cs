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
            var possibleMovesFactory = Container.Resolve<MovesFactory>();
            _whiteKnightNonTurnMoves = possibleMovesFactory.Create(PieceType.WhiteKnight, false);
            _blackKnightNonTurnMoves = possibleMovesFactory.Create(PieceType.BlackKnight, false);
            _boardSetup = Container.Resolve<BoardSetup>();
        }


        [Test]
        public void WithFriendlyPiece_White_KnightCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(5, 2)));
        }


        [Test]
        public void WithFriendlyPiece_Black_BishopCanDefend()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(5, 2)));
        }


        [Test]
        public void WithOpposingPiece_White_KnightIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.WhiteKnight, new Position(4, 4)),
                (PieceType.BlackPawn, new Position(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteKnightNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(5, 2)));
        }


        [Test]
        public void WithOpposingPiece_Black_KnightIsNotBlocked()
        {
            var pieces = new List<(PieceType, Position)>
            {
                (PieceType.BlackKnight, new Position(4, 4)),
                (PieceType.WhitePawn, new Position(5, 2))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _blackKnightNonTurnMoves.GetPossiblePieceMoves(new Position(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new Position(5, 2)));
        }
    }
}