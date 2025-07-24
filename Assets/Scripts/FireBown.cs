using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireBown : PausedBehaviour
{
    private Light2D _light2D;
    private bool playerInZone = false;
    [SerializeField] private GameObject eIcon;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private CircleCollider2D _circleCollider2D;
 
    private void Start()
    {
        _light2D = GetComponent<Light2D>();
    }
    public override void GameUpdate()
    {
        _circleCollider2D.radius = _light2D.pointLightOuterRadius;
        _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius - Time.deltaTime * 0.05f, 0, 3.5f);
        var main = _particleSystem.main;
        main.startLifetime = Mathf.Lerp(0.5f, 2f, Mathf.InverseLerp(0.5f, 3.5f, _light2D.pointLightOuterRadius));
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && PlayerDataManager.I.TryRemoveEssencePoint(10))
        {
            _light2D.enabled = true;
            _particleSystem.gameObject.SetActive(true);
            if (_light2D.pointLightOuterRadius == 0)
            {
                _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius + 1.5f, 0, 3.5f);
            } else _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius + 0.5f, 0, 3.5f);
        }

        if (_light2D.pointLightOuterRadius <= 1f)
        {
            _light2D.pointLightOuterRadius = 0;
            _particleSystem.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (_light2D.pointLightOuterRadius < 1f) eIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            eIcon.SetActive(false);
        }
    }
}
