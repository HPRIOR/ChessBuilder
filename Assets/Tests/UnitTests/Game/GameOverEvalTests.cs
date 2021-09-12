using System.Collections.Generic;
using Bindings.Installers.GameInstallers;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Game
{
    [TestFixture]
    public class GameOverEvalTests : ZenjectUnitTestFixture
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

        private IGameOverEval _gameOverEval;

        private void InstallBindings()
        {
            GameOverEvalInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _gameOverEval = Container.Resolve<IGameOverEval>();
        }

        [Test]
        public void CheckAndNoMovesProduces_GameOver()
        {
            var moves = new Dictionary<Position, HashSet<Position>>
            {
                { new Position(4, 4), new HashSet<Position>() }
            };
            const bool check = true;

            Assert.That(_gameOverEval.CheckMate(check, moves), Is.True);
        }
    }
}