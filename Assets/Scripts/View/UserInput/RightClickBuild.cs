﻿using Controllers.Factories;
using Controllers.Interfaces;
using Game.Interfaces;
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
        private IGameState _gameState;

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
        public void Construct(ICommandInvoker commandInvoker, IGameState gameState,
            BuildCommandFactory buildCommandFactory)
        {
            _gameState = gameState;
            _commandInvoker = commandInvoker;
            _buildCommandFactory = buildCommandFactory;
        }
    }
}