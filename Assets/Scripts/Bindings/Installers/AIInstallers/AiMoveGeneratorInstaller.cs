using Models.Services.AI.Implementations;
using Models.Services.AI.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiMoveGeneratorInstaller : Installer<AiMoveGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAiMoveGenerator>().To<AiMoveGenerator>().AsSingle();
        }
    }
}