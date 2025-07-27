using UnityEngine;

public class Altar : PausedBehaviour
{
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private int id;
    [SerializeField] private int essenceCost;
    private bool playerInZone = false;
    [SerializeField] private GameObject eIcon;
    [SerializeField] private ParticleSystem _particleSystem;
    
    
    protected override void GameUpdate()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && PlayerDataManager.I.TryRemoveEssencePoint(essenceCost))
        {
            _particleSystem.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = _activatedSprite;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            eIcon.SetActive(true);
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
