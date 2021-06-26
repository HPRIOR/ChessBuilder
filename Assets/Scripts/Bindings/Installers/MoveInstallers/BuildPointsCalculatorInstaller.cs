using Models.Services.Build.Interfaces;
using Models.Services.Build.Utils;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class BuildPointsCalculatorInstaller : Installer<BuildPointsCalculatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuildPointsCalculator>().To<BuildPointsCalculator>().AsSingle();
        }
    }
}