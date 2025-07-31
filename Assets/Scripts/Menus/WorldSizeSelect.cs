using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSizeSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _HightLightSprite;
    [SerializeField] private Sprite _SelectedSprite;
    [SerializeField] private Image _image;
    [SerializeField] private WorldSize _worldSize;
    [SerializeField] public static List<WorldSizeSelect> WorldSizeSelects = new List<WorldSizeSelect>();

    private void Awake()
    {
        WorldSizeSelects.Add(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = _HightLightSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_image.sprite != _SelectedSprite)
            _image.sprite = _defaultSprite;
    }

    public void ResetSprite()
    {
        _image.sprite = _defaultSprite;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var wss in WorldSizeSelects)
        {
            wss.ResetSprite();
        }
        _image.sprite = _SelectedSprite;
        StartMenu.I.SetWorldSize(_worldSize);
    }
}
