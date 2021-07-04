using UnityEditor;
using UnityEngine;
using Zenject;

namespace View.Prefab.Spawners
{
    public class SpriteSpawner : MonoBehaviour
    {
        private string _assetPath;
        private GameObject _parent;
        private Vector3 _position;
        private float _scale;
        private int _sortingOrder;

        private void Start()
        {
            var thisGameObject = gameObject;
            thisGameObject.transform.position = _position;
            thisGameObject.transform.localScale = new Vector3(_scale, _scale);

            thisGameObject.transform.parent = _parent.transform;

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = _sortingOrder;

            spriteRenderer.sprite = Resources.Load<Sprite>(_assetPath);
        }


        [Inject]
        public void Construct(Vector3 position, float scale, GameObject parent, string assetPath, int sortingOrder)
        {
            _position = position;
            _scale = scale;
            _parent = parent;
            _assetPath = assetPath;
            _sortingOrder = sortingOrder;
        }

        public class Factory : PlaceholderFactory<Vector3, float, GameObject, string, int, SpriteSpawner>
        {
        }
    }
}