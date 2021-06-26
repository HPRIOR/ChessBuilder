using Controllers.Builders;
using Controllers.Interfaces;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class BuildValidatorInstaller : Installer<BuildValidatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuildValidator>().To<BuildValidator>().AsSingle();
        }
    }
}