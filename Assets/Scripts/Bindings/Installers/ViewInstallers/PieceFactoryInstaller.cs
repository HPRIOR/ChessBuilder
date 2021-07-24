using Models.Services.Moves.Interfaces;
using Models.State.Board;
using UnityEngine;
using View.Interfaces;
using View.Prefab.Factories;
using View.Prefab.Spawners;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class PieceFactoryInstaller : Installer<PieceFactoryInstaller>
    {
        private readonly GameObject _piecePrefab =
            Resources.Load<GameObject>("Prefabs/Pieces/PiecePrefab");

        public override void InstallBindings()
        {
            Container.Bind<IPieceFactory>().To<PieceFactory>().AsSingle();
            Container.BindFactory<IPieceRenderInfo, Position, PieceSpawner, PieceSpawner.Factory>()
                .FromComponentInNewPrefab(_piecePrefab);
        }
    }
}