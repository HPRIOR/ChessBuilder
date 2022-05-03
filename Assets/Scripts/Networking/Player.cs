using Mirror;
using Models.State.Board;
using UnityEngine;
using Zenject;

namespace Networking
{
    public class Player : NetworkBehaviour
    {
        private MoveServer _moveServer;
        public void Start()
        {
            _moveServer = GameObject.FindWithTag("MoveServer").GetComponent<MoveServer>();
        }

        [Command]
        public void TryAddCommand(Position start, Position destination)
        {
            _moveServer.TryAddCommand(start, destination);
        }


        public class Factory : PlaceholderFactory<Player>
        {
        }
    }
}