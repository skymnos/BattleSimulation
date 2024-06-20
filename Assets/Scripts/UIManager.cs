using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.onUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.onUI = false;
    }
}
