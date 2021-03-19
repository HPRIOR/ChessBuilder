using System;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PossibleMoveGeneratorFactoryInstaller : Installer<PossibleMoveGeneratorFactoryInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPossibleMoveGeneratorFactory>().To<PossibleMoveGeneratorFactory>().AsSingle();
    }
}
