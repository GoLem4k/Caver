using System;
using UnityEngine;

public class Altar : Structure
{
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private int id;
    [SerializeField] private int essenceCost;
    [SerializeField] private ParticleSystem _particleSystem;
    private SpriteRenderer _spriteRenderer;


    protected override void OnInitialize()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void UpdateBehaviour(float deltaTime)
    {
        
    }
    protected override void Interaction()
    {
        if (PlayerDataManager.I.TryRemoveEssencePoint(essenceCost))
        {
            _particleSystem.gameObject.SetActive(true);
            _spriteRenderer.sprite = _activatedSprite;
            StopInteraction();
        }
    }
}
