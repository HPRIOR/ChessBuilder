using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveDataInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<MoveDataFactory>().AsSingle();
        Container.Bind<MoveDataFactory>().AsSingle();
        Container.BindFactory<GameObject, IBoardPosition, MoveData, MoveData.Factory>().FromNew();
    }
}
