using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bindings.Utils;
using Models.Services.Board;
using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
    [TestFixture]
    public class GameStateUpdaterTests : ZenjectUnitTestFixture
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

        private IGameStateUpdater _gameStateUpdater;
        private IBoardGenerator _boardGenerator;
        private GameInitializer _gameStateInitializer;

        private void InstallBindings()
        {
            BindAllDefault.InstallAllTo(Container);
        }

        private void ResolveContainer()
        {
            _gameStateUpdater = Container.Resolve<IGameStateUpdater>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _gameStateInitializer = Container.Resolve<GameInitializer>();
        }
    }

}