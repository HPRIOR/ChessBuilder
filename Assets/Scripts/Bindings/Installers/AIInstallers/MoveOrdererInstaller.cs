using Models.Services.AI.Implementations;
using Models.Services.Game.Implementations;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class MoveOrdererInstaller : Installer<MoveOrdererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MoveOrdererInstaller>().AsSingle();
            Container.BindFactory<GameStateUpdater, MoveOrderer, MoveOrderer.Factory>();
        }
    }
}