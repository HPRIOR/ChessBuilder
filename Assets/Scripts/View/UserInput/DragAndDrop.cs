using Controllers.Factories;
using Controllers.Interfaces;
using UnityEngine;
using View.Utils;
using Zenject;

namespace View.UserInput
{
    // TODO: use UnityEngine.EventSystems.Interfaces.IDragHandler etc
    //  https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IMoveHandler.html
    public class DragAndDrop : MonoBehaviour
    {
        private static MoveCommandFactory _moveCommandFactory;
        private static ICommandInvoker _commandInvoker;
        private bool _isDragging;
        private PieceSpawner _piece;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _piece = gameObject.GetComponent<PieceSpawner>();
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

            _commandInvoker.AddCommand(
                _moveCommandFactory.Create(
                    _piece.Position,
                    nearestBoardPosition)
            );
            _isDragging = false;
        }

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, MoveCommandFactory moveCommandFactory)
        {
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
        }
    }
}