using Bindings.Installers.ControllerInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            MoveCommandInstaller.Install(Container);
            CommandInvokerInstaller.Install(Container);
        }
    }
}