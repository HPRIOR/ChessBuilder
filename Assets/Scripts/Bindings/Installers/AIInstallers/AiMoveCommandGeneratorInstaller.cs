using Models.Services.AI;
using Models.Services.AI.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiMoveCommandGeneratorInstaller: Installer<AiMoveCommandGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAiCommandGenerator>().To<AiMoveCommandGenerator>().AsSingle();
        }
    }
}