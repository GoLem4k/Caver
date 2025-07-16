using UnityEngine;

[CreateAssetMenu(fileName = "upgradeTemplate", menuName = "Scriptable Objects/upgradeTemplate")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private Sprite _image;
    [SerializeField] private UpgradeType _type;
    [SerializeField] private int _id;
    [SerializeField] private int _needId;
    [SerializeField] private int _totalCount;
    [SerializeField] private string _discription;

    public Sprite image => _image;
    public UpgradeType type => _type;
    public int id => _id;
    public int needId => _needId;
    public int totalCount => _totalCount;
    public string discription => _discription;


    public override string ToString()
    {
        return $"ID: {_id}, TYPE: {_type}, TOTAL COUNT: {_totalCount}";
    }
}

public enum UpgradeType
{
    Passive,
    Support,
    Active,
    Magic
}
