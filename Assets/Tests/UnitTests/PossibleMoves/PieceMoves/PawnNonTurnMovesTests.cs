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
    public class PawnNonTurnMovesTests : ZenjectUnitTestFixture
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

        private IPieceMoveGenerator _blackNonTurnPawnMoves;
        private IPieceMoveGenerator _whiteNonTurnPawnMoves;
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
            _boardSetup = Container.Resolve<BoardSetup>();
            var possibleMovesFactory = Container.Resolve<PossibleMovesFactory>();

            _blackNonTurnPawnMoves = possibleMovesFactory.Create(PieceType.BlackPawn, false);
            _whiteNonTurnPawnMoves = possibleMovesFactory.Create(PieceType.WhitePawn, false);
        }

        [Test]
        public void OnEmptyBoard_White_PawnGeneratesMovesOnEitherSide()
        {
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhitePawn, new BoardPosition(4, 4))
            };
            var boardState = _boardSetup.SetupBoardWith(pieces);

            var possibleMoves = _whiteNonTurnPawnMoves.GetPossiblePieceMoves(new BoardPosition(4, 4), boardState)
                .ToList();

            Assert.That(possibleMoves, Does.Contain(new BoardPosition(3, 5)));
            Assert.That(possibleMoves, Does.Contain(new BoardPosition(5, 5)));
        }
    }
}