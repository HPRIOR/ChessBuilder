using System;
using Models.State.PieceState;
using UnityEngine;
using View.UI.PieceBuildSelector;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class PieceBuildSelectorFactoryInstaller : Installer<PieceBuildSelectorFactoryInstaller>
    {
        private readonly GameObject _prefab =
            Resources.Load<GameObject>("Prefabs/UI/BuildUIPiece");

        public override void InstallBindings()
        {
            Container.Bind<PieceBuildSelectorFactory>().AsSingle();
            Container
                .BindFactory<Vector3, PieceType, Action<PieceType>, bool, PieceBuildSelector,
                    PieceBuildSelector.Factory>().FromComponentInNewPrefab(_prefab);
        }
    }
}