using UnityEngine;
using Zenject;

public class MoveDataInstaller : Installer<MoveDataInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<MoveDataFactory>().AsSingle();
        Container.BindFactory<GameObject, IBoardPosition, MoveData, MoveData.Factory>().FromNew();
    }
}