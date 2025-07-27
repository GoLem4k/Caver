using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayNightController : PausedBehaviour
{
    public static DayNightController I { get; private set; }
    
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Image _image;
    [SerializeField] private Color _DayColor;
    [SerializeField] private Color _NightColor;
    [SerializeField] private float _DayDuration;
    [SerializeField] private float _NightDuration;

    [SerializeField] private float _dayIdensity;
    [SerializeField] private float _nightIdensity;

    private float _targetGlobalLightVolume;
    private float _phaseTimer;
    private TimePhase _currentPhase;
    
    
    private void Start()
    {
        if (I == null) I = this;
        _targetGlobalLightVolume = _dayIdensity;
        _currentPhase = TimePhase.Day;
        _phaseTimer = _DayDuration;
    }

    protected override void GameUpdate()
    {
        if (_light2D.intensity != _targetGlobalLightVolume)
        {
            switch (_currentPhase)
            {
                case TimePhase.Day:
                    _light2D.intensity = Mathf.Clamp(_light2D.intensity + Time.deltaTime * 0.25f, _nightIdensity, _dayIdensity);
                    break;
                case TimePhase.Night:
                    _light2D.intensity = Mathf.Clamp(_light2D.intensity - Time.deltaTime * 0.25f, _nightIdensity, _dayIdensity);
                    break;
                default:
                    _light2D.intensity = Mathf.Clamp(_light2D.intensity + Time.deltaTime * 0.25f, _nightIdensity, 1f);
                    break;
                
            }            
        }

            
        _image.color = Color.Lerp(_NightColor, _DayColor, Mathf.Clamp01(transform.localScale.x / 1));
        switch (_currentPhase)
        {
            case TimePhase.Day:
                DayIteration();
                ChangeGlobalLight(TimePhase.Day);
                break;
            case TimePhase.Night:
                NightIteration();
                ChangeGlobalLight(TimePhase.Night);
                break;
            default:
                Debug.Log("Неопределённая фаза дня");
                break;
        }
    }

    private void DayIteration()
    {
        _phaseTimer = Mathf.Clamp(_phaseTimer - Time.deltaTime, 0, _DayDuration);
        if (_phaseTimer == 0)
        {
            _currentPhase = TimePhase.Night;
            _NightDuration += 10f;
        }
        transform.localScale = new Vector3(_phaseTimer / _DayDuration, 1, 0);
    }

    private void NightIteration()
    {
        _phaseTimer = Mathf.Clamp(_phaseTimer + Time.deltaTime, 0, _NightDuration);
        if (_phaseTimer >= _NightDuration)
        {
            _phaseTimer = _DayDuration;
            _currentPhase = TimePhase.Day;
            _DayDuration *= 0.95f;
        }
        transform.localScale = new Vector3(_phaseTimer / _NightDuration, 1, 0);
    }

    private void ChangeGlobalLight(TimePhase phase)
    {
        switch (phase)
        {
            case TimePhase.Day:
                _targetGlobalLightVolume = _dayIdensity;
                break;
            case TimePhase.Night:
                _targetGlobalLightVolume = _nightIdensity;
                break;
            default:
                Debug.Log("Неопределённая фаза дня");
                break;
        }
    }

    public bool IsDay()
    {
        return _currentPhase == TimePhase.Day;
    }
}

public enum TimePhase
{
    None,
    Day,
    Night
}
