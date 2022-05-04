using Bindings.Installers.NetworkInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class NetworkInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            NetworkEventsInstaller.Install(Container);
        }
    }
}