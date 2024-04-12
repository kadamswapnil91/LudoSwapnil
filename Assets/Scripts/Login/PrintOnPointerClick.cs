using UnityEngine;
using UnityEngine.EventSystems;

public class PrintOnPointerClick : MonoBehaviour, IPointerClickHandler
{
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        print("Clicked " + name);
    }
}