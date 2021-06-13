using Models.Services.Interfaces;
using Models.Services.Piece;
using Models.State.Board;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using View.Renderers;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PieceSpawnerInstaller : Installer<PieceSpawnerInstaller>
    {
        private readonly GameObject _piecePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pieces/PiecePrefab.prefab");

        public override void InstallBindings()
        {
            Container.Bind<IPieceSpawner>().To<PieceSpawner>().AsSingle();
            Container.BindFactory<IPieceInfo, Position, PieceMono, PieceMono.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}