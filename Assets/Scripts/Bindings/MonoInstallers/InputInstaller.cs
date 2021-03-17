using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        DragAndDropCommandInstaller.Install(Container);
        CommandInvokerInstaller.Install(Container);
        MoveDataInstaller.Install(Container);
    }
}
