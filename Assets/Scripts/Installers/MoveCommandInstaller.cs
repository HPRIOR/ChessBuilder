using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveCommandInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<MovePieceCommandFactory>().AsSingle();
        Container.Bind<MovePieceCommandFactory>().AsSingle();
        Container.Bind<IPieceMover>().To<PieceMover>().AsSingle();
        Container.BindFactory<GameObject, IBoardPosition, MovePieceCommand, MovePieceCommand.Factory>().FromNew();
    }
}
