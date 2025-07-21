using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchableImage : MonoBehaviour
{
    [SerializeField] private Sprite _inacrive;
    [SerializeField] private Sprite _acrive;
    [SerializeField] private Sprite _super;

    private Image _currentSptire;

    private void Awake()
    {
        _currentSptire = GetComponent<Image>();
        SetInactiveSprite();
    }

    public void SetInactiveSprite()
    {
        _currentSptire.sprite = _inacrive;
    }
    public void SetActiveSprite()
    {
        _currentSptire.sprite = _acrive;
    }

    public void SetSuperSprite()
    {
        _currentSptire.sprite = _super;
    }
    
    
    
}
