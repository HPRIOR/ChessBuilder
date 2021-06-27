using Models.State.Board;
using Models.State.Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace View.Utils
{
    public class PieceSpawner : MonoBehaviour
    {
        public Position Position { get; private set; }
        public IPieceInfo Info { get; private set; }

        private void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (Info.SpriteAssetPath != "")
                spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Info.SpriteAssetPath);

            // set parent object and piece position
            GameObject pieceGameObject;
            (pieceGameObject = gameObject).transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;

            // change position of game object 
            Vector3 position = Position.Vector;
            position += new Vector3(0, 0, -1); // move 'forward' so that there are no box collider clashes
            pieceGameObject.transform.position = position;
        }

        [Inject]
        public void Construct(IPieceInfo pieceInfo, Position position)
        {
            Info = pieceInfo;
            Position = position;
        }


        public override string ToString() => $"{Info}\n{Position}\n";

        public class Factory : PlaceholderFactory<IPieceInfo, Position, PieceSpawner>
        {
        }
    }
}