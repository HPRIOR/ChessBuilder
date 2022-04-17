using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameStateUpdaterInstaller : Installer<GameStateUpdaterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateUpdater>().To<GameStateUpdater>().AsSingle();
        }
    }
}