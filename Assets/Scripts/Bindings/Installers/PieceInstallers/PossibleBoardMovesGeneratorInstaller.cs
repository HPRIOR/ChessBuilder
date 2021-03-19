using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class PossibleBoardMovesGeneratorInstaller : Installer<PossibleBoardMovesGeneratorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPossibleBoardMovesGenerator>().To<PossibleBoardMovesGenerator>().AsSingle();
    }
}
