using Models.Services.AI.Implementations;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiMoveGeneratorInstaller : Installer<AiMoveGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AiMoveGenerator>().AsSingle();
        }
    }
}