using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameStateControllerInstaller : Installer<GameStateControllerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ITurnEventInvoker), typeof(IGameStateController)).To<GameStateController>()
                .AsSingle();
        }
    }
}