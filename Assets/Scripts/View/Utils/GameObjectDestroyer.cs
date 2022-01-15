using UnityEngine;

namespace View.Utils
{
    public static class GameObjectDestroyer
    {
        public static void DestroyChildrenOfObjectWithTag(string tag)
        {
            var piecesGameObject = GameObject.FindGameObjectWithTag(tag);
            if (piecesGameObject.transform.childCount > 0)
                foreach (Transform child in piecesGameObject.transform)
                    Object.Destroy(child.gameObject);
        }
    }
}