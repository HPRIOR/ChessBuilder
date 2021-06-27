﻿using Models.State.Board;
using Models.State.BuildState;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using View.Utils.Prefab.Factories;
using View.Utils.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class BuildPieceFactoryInstaller : Installer<BuildPieceFactoryInstaller>
    {
        private readonly GameObject _piecePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pieces/BuildingPiecePrefab.prefab");


        public override void InstallBindings()
        {
            Container.Bind<BuildingPieceFactory>().AsSingle();
            Container
                .BindFactory<IPieceRenderInfo, Position, BuildState, BuildingPieceSpawner,
                    BuildingPieceSpawner.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}