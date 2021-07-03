using Controllers.Factories;
using Controllers.Interfaces;
using Game.Interfaces;
using Models.State.PieceState;
using UnityEngine;
using UnityEngine.EventSystems;
using View.UI.PieceBuildSelector;
using View.Utils;
using Zenject;

namespace View.UserInput
{
    /// <summary>
    ///     Generates prefab UI instance
    /// </summary>
    public class RightClickBuild : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private static ICommandInvoker _commandInvoker;
        private static BuildCommandFactory _buildCommandFactory;

        private static readonly PieceType[] _blackSelection =
        {
            PieceType.BlackQueen, PieceType.BlackRook, PieceType.BlackKnight, PieceType.BlackBishop,
            PieceType.BlackPawn
        };

        private static readonly PieceType[] _whiteSelection =
        {
            PieceType.WhiteQueen, PieceType.WhiteRook, PieceType.WhiteKnight, PieceType.WhiteBishop,
            PieceType.WhitePawn
        };

        private IGameState _gameState;
        private PieceBuildSelectorFactory _pieceBuildSelectorFactory;

        private PieceType _pieceToBuild;

        public void OnPointerDown(PointerEventData eventData)
        {
            var nearestPos =
                NearestBoardPosFinder.GetNearestBoardPosition(eventData.pointerCurrentRaycast.worldPosition);
            RenderSelections(_gameState.Turn == PieceColour.Black ? _blackSelection : _whiteSelection,
                nearestPos.Vector);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // var nearestPos =
            //     NearestBoardPosFinder.GetNearestBoardPosition(eventData.pointerCurrentRaycast.worldPosition);
            // _commandInvoker.AddCommand(
            //     _buildCommandFactory.Create(nearestPos, PieceType.WhiteQueen)
            // );
            GameObjectDestroyer.DestroyChildrenOfObjectWith("UI");
        }


        private void SetPieceCallBack(PieceType pieceType) => _pieceToBuild = pieceType;

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, IGameState gameState,
            BuildCommandFactory buildCommandFactory, PieceBuildSelectorFactory pieceBuildSelectorFactory)
        {
            _gameState = gameState;
            _commandInvoker = commandInvoker;
            _buildCommandFactory = buildCommandFactory;
            _pieceBuildSelectorFactory = pieceBuildSelectorFactory;
        }

        private void RenderSelections(PieceType[] pieces, Vector3 center)
        {
            var vectors = CircleVectors.VectorPoints(5, 0.75f, center, Vector3.forward);
            for (var i = 0; i < vectors.Length; i++)
            {
                var pieceBuildSelector =
                    _pieceBuildSelectorFactory.Create(vectors[i], pieces[i], SetPieceCallBack, true);
                pieceBuildSelector.transform.parent = GameObject.FindGameObjectWithTag("UI").transform;
            }
        }
    }
}