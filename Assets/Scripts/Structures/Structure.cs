using UnityEngine;

public class Structure : PausedBehaviour
{

    private float _updateTimer;
    [SerializeField] private GameObject _interactIcon;
    [SerializeField] private KeyCode _keyCode;
    private bool isPlayerInInteractZone;
    protected bool canInteract = true;
    public float _behaviorUpdateInterval = 0.2f;
    [SerializeField] private Transform playerTransform;
    
    
    public void Initialize()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
        
    }

    protected override void GameUpdate()
    {
        if (Input.GetKeyDown(_keyCode) && isPlayerInInteractZone && canInteract) Interaction();
        _updateTimer += Time.deltaTime;
        if (_updateTimer < _behaviorUpdateInterval) return;
        UpdateBehaviour(_updateTimer);
        UpdateBehaviourIntervalController();
        _updateTimer = 0f;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            isPlayerInInteractZone = true;
            _interactIcon.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInInteractZone = false;
            _interactIcon.SetActive(false);
        }
    }

    protected void StopInteraction()
    {
        canInteract = false;
        _interactIcon.SetActive(false);
    }

    protected void UpdateBehaviourIntervalController()
    {
        if ((playerTransform.position - transform.position).sqrMagnitude < 400)
            _behaviorUpdateInterval = 0.2f;
        else _behaviorUpdateInterval = 1f;
    }
        

    protected virtual void UpdateBehaviour(float deltaTime)
    {
        
    }
    protected virtual void Interaction()
    {

    }
}
