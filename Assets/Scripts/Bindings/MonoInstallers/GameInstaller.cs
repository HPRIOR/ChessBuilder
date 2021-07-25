using Bindings.Installers.AIInstallers;
using Bindings.Installers.GameInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class GameInstaller : MonoInstaller
    {
        public bool AI;

        public override void InstallBindings()
        {
            if (AI)
                AiGameStateControllerInstaller.Install(Container);
            else
                GameStateControllerInstaller.Install(Container);

            GameStateUpdaterInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            GameOverEvalInstaller.Install(Container);
            GameInitializerInstaller.Install(Container);
        }
    }
}