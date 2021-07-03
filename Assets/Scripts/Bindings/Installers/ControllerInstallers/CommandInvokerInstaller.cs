using Controllers.Commands;
using Controllers.Interfaces;
using Zenject;

namespace Bindings.Installers.ControllerInstallers
{
    public class CommandInvokerInstaller : Installer<CommandInvokerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();
        }
    }
}