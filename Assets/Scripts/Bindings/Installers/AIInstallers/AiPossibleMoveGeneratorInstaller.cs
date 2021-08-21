using Models.Services.AI.Implementations;
using Models.Services.AI.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiPossibleMoveGeneratorInstaller : Installer<AiPossibleMoveGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAiPossibleMoveGenerator>().To<AiPossibleMoveGenerator>().AsSingle();
        }
    }
}