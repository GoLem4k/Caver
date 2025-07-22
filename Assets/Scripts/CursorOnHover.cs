using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOnHover : PausedBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ID курсора при наведении"), Tooltip("ID: 0 - Default, 1 - Active, 2 - Break")]
    public int state;
    
    
    
    void Start()
    {
        CursorManager.I.ResetCursor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.I.ResetCursor();
    }

    public override void GameUpdate()
    {
        // 1. Если мышь вне экрана — сбросить курсор
        if (!Camera.main.pixelRect.Contains(Input.mousePosition))
        {
            return;
        }

        // 2. Если мышь над UI — ничего не делать (оставить курсор как есть)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 3. Проверка тайла под курсором
        if (TileManager.I.IsTileByWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            SetCursor();
        }
        else
        {
            CursorManager.I.ResetCursor();
        }
    }

    private void SetCursor()
    {
        switch (state)
        {
            case 1:
                CursorManager.I.SetHoverCursor();
                break;
            case 2:
                CursorManager.I.SetBreakCursor();
                break;
            default:
                CursorManager.I.ResetCursor();
                break;
        }
    }
}

