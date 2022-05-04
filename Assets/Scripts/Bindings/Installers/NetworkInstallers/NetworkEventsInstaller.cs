using Networking;
using Zenject;

namespace Bindings.Installers.NetworkInstallers
{
    public class NetworkEventsInstaller: Installer<NetworkEventsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkEvents>().AsSingle();
        }
    }
}