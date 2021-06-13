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
        public Position Position { get; set; }
        public IPieceInfo Info { get; private set; }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (Info.SpriteAssetPath != "")
                _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Info.SpriteAssetPath);
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;
            gameObject.transform.position = Position.Vector;
        }

        [Inject]
        public void Construct(
            IPieceInfo pieceInfo,
            Position position
        )
        {
            Info = pieceInfo;
            Position = position;
        }

        public override string ToString() => $"{Info}\n{Position}\n";

        public class Factory : PlaceholderFactory<IPieceInfo, Position, PieceMono>
        {
        }
    }
}