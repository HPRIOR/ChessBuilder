using Models.State.Board;
using UnityEditor;
using UnityEngine;
using View.Prefab.Factories;
using View.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class TileFactoryInstaller : Installer<TileFactoryInstaller>
    {
        private readonly GameObject _tilePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Board/BoardTile.prefab");

        public override void InstallBindings()
        {
            Container.Bind<TileFactory>().AsSingle();
            Container.BindFactory<Position, GameObject, Color32, TileSpawner, TileSpawner.Factory>()
                .FromComponentInNewPrefab(_tilePrefab);
        }
    }
}