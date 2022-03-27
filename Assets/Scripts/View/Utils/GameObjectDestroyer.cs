using UnityEngine;

namespace View.Utils
{
    public static class GameObjectDestroyer
    {
        public static void DestroyChildrenOfObjectWithTag(string tag)
        {
            var piecesGameObject = GameObject.FindGameObjectWithTag(tag);
            if (piecesGameObject.transform.childCount > 0)
                DeleteChildrenOf(piecesGameObject);
        }

        private static void DeleteChildrenOf(GameObject obj)
        {
            var childCount = obj.transform.childCount;
            for (var i = 0; i < childCount; i++) Object.Destroy(obj.transform.GetChild(i).gameObject);
        }
    }
}