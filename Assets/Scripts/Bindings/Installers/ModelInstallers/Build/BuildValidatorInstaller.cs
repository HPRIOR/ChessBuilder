using Controllers.Commands;
using Controllers.Interfaces;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Build
{
    public class BuildValidatorInstaller : Installer<BuildValidatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuildValidator>().To<BuildValidator>().AsSingle();
        }
    }
}