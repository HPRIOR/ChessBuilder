using Bindings.Installers.ControllerInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class ControllerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BuildCommandInstaller.Install(Container);
            CommandInvokerInstaller.Install(Container);
            MoveCommandInstaller.Install(Container);
        }
    }
}