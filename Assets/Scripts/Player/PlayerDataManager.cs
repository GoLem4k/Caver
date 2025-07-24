using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDataManager : PausedBehaviour
{
    public static PlayerDataManager I { get; private set; }


    [SerializeField] public int skillPoints;
    [SerializeField] public int essencePoints;
    
    [SerializeField] private GameObject _expBar;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _expStep;

    [SerializeField] private GameObject skillPointsUI;
    private TextMeshProUGUI _textMeshProUGUI1;
    
    [SerializeField] private GameObject essenceCounterUI;
    private TextMeshProUGUI _textMeshProUGUI2;


    public float _exp;
    
    public void Initialize()
    {
        if (I == null) I = this;
        _expBar.transform.localScale = new Vector3Int(0, 1, 0);
        _exp = 0;
        skillPoints = 0;
        essencePoints = 0;
        _textMeshProUGUI1 = skillPointsUI.GetComponent<TextMeshProUGUI>();
        _textMeshProUGUI2 = essenceCounterUI.GetComponent<TextMeshProUGUI>();
    }
    public override void GameUpdate()
    {
        _exp += (RunData.I.expPerSecond * Time.deltaTime) * RunData.I.globalExpMultiplier;
        _expBar.transform.localScale = new Vector3(_exp / _maxExp, 1, 0);
        if (_exp >= _maxExp)
        {
            _exp -= _maxExp;
            _maxExp += _expStep;
            _expBar.transform.localScale = new Vector3Int(0, 1, 0);
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
}
