using Models.Services.AI.Implementations;
using Models.Services.AI.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class MoveOrdererInstaller : Installer<MoveOrdererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMoveOrderer>().To<MoveOrderer>().AsSingle();
        }
    }
}