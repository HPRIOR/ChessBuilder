using Controllers.Commands;
using Controllers.Interfaces;
using Zenject;

public class CommandInvokerInstaller : Installer<CommandInvokerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();
    }
}