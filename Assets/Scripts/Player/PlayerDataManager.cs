using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDataManager : PausedBehaviour
{
    public static PlayerDataManager I { get; private set; }


    [SerializeField] public int skillPoints;
    [SerializeField] private GameObject _expBar;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _expStep;
    public float _exp;
    
    public void Initialize()
    {
        if (I == null) I = this;
        _expBar.transform.localScale = new Vector3Int(0, 1, 0);
        _exp = 0;
        skillPoints = 0;
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
            skillPoints++;
        }
    }
}
