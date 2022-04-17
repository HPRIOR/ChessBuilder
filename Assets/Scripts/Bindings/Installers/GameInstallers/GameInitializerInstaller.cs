using Models.Services.Game.Implementations;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameInitializerInstaller : Installer<GameInitializerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameInitializer>().AsSingle();
        }
    }
}