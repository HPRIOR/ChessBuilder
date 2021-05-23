using Models.State.Board;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace View.Renderers
{
    public class PieceMono : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public BoardPosition BoardPosition { get; set; }
        public IPieceInfo Info { get; private set; }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (Info.SpriteAssetPath != "")
                _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Info.SpriteAssetPath);
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;
            gameObject.transform.position = BoardPosition.Vector;
        }

        [Inject]
        public void Construct(
            IPieceInfo pieceInfo,
            BoardPosition boardPosition
        )
        {
            Info = pieceInfo;
            BoardPosition = boardPosition;
        }

        public override string ToString()
        {
            return $"{Info}\n{BoardPosition}\n";
        }

        public class Factory : PlaceholderFactory<IPieceInfo, BoardPosition, PieceMono>
        {
        }
    }
}