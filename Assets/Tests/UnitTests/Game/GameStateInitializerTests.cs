using System.Collections.Generic;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Game.Implementations;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
    [TestFixture]
    public class GameStateInitializerTests : ZenjectUnitTestFixture
    {
        private GameInitializer _gameInitializer;

        private void InstallBindings()
        {
            MovesGeneratorRepositoryInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            GameInitializerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _gameInitializer = Container.Resolve<GameInitializer>();
        }

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

        [Test]
        public void WillCreateGameState_WithCorrectMoves()
        {
            var piecesDict = new Dictionary<Position, PieceType>
            {
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(5, 5), PieceType.WhitePawn },
                { new Position(0, 0), PieceType.WhiteKing },
            };
            var boardState = new BoardState(piecesDict);

            var sut = _gameInitializer.InitialiseGame(boardState);

            var expectedKingMoves = new List<Position>()
            {
                new Position(0,1),
                new Position(1,1),
                new Position(1,0),
            };
            var expectedPawnMoves = new List<Position>()
            {
                new Position(5,6)
            };
            Assert.That(sut.PossiblePieceMoves[new Position(0,0)], Is.EquivalentTo(expectedKingMoves));
            Assert.That(sut.PossiblePieceMoves[new Position(5,5)], Is.EquivalentTo(expectedPawnMoves));
        }

        [Test]
        public void WillCreateGameState_WithCorrectBuildPoints()
        {
            var piecesDict = new Dictionary<Position, PieceType>
            {
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(5, 5), PieceType.WhitePawn },
                { new Position(6,6), PieceType.WhiteQueen },
                { new Position(0, 0), PieceType.WhiteKing },
            };
            var boardState = new BoardState(piecesDict);

            var sut = _gameInitializer.InitialiseGame(boardState);
            var buildPoints = sut.PlayerState.BuildPoints;
            
            // Todo inject total build points
            Assert.That(buildPoints, Is.EqualTo(39 - PieceType.WhitePawn.Value() - PieceType.WhiteQueen.Value()));
        }
        
        [Test]
        public void WillCreateGameState_WithMultipleOfTheSamePiece()
        {
            var piecesDict = new Dictionary<Position, PieceType>
            {
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(5, 5), PieceType.WhitePawn },
                { new Position(5,6), PieceType.WhitePawn },
                { new Position(6,6), PieceType.WhiteQueen },
                { new Position(0, 0), PieceType.WhiteKing },
            };
            var boardState = new BoardState(piecesDict);
            
            Assert.DoesNotThrow(() => _gameInitializer.InitialiseGame(boardState));
        }
    }
}