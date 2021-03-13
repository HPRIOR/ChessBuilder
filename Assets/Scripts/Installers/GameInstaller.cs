using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardGenerator>().To<BoardInitialiser>().AsTransient();
        Container.Bind<PieceInjector>().AsSingle();
        Container.Bind<IBoardState>().To<BoardState>().AsSingle();
    }
}
