using Models.State.Board;
using Models.State.BuildState;
using Models.State.Interfaces;
using Models.State.PieceState;
using UnityEditor;
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
            // move piece to vector
            gameObject.transform.position = _position.Vector;

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (_renderInfo.SpriteAssetPath != "")
                spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_renderInfo.SpriteAssetPath);


            var transparencyModifier = _buildState.Turns switch
            {
                0 => 0.7f,
                1 => 0.6f,
                _ => 1f / _buildState.Turns
            };
            spriteRenderer.color = new Color(1f, 1f, 1f, transparencyModifier);
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("BuildingPieces")?.transform;
        }

        [Inject]
        public void Construct(Position position, BuildState buildState)
        {
            _renderInfo = new PieceRenderInfo(buildState.BuildingPiece);
            _position = position;
            _buildState = buildState;
        }

        public class Factory : PlaceholderFactory<Position, BuildState, BuildingPieceSpawner>
        {
        }
    }
}