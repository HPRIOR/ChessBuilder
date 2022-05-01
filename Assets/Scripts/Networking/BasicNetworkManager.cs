using Mirror;
using UnityEngine;
using Zenject;

namespace Networking
{
    public class BasicNetworkManager : NetworkManager
    {
        private PlayerFactory _playerFactory;
        [Inject]
        public void Construct(PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }
        
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // Zenject factory pattern required in order to inject CommandInvoker to instantiated player prefab
            var player = _playerFactory.Create();
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }
    }
}