using UnityEngine;
using Zenject;

public class PieceInstaller : MonoInstaller
{
    public GameObject piecePrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PieceSpawner>().AsSingle();
        Container.Bind<IPieceSpawner>().To<PieceSpawner>().AsSingle();
        Container.Bind<ICommandInvoker>().To<CommandInvoker>().AsSingle();
        Container.BindFactory<PieceType, IBoardPosition, Piece, Piece.Factory>().FromComponentInNewPrefab(piecePrefab);
    }

}
