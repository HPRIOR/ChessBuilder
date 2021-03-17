using UnityEngine;
using Zenject;

public class PieceSpawnerInstaller : MonoInstaller
{
    public GameObject piecePrefab;

    public override void InstallBindings()
    {
        Container.Bind<IPieceSpawner>().To<PieceSpawner>().AsSingle();
        Container.BindFactory<PieceType, IBoardPosition, Piece, Piece.Factory>().FromComponentInNewPrefab(piecePrefab);
    }

}
