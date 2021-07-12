using Models.State.Board;
using Models.State.BuildState;
using UnityEngine;
using View.Prefab.Factories;
using View.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class BuildPieceFactoryInstaller : Installer<BuildPieceFactoryInstaller>
    {
        private readonly GameObject _piecePrefab =
            Resources.Load<GameObject>("Prefabs/Pieces/BuildingPiecePrefab");


        public override void InstallBindings()
        {
            Container.Bind<BuildingPieceFactory>().AsSingle();
            Container
                .BindFactory<Position, BuildTileState, BuildingPieceSpawner,
                    BuildingPieceSpawner.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}