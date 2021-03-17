﻿using UnityEditor;
using UnityEngine;
using Zenject;

public class PieceSpawnerInstaller : Installer<PieceSpawnerInstaller>
{
    private GameObject piecePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pieces/PiecePrefab.prefab");

    public override void InstallBindings()
    {
        Container.Bind<IPieceSpawner>().To<PieceSpawner>().AsSingle();
        Container.BindFactory<PieceType, IBoardPosition, Piece, Piece.Factory>().FromComponentInNewPrefab(piecePrefab);
    }

}