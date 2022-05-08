using Mirror;
using Models.State.PieceState;

namespace Networking
{
    public class BasicNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var playerGameObject = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, playerGameObject);
            var playerColour = numPlayers == 1 ? PieceColour.White : PieceColour.Black;

            var player = playerGameObject.GetComponent<Player>();
            player.SetPlayerContext(playerColour);
            player.SetGameReady();
        }
    }
}