using Models.State.Board;
using UnityEngine;
using Zenject;

namespace View.Utils.Prefab.Spawners
{
    public class TileSpawner : MonoBehaviour
    {
        private Color32 _colour;
        private GameObject _parent;
        private Position _position;

        public void Start()
        {
            gameObject.transform.position = _position.Vector;
            gameObject.transform.parent = _parent.transform;
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = _colour;
        }

        [Inject]
        public void Construct(Position position, GameObject parent, Color32 colour)
        {
            _position = position;
            _parent = parent;
            _colour = colour;
        }

        public class Factory : PlaceholderFactory<Position, GameObject, Color32, TileSpawner>
        {
        }
    }
}