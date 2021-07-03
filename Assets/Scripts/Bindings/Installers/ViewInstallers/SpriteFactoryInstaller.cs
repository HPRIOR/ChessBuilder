using UnityEditor;
using UnityEngine;
using View.Prefab.Factories;
using View.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class SpriteFactoryInstaller : Installer<SpriteFactoryInstaller>
    {
        private readonly GameObject _piecePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Utils/SpritePrefab.prefab");

        public override void InstallBindings()
        {
            Container.Bind<SpriteFactory>().AsSingle();
            Container.BindFactory<Vector3, float, GameObject, string, int, SpriteSpawner, SpriteSpawner.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}