using System;
using UnityEngine;

public class expOrb : PausedBehaviour
{
    public Transform target;             
    public float speed = 2f;              
    public float moveRadius = 5f;          
    
    [SerializeField] private ParticleSystem _expParticle;
    [SerializeField] private ParticleSystem _expParticleInstantiate;

    private void Start()
    {
        target = VectorMovementController.playerTransform;
    }

    public override void GameUpdate ()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= moveRadius)
        {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDataManager.I._exp += 10;
            PlayerDataManager.I.AddEssencePoint(1);
            _expParticleInstantiate = Instantiate(_expParticle, target);
            Destroy(gameObject);
        }
    }
}
