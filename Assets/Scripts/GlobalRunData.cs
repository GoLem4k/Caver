using UnityEngine;

[CreateAssetMenu(fileName = "GlobalRunData", menuName = "Scriptable Objects/GlobalRunData")]
public class GlobalRunData : ScriptableObject
{
    public WorldSize WorldSize;
    public int SEED;
}

public enum WorldSize { Small, Medium, Large }