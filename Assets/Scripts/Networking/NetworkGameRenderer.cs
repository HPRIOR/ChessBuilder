using Models.Services.Game.Interfaces;
using UnityEngine;
using View.Interfaces;
using Zenject;

// 'View class'  which is subscribed to changes in game state
namespace Networking
{
    public class NetworkGameRenderer : MonoBehaviour
    {
        private IRenderer _boardRenderer;
        private IStateChangeRenderer _buildRenderer;
        private IStateChangeRenderer _pieceRenderer;
        private ITurnEventInvoker _turnEventInvoker;
        private NetworkEvents _networkEvents;

        private void Awake()
        {
            _turnEventInvoker.BoardStateChangeEvent += _pieceRenderer.Render;
            _turnEventInvoker.BoardStateChangeEvent += _buildRenderer.Render;

            _networkEvents.RegisterEventCallBack(NetworkEvent.ContextReady, () =>
                _boardRenderer.Render());
        }


        [Inject]
        public void Construct(
            [Inject(Id = "Piece")] IStateChangeRenderer pieceRenderer,
            [Inject(Id = "Build")] IStateChangeRenderer buildRenderer,
            IRenderer boardRenderer,
            ITurnEventInvoker turnEventInvoker,
            NetworkEvents networkEvents)
        {
            _buildRenderer = buildRenderer;
            _pieceRenderer = pieceRenderer;
            _turnEventInvoker = turnEventInvoker;
            _boardRenderer = boardRenderer;
            _networkEvents = networkEvents;
        }
    }
}