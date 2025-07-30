using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireBown : Structure
{
    private Light2D _light2D;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private CircleCollider2D _circleCollider2D;
 
    protected override void OnInitialize()
    {
        _light2D = GetComponent<Light2D>();
    }
    protected override void UpdateBehaviour(float deltaTime)
    {
        _circleCollider2D.radius = _light2D.pointLightOuterRadius;
        _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius - deltaTime * 0.05f, 0, 3.5f * RunData.I.fireBowlRange);
        var main = _particleSystem.main;
        main.startLifetime = Mathf.Lerp(0.5f, 2f, Mathf.InverseLerp(0.5f, 3.5f * RunData.I.fireBowlRange, _light2D.pointLightOuterRadius));


        if (_light2D.pointLightOuterRadius <= 1f)
        {
            _light2D.pointLightOuterRadius = 0;
            _particleSystem.gameObject.SetActive(false);
        }
    }

    protected override void Interaction(){
        if (PlayerDataManager.I.TryRemoveEssencePoint(1))
        {
            _light2D.enabled = true;
            _particleSystem.gameObject.SetActive(true);
            if (_light2D.pointLightOuterRadius == 0)
            {
                _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius + 1f + 0.5f * RunData.I.essenceEfficiency, 0, 3.5f * RunData.I.fireBowlRange);
            } else _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius + 0.5f * RunData.I.essenceEfficiency, 0, 3.5f * RunData.I.fireBowlRange);
        }
    }
}
