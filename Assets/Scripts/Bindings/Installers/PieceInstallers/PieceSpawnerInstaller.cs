using Models.Services.Interfaces;
using Models.Services.Piece;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using View.Interfaces;
using View.Renderers;
using Zenject;

public class PieceSpawnerInstaller : Installer<PieceSpawnerInstaller>
{
    private readonly GameObject piecePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pieces/PiecePrefab.prefab");

    public override void InstallBindings()
    {
        Container.Bind<IPieceSpawner>().To<PieceSpawner>().AsSingle();
        Container.BindFactory<IPieceInfo, IBoardPosition, PieceMono, PieceMono.Factory>().FromComponentInNewPrefab(piecePrefab);
    }
}