using Bindings.Installers.AIInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class AIInstaller : MonoInstaller
    {
        public bool aiToggle;
        private string aiToggleStr = "AiToggle";

        public override void InstallBindings()
        {
            if (aiToggle)
            {
                Container.BindInstance(true).WithId(aiToggleStr);
            }
            else Container.BindInstance(false).WithId(aiToggleStr);

            AiPossibleMoveGeneratorInstaller.Install(Container);
            AiMoveExecutorInstaller.Install(Container);
            AiMoveGeneratorInstaller.Install(Container);
            StaticEvaluatorInstaller.Install(Container);
            MoveOrdererInstaller.Install(Container);
        }
    }
}