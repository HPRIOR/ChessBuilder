using Models.State.Board;
using Models.State.BuildState;
using UnityEngine;
using View.Interfaces;
using View.Prefab.Factories;
using View.Utils;
using Zenject;

namespace View.Prefab.Spawners
{
    public class BuildingPieceSpawner : MonoBehaviour
    {
        private BuildTileState _buildTileState;
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
                spriteRenderer.sprite = Resources.Load<Sprite>(_renderInfo.SpriteAssetPath);


            var transparencyModifier = _buildTileState.Turns switch
            {
                0 => 0.7f,
                1 => 0.6f,
                _ => 1f / _buildTileState.Turns
            };
            spriteRenderer.color = new Color(1f, 1f, 1f, transparencyModifier);
            thisGameObject.transform.parent = GameObject.FindGameObjectWithTag("BuildingPieces")?.transform;

            _spriteFactory.Create(
                thisGameObject.transform.position + new Vector3(0.3f, 0.3f),
                2.5f,
                thisGameObject,
                $"Sprites/Numbers/{_buildTileState.Turns.ToString()}",
                3
            );
        }

        [Inject]
        public void Construct(Position position, BuildTileState buildTileState, SpriteFactory spriteFactory)
        {
            _renderInfo = new PieceRenderInfo(buildTileState.BuildingPiece);
            _position = position;
            _buildTileState = buildTileState;
            _spriteFactory = spriteFactory;
        }

        public class Factory : PlaceholderFactory<Position, BuildTileState, BuildingPieceSpawner>
        {
        }
    }
}