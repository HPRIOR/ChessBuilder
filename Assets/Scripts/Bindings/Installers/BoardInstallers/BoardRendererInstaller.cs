using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardRendererInstaller : Installer<BoardRendererInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardRenderer>().To<BoardRenderer>().AsTransient();
    }
}
