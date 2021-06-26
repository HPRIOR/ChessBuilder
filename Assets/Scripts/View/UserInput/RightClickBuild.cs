using Controllers.Factories;
using Controllers.Interfaces;
using Models.State.PieceState;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Utils;
using Zenject;

namespace View.UserInput
{
    public class RightClickBuild : MonoBehaviour, IPointerClickHandler
    {
        private static ICommandInvoker _commandInvoker;
        private static BuildCommandFactory _buildCommandFactory;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var nearestPos =
                    NearestBoardPosFinder.GetNearestBoardPosition(eventData.pointerCurrentRaycast.worldPosition);
                _commandInvoker.AddCommand(
                    _buildCommandFactory.Create(nearestPos, PieceType.BlackPawn)
                );
            }
        }

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, BuildCommandFactory buildCommandFactory)
        {
            _commandInvoker = commandInvoker;
            _buildCommandFactory = buildCommandFactory;
        }
    }
}