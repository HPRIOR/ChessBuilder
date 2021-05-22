using Bindings.Installers.InputInstallers;
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