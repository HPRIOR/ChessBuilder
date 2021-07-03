using System;
using Models.State.PieceState;
using UnityEditor;
using UnityEngine;
using View.UI.PieceBuildSelector;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PieceBuildSelectorFactoryInstaller : Installer<PieceBuildSelectorFactoryInstaller>
    {
        private readonly GameObject _prefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/BuildUIPiece.prefab");

        public override void InstallBindings()
        {
            Container.Bind<PieceBuildSelectorFactory>().AsSingle();
            Container
                .BindFactory<Vector3, PieceType, Action<PieceType>, bool, PieceBuildSelector,
                    PieceBuildSelector.Factory>().FromComponentInNewPrefab(_prefab);
        }
    }
}