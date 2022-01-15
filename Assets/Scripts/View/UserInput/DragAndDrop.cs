using Controllers.Factories;
using Controllers.Interfaces;
using UnityEngine;
using View.Prefab.Spawners;
using View.Utils;
using Zenject;

namespace View.UserInput
{
    // TODO: use UnityEngine.EventSystems.Interfaces.IDragHandler etc
    //  https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IMoveHandler.html
    public class DragAndDrop : MonoBehaviour
    {
        private ICommandInvoker _commandInvoker;
        private bool _isDragging;
        private MoveCommandFactory _moveCommandFactory;
        private AiMoveCommandFactory _aiMoveCommandFactory;
        private PieceSpawner _piece;
        private SpriteRenderer _spriteRenderer;
        [Inject(Id = "AiToggle")] private bool _aiEnabled;


        [Inject]
        public void Construct(ICommandInvoker commandInvoker, MoveCommandFactory moveCommandFactory,
            AiMoveCommandFactory aiMoveCommandFactory)
        {
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
            _aiMoveCommandFactory = aiMoveCommandFactory;
        }

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

            var moveCommand = _moveCommandFactory.Create(_piece.Position, nearestBoardPosition);
            var moveCommandIsValid = moveCommand.IsValid();
            _commandInvoker.AddCommand(
                moveCommand
            );

            if (moveCommandIsValid & _aiEnabled)
            {
                _commandInvoker.AddCommand(_aiMoveCommandFactory.Create());
            }

            _isDragging = false;
        }
    }
}