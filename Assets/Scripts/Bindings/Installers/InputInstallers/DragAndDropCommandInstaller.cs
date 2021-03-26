using UnityEngine;
using Zenject;

public class DragAndDropCommandInstaller : Installer<DragAndDropCommandInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<DragAndDropCommandFactory>().AsSingle();
        Container.BindFactory<GameObject, IBoardPosition, DragAndDropCommand, DragAndDropCommand.Factory>().FromNew();
    }
}