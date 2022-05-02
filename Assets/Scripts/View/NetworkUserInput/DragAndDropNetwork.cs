using Controllers.Factories;
using Controllers.Interfaces;
using Mirror;
using Models.State.Board;
using Networking;
using UnityEditor;
using UnityEngine;
using View.Prefab.Spawners;
using View.Utils;
using Zenject;

namespace View.NetworkUserInput
{
    // TODO: use UnityEngine.EventSystems.Interfaces.IDragHandler etc
    //  https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IMoveHandler.html
    public class DragAndDropNetwork : NetworkBehaviour
    {
        private bool _isDragging;
        private PieceSpawner _piece;
        private SpriteRenderer _spriteRenderer;
        private Player _player;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _piece = gameObject.GetComponent<PieceSpawner>();
            _player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
        }

        [Client]
        private void Update()
        {
            if (_isDragging)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.Translate(mousePosition);
            }
        }

        [Client]
        private void OnMouseDown()
        {
            _spriteRenderer.sortingOrder = 2;
            _isDragging = true;
        }

        [Client]
        private void OnMouseUp()
        {
            _spriteRenderer.sortingOrder = 1;
            var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var nearestBoardPosition = NearestBoardPosFinder.GetNearestBoardPosition(currentMousePosition);
            Debug.Log("adding command");
            _player.AddCommand(_piece.Position, nearestBoardPosition);
            _isDragging = false;
        }
    }
}