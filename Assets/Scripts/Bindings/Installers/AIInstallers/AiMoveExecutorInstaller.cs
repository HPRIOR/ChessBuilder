using Models.Services.AI.Implementations;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiMoveExecutorInstaller: Installer<AiMoveExecutorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AiMoveExecutor>().AsSingle();
        }
    }
}