using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class MoveValidatorInstaller : Installer<MoveValidatorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IMoveValidator>().To<MoveValidator>().AsSingle();
    }
}
