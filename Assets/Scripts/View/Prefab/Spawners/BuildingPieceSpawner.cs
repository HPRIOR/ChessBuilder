using Models.State.Board;
using Models.State.BuildState;
using Models.State.Interfaces;
using Models.State.PieceState;
using UnityEditor;
using UnityEngine;
using View.Utils.Prefab.Factories;
using Zenject;

namespace View.Utils.Prefab.Spawners
{
    public class BuildingPieceSpawner : MonoBehaviour
    {
        private BuildState _buildState;
        private Position _position;
        private IPieceRenderInfo _renderInfo;
        private SpriteFactory _spriteFactory;

        public void Start()
        {
            var thisGameObject = gameObject;
            // move piece to vector
            thisGameObject.transform.position = _position.Vector;

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
            thisGameObject.transform.parent = GameObject.FindGameObjectWithTag("BuildingPieces")?.transform;

            _spriteFactory.Create(
                thisGameObject.transform.position + new Vector3(0.3f, 0.3f),
                2.5f,
                thisGameObject,
                $"Assets/Sprites/Numbers/{_buildState.Turns.ToString()}.png",
                3
            );
        }

        [Inject]
        public void Construct(Position position, BuildState buildState, SpriteFactory spriteFactory)
        {
            _renderInfo = new PieceRenderInfo(buildState.BuildingPiece);
            _position = position;
            _buildState = buildState;
            _spriteFactory = spriteFactory;
        }

        public class Factory : PlaceholderFactory<Position, BuildState, BuildingPieceSpawner>
        {
        }
    }
}