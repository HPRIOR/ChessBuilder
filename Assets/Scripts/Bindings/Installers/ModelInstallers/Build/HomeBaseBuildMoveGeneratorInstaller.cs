using Models.Services.Build.BuildMoves;
using Models.Services.Build.Interfaces;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Build
{
    public class HomeBaseBuildMoveGeneratorInstaller : Installer<HomeBaseBuildMoveGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuildMoveGenerator>().To<HomeBaseBuildMoveGenerator>().AsSingle();
        }
    }
}