using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class PieceMoverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PieceMover>();
    }
}
