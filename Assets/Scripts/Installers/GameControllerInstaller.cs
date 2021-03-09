using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameControllerInstaller : MonoInstaller<GameControllerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardRenderer>().To<BoardRenderer>().AsTransient();
        Container.Bind<IBoardGenerator>().To<BoardInitialiser>().AsTransient();
        Container.Bind<IPieceGenerator>().To<PieceGenerator>().AsSingle();
        
    }
}
