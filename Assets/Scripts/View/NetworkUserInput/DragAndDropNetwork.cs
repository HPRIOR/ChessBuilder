using System;
using Mirror;
using Networking;
using UnityEngine;
using View.Prefab.Spawners;
using View.Utils;
using Zenject;

namespace View.NetworkUserInput
{
    // TODO: use UnityEngine.EventSystems.Interfaces.IDragHandler etc
    //  https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IMoveHandler.html
    public class DragAndDropNetwork : MonoBehaviour
    {
        private bool _isDragging;
        private NetworkEvents _networkEvents;
        private PieceSpawner _piece;
        private Player _player;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _piece = gameObject.GetComponent<PieceSpawner>();
            SetupPlayerReference();
        }


        private void Update()
        {
            if (_isDragging)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.Translate(mousePosition);
            }
        }

        private void OnMouseDown()
        {
            _spriteRenderer.sortingOrder = 2;
            _isDragging = true;
        }

        private void OnMouseUp()
        {
            _spriteRenderer.sortingOrder = 1;
            var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var nearestBoardPosition = NearestBoardPosFinder.GetNearestBoardPosition(currentMousePosition);
            _player.TryAddCommand(_piece.Position, nearestBoardPosition);
            _isDragging = false;
        }


        [Inject]
        public void Construct(NetworkEvents networkEvents)
        {
            _networkEvents = networkEvents;
        }

        /// <summary>
        ///     This behaviour is attached to piece objects which are spawned at the very start of the game, and then
        ///     at every subsequent rerender. The network event callback ensures player reference availability on game
        ///     initialisation (sometimes the Player class is not initialised when pieces are first spawned). The
        ///     try/catch allows spawned piece (during rerender) to attempt to establish a reference in a way that won't
        ///     throw an error when the player is not available
        /// </summary>
        private void SetupPlayerReference()
        {
            try
            {
                _player ??= NetworkClient.localPlayer.gameObject.GetComponent<Player>();
            }
            catch (NullReferenceException)
            {
            }

            _networkEvents.RegisterEventCallBack(NetworkEvent.PlayerPrefabReady, () =>
                _player ??= NetworkClient.localPlayer.gameObject.GetComponent<Player>());
        }
    }
}