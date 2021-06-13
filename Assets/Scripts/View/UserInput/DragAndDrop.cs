using Controllers.Factories;
using Controllers.Interfaces;
using Models.State.Board;
using UnityEngine;
using View.Renderers;
using Zenject;

namespace View.UserInput
{
    public class DragAndDrop : MonoBehaviour
    {
        private static MoveCommandFactory _dragAndDropCommandFactory;
        private ICommandInvoker _commandInvoker;
        private bool _isDragging;
        private PieceMono _piece;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _piece = gameObject.GetComponent<PieceMono>();
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
            var nearestBoardPosition = GetNearestBoardPosition(currentMousePosition);

            _commandInvoker.AddCommand(
                _dragAndDropCommandFactory.Create(
                    _piece.Position,
                    nearestBoardPosition)
            );
            _isDragging = false;
        }

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, MoveCommandFactory dragAndDropCommandFactory)
        {
            _commandInvoker = commandInvoker;
            _dragAndDropCommandFactory = dragAndDropCommandFactory;
        }

        private Position GetNearestBoardPosition(Vector2 position) =>
            new Position(ConvertAxisToNearestBoardIndex(position.x),
                ConvertAxisToNearestBoardIndex(position.y));

        private int ConvertAxisToNearestBoardIndex(float axis)
        {
            if (axis > 7.5) return 7;
            if (axis < 0.5) return 0;
            return (int) axis;
        }
    }
}