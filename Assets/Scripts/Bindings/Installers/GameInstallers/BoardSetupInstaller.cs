using Game.Implementations;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class BoardSetupInstaller : Installer<BoardSetupInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BoardSetup>().AsSingle();
        }
    }
}