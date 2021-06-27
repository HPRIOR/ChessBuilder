using Models.State.Board;
using Models.State.BuildState;
using Models.State.Interfaces;
using UnityEngine;
using Zenject;

namespace View.Utils.Prefab.Spawners
{
    public class BuildingPieceSpawner : MonoBehaviour
    {
        private BuildState _buildState;
        private Position _position;
        private IPieceRenderInfo _renderInfo;

        public void Start()
        {
        }

        [Inject]
        public void Construct(IPieceRenderInfo pieceRenderInfo, Position position, BuildState buildState)
        {
            _renderInfo = pieceRenderInfo;
            _position = position;
            _buildState = buildState;
        }

        public class Factory : PlaceholderFactory<IPieceRenderInfo, Position, BuildState, BuildingPieceSpawner>
        {
        }
    }
}