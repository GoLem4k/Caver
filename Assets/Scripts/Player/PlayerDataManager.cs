using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDataManager : PausedBehaviour
{
    public static PlayerDataManager I { get; private set; }
    
    [SerializeField] private GameObject _expBar;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _expStep;
    private float _exp;
    
    public void Initialize()
    {
        if (I == null) I = this;
        _expBar.transform.localScale = new Vector3Int(0, 1, 0);
        _exp = 0;
        StartCoroutine(ExpAdder());
    }

    public override void GameUpdate()
    {
        if (_exp >= _maxExp)
        {
            _exp -= _maxExp;
            _maxExp += _expStep;
            _expBar.transform.localScale = new Vector3Int(0, 1, 0);
            PAUSE = true;
            UpgradesManager.I.UpgradeChoice();
        }
    }

    public IEnumerator ExpAdder()
    {
        while (true)
        {
            if (!PAUSE)
            {
                _exp++;
                _expBar.transform.localScale = new Vector3(_exp / _maxExp, 1, 0);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null; // отдаёт управление, ждёт кадр
            }
        }
    }
}
