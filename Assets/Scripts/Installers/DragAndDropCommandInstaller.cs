using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DragAndDropCommandInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<DragAndDropCommandFactory>().AsSingle();
        Container.Bind<DragAndDropCommandFactory>().AsSingle();
        Container.BindFactory<GameObject, IBoardPosition, DragAndDropCommand, DragAndDropCommand.Factory>().FromNew();
    }
}
