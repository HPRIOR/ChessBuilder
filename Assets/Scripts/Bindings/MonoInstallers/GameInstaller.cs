using Bindings.Installers.GameInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameStateInstaller.Install(Container);
        }
    }
}