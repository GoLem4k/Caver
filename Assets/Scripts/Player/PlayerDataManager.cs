using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerDataManager : PausedBehaviour
{
    public static PlayerDataManager I { get; private set; }


    [SerializeField] public int skillPoints;
    [SerializeField] public int essencePoints;
    
    [SerializeField] private Image _expBar;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _expStep;

    [SerializeField] private Image _hpBar;
    [SerializeField] private float _currentHP;

    private bool isNearFire;

    [SerializeField] private GameObject skillPointsUI;
    private TextMeshProUGUI _textMeshProUGUI1;
    
    [SerializeField] private GameObject essenceCounterUI;
    private TextMeshProUGUI _textMeshProUGUI2;


    public float _exp;
    
    public void Initialize()
    {
        isNearFire = false;
        if (I == null) I = this;
        _exp = 0;
        skillPoints = 0;
        essencePoints = 0;
        _textMeshProUGUI1 = skillPointsUI.GetComponent<TextMeshProUGUI>();
        _textMeshProUGUI2 = essenceCounterUI.GetComponent<TextMeshProUGUI>();
    }
    protected override void GameUpdate()
    {
        if (isNearFire == false && !DayNightController.I.IsDay()) _currentHP = Mathf.Clamp(_currentHP - Time.deltaTime * 5f, 0, RunData.I.health);
        if (_currentHP < RunData.I.health)
        {
            _currentHP = Mathf.Clamp(_currentHP + Time.deltaTime * RunData.I.regenerationSpeed, 0, RunData.I.health);
        }

        _hpBar.fillAmount = _currentHP / RunData.I.health;
        
        _exp += (RunData.I.expPerSecond * Time.deltaTime) * RunData.I.globalExpMultiplier;
        _expBar.fillAmount = _exp / _maxExp;
        if (_exp >= _maxExp)
        {
            _exp -= _maxExp;
            _maxExp += _expStep;
            _expBar.fillAmount = 0;
            AddSkillPoint();
        }
    }

    private void AddSkillPoint()
    {
        skillPoints++;
        _textMeshProUGUI1.text = $"SKILL POINTS: {skillPoints}";
    }

    public void RemoveSkillPoint()
    {
        skillPoints--;
        _textMeshProUGUI1.text = $"SKILL POINTS: {skillPoints}";
    }
    
    public void AddEssencePoint(int count)
    {
        essencePoints += count;
        _textMeshProUGUI2.text = essencePoints.ToString();
    }

    public bool TryRemoveEssencePoint(int count)
    {
        if (count <= essencePoints)
        {
            essencePoints -= count;
            _textMeshProUGUI2.text = essencePoints.ToString();
            return true;
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("FireBowl")) isNearFire = true;
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("FireBowl")) isNearFire = false;
    }
}
