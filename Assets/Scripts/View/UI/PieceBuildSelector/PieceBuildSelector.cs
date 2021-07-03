using System;
using Models.State.BuildState;
using Models.State.PieceState;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Prefab.Factories;
using View.Utils;
using Zenject;

namespace View.UI.PieceBuildSelector
{
    public class PieceBuildSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _canBuild;
        private PieceType _pieceType;
        private Vector3 _renderPosition;
        private Action<PieceType> _selectPiece;
        private SpriteFactory _spriteFactory;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            var thisGameObject = gameObject;
            _spriteRenderer = GetComponent<SpriteRenderer>();


            RenderSprite();
            ChangeBoxColliderSize();
            thisGameObject.transform.position = _renderPosition + new Vector3(0, 0, -2);
            RenderBuildPointsIcon(thisGameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_canBuild) _selectPiece(_pieceType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_canBuild) _selectPiece(PieceType.NullPiece);
        }

        [Inject]
        public void Construct(Vector3 renderPosition, PieceType pieceType, Action<PieceType> selectPiece, bool canBuild,
            SpriteFactory spriteFactory)
        {
            _renderPosition = renderPosition;
            _pieceType = pieceType;
            _selectPiece = selectPiece;
            _canBuild = canBuild;
            _spriteFactory = spriteFactory;
        }

        private void ChangeBoxColliderSize()
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = _spriteRenderer.size;
        }

        private void RenderBuildPointsIcon(GameObject thisGameObject)
        {
            var buildPoints = BuildPoints.PieceCost[_pieceType];
            _spriteFactory.Create(
                thisGameObject.transform.position + new Vector3(0.2f, 0.2f),
                2.5f,
                thisGameObject,
                $"Assets/Sprites/Numbers/{buildPoints.ToString()}.png",
                3
            );
        }

        private void RenderSprite()
        {
            var pieceSpritePath = PieceSpriteAssetManager.GetSpriteAsset(_pieceType);
            _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pieceSpritePath);
            _spriteRenderer.transform.localScale = new Vector2(0.25f, 0.25f);
            if (!_canBuild) _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }

        public class Factory : PlaceholderFactory<Vector3, PieceType, Action<PieceType>, bool, PieceBuildSelector>
        {
        }
    }
}