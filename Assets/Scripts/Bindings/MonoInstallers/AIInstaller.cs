using Bindings.Installers.AIInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class AIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            AiPossibleMoveGeneratorInstaller.Install(Container);
            AiMoveGeneratorInstaller.Install(Container);
            StaticEvaluatorInstaller.Install(Container);
        }
    }
}