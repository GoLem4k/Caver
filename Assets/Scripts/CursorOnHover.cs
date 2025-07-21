using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class CursorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D hoverCursor;
    public Texture2D defaultCursor;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, new Vector2(16, 16), CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(hoverCursor, new Vector2(16, 16), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursor, new Vector2(16, 16), CursorMode.Auto);
    }
}

