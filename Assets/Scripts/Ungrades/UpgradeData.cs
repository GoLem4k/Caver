using UnityEngine;

[CreateAssetMenu(fileName = "upgradeTemplate", menuName = "Scriptable Objects/upgradeTemplate")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private Sprite _imageActive;
    [SerializeField] private Sprite _imageInactive;
    [SerializeField] private int _id;
    [SerializeField] private int _maxLevel;
    [SerializeField] private string _discription;

    public Sprite imageActive => _imageActive;
    public Sprite imageInactive => _imageInactive;
    public int id => _id;
    public int maxLevel => _maxLevel;
    public string discription => _discription;


    public override string ToString()
    {
        return $"ID: {_id} TOTAL COUNT: {_maxLevel}";
    }
}
