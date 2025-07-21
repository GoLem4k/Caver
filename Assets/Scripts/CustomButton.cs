using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.I.SetHoverCursor();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.I.ResetCursor();
    }
}