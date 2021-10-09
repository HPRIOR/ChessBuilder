using UnityEngine;
using UnityEngine.EventSystems;

namespace View.UserInput
{
    public class HoverSelect : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("log");
        }
    }
}