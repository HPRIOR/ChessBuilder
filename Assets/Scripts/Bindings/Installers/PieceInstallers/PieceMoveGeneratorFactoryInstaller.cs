using System;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PieceMoveGeneratorFactoryInstaller : Installer<PieceMoveGeneratorFactoryInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPieceMoveGeneratorFactory>().To<PieceMoveGeneratorFactory>().AsSingle();
    }
}
