using Bindings.Installers.AIInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class AIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            AiMoveGeneratorInstaller.Install(Container);
            MiniMaxInstaller.Install(Container);
            StaticEvaluatorInstaller.Install(Container);
        }
    }
}