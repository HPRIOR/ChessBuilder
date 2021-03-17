using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardStateInstaller: Installer<BoardStateInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardState>().To<BoardState>().AsSingle();
    }
}
