using Game.Implementations;
using Game.Interfaces;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameStateInstaller : Installer<GameStateInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ITurnEventInvoker), typeof(IGameState)).To<GameStateController>().AsSingle();
        }
    }
}