using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager I;
    public Texture2D hoverCursor;
    public Texture2D breakCursor;
    public Vector2 hotspot;

    void Awake()
    {
        I = this;
    }

    public void SetHoverCursor() => Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    public void ResetCursor() => Cursor.SetCursor(null, hotspot, CursorMode.Auto);
    public void SetBreakCursor() => Cursor.SetCursor(breakCursor, hotspot, CursorMode.Auto);
}
