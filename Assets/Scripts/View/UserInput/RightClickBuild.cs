﻿using Controllers.Factories;
using Controllers.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using UnityEngine.EventSystems;
using View.UI.PieceBuildSelector;
using View.Utils;
using Zenject;

namespace View.UserInput
{
    public class RightClickBuild : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private static ICommandInvoker _commandInvoker;
        private static BuildCommandFactory _buildCommandFactory;

        private static readonly PieceType[] BlackSelection =
        {
            PieceType.BlackQueen, PieceType.BlackRook, PieceType.BlackKnight, PieceType.BlackBishop,
            PieceType.BlackPawn
        };

        private static readonly PieceType[] WhiteSelection =
        {
            PieceType.WhiteQueen, PieceType.WhiteRook, PieceType.WhiteKnight, PieceType.WhiteBishop,
            PieceType.WhitePawn
        };

        private bool _buildSelectionInstigated;
        private IGameStateController _gameStateController;
        private Position _nearestPos;
        private PieceBuildSelectorFactory _pieceBuildSelectorFactory;
        private PieceType _pieceToBuild;


        public void OnPointerDown(PointerEventData eventData)
        {
            _nearestPos =
                NearestBoardPosFinder.GetNearestBoardPosition(eventData.pointerCurrentRaycast.worldPosition);
            if (_gameStateController.CurrentGameState.PossibleBuildMoves.BuildPositions.Contains(_nearestPos))
            {
                RenderSelections(_gameStateController.Turn == PieceColour.Black ? BlackSelection : WhiteSelection,
                    _nearestPos.GetVector());
                _buildSelectionInstigated = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_buildSelectionInstigated)
            {
                _commandInvoker.AddCommand(
                    _buildCommandFactory.Create(_nearestPos, _pieceToBuild)
                );
                GameObjectDestroyer.DestroyChildrenOfObjectWith("UI");
            }
        }


        private void SetPieceCallBack(PieceType pieceType) => _pieceToBuild = pieceType;

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, IGameStateController gameStateController,
            BuildCommandFactory buildCommandFactory, PieceBuildSelectorFactory pieceBuildSelectorFactory)
        {
            _gameStateController = gameStateController;
            _commandInvoker = commandInvoker;
            _buildCommandFactory = buildCommandFactory;
            _pieceBuildSelectorFactory = pieceBuildSelectorFactory;
        }

        private void RenderSelections(PieceType[] pieces, Vector3 center)
        {
            var vectors = CircleVectors.VectorPoints(5, 0.75f, center, Vector3.forward);
            for (var i = 0; i < vectors.Length; i++)
            {
                var canBuild = _gameStateController.CurrentGameState.PossibleBuildMoves.BuildPieces.Contains(pieces[i]);
                var pieceBuildSelector =
                    _pieceBuildSelectorFactory.Create(vectors[i], pieces[i], SetPieceCallBack, canBuild);
                pieceBuildSelector.transform.parent = GameObject.FindGameObjectWithTag("UI").transform;
            }
        }
    }
}