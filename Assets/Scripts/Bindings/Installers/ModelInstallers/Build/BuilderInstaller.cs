using Controllers.Builders;
using Models.Services.Build.Interfaces;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Build
{
    public class BuilderInstaller : Installer<BuilderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuilder>().To<Builder>().AsSingle();
        }
    }
}