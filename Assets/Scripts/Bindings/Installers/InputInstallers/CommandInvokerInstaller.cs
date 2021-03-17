using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class CommandInvokerInstaller : Installer<CommandInvokerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();
    }
}
