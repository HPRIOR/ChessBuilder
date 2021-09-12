using Models.Services.Board;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Build
{
    public class BuildStateDecrementorInstaller : Installer<BuildStateDecrementorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BuildStateDecrementor>().AsSingle();
        }
    }
}