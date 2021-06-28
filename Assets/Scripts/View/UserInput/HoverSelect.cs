using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.UserInput
{
    public class HoverSelect : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("log");
            var image = GetComponent<Image>();
        }
    }
}