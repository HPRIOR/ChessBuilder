using UnityEngine;
using View.Prefab.Spawners;

namespace View.Prefab.Factories
{
    public class SpriteFactory
    {
        private readonly SpriteSpawner.Factory _spriteFactory;

        public SpriteFactory(SpriteSpawner.Factory spriteFactory)
        {
            _spriteFactory = spriteFactory;
        }

        public SpriteSpawner Create(Vector3 position, float scale, GameObject parent, string assetPath,
            int sortingOrder) => _spriteFactory.Create(position, scale, parent, assetPath, sortingOrder);
    }
}