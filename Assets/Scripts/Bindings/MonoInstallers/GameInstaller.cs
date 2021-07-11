using Bindings.Installers.GameInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameStateControllerInstaller.Install(Container);
            GameStateUpdaterInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
            GameOverEvalInstaller.Install(Container);
        }
    }
}