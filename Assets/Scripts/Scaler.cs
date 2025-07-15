using UnityEngine;

public class Scaler : MonoBehaviour
{
    private Vector3 defaultScale;
    private Vector3 targetScale;
    private Quaternion defaultRotation;

    private bool scaling = false;
    private bool rotating = false;
    [SerializeField] protected bool ignoreMouse = false;
    [SerializeField] private bool simulated = false; // Флаг симуляции

    [SerializeField] protected float rescale = 1.2f;
    [SerializeField] protected float scaleSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float amplitude = 5f;
    [SerializeField] protected float frequency = 5f;

    protected virtual void Start()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
        defaultRotation = transform.rotation;
    }

    protected virtual void OnMouseEnter()
    {
        if (ignoreMouse || simulated) return; 
        ApplyScaling(true);
    }

    protected virtual void OnMouseExit()
    {
        if (ignoreMouse || simulated) return;
        ApplyScaling(false);
    }

    protected virtual void Update()
    {
        if (scaling)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                scaling = false;
            }
        }

        if (rotating)
        {
            float angle = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, defaultRotation) < 0.5f)
            {
                transform.rotation = defaultRotation;
                rotating = false;
            }
        }
    }

    public virtual void SimulatePointerEnter()
    {
        simulated = true;
        ApplyScaling(true);
    }

    public virtual void SimulatePointerExit()
    {
        simulated = false;
        ApplyScaling(false);
    }

    public virtual void SetIgnoreMouseMod(bool t)
    {
        ignoreMouse = t;
        if (t) SimulatePointerEnter();
        else SimulatePointerExit();
    }

    public virtual void SwitchIgnoreMouseMod()
    {
        SetIgnoreMouseMod(!ignoreMouse);
    }

    protected virtual void ApplyScaling(bool increase)
    {
        targetScale = increase ? defaultScale * rescale : defaultScale;
        scaling = true;
        rotating = increase;
    }
}
