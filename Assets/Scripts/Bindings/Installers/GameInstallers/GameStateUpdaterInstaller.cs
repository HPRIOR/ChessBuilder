using Models.Services.Game.Implementations;
using Models.State.GameState;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameStateUpdaterInstaller : Installer<GameStateUpdaterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateUpdaterFactory>().AsSingle();
            Container.BindFactory<GameState, GameStateUpdater, GameStateUpdater.Factory>();
        }
    }
}