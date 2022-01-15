using Controllers.Factories;
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
        [Inject(Id = "AiToggle")] private bool _aiEnabled;
        private AiMoveCommandFactory _aiMoveCommandFactory;
        private IGameStateController _gameStateController;
        private PieceBuildSelectorFactory _pieceBuildSelectorFactory;
        private PieceType _pieceToBuild;
        private Position _nearestPos;
        private bool _buildSelectionInstigated;
        private static BuildCommandFactory _buildCommandFactory;
        private static ICommandInvoker _commandInvoker;
        
        [Inject]
        public void Construct(ICommandInvoker commandInvoker, IGameStateController gameStateController,
            BuildCommandFactory buildCommandFactory, AiMoveCommandFactory aiMoveCommandFactory, PieceBuildSelectorFactory pieceBuildSelectorFactory)
        {
            _aiMoveCommandFactory = aiMoveCommandFactory;
            _buildCommandFactory = buildCommandFactory;
            _commandInvoker = commandInvoker;
            _gameStateController = gameStateController;
            _pieceBuildSelectorFactory = pieceBuildSelectorFactory;
        }

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
                var buildCommand = _buildCommandFactory.Create(_nearestPos, _pieceToBuild);
                var commandIsValid = buildCommand.IsValid(peak: true);
                _commandInvoker.AddCommand(
                    buildCommand
                );
                GameObjectDestroyer.DestroyChildrenOfObjectWithTag("UI");
                if (commandIsValid & _aiEnabled)
                {
                    _commandInvoker.AddCommand(
                        _aiMoveCommandFactory.Create()
                        );
                }
            }
        }


        private void SetPieceCallBack(PieceType pieceType) => _pieceToBuild = pieceType;


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