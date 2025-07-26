using UnityEngine;

[CreateAssetMenu(fileName = "GlobalRunData", menuName = "Scriptable Objects/GlobalRunData")]
public class GlobalRunData : ScriptableObject
{
    public int WorldSize;
    public int SEED;
}
