﻿using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using View.Utils.Prefab.Factories;
using View.Utils.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PieceFactoryInstaller : Installer<PieceFactoryInstaller>
    {
        private readonly GameObject _piecePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pieces/PiecePrefab.prefab");

        public override void InstallBindings()
        {
            Container.Bind<IPieceFactory>().To<PieceFactory>().AsSingle();
            Container.BindFactory<IPieceRenderInfo, Position, PieceSpawner, PieceSpawner.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}