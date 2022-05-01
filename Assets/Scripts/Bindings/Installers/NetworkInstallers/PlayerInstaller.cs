using Networking;
using UnityEngine;
using Zenject;

namespace Bindings.Installers.NetworkInstallers
{
    public class PlayerInstaller: Installer<PlayerInstaller>
    {
        private GameObject _playerPrefab = Resources.Load<GameObject>("Prefabs/Network/Player");

        public override void InstallBindings()
        {
            Container.Bind<PlayerFactory>().AsSingle();
            Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(_playerPrefab);
        }
    }
}