using UnityEngine.EventSystems;

public class UIScaler : Scaler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData) => base.OnMouseEnter();
    public void OnPointerExit(PointerEventData eventData) => base.OnMouseExit();
}