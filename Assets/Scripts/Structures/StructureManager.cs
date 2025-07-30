using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StructureManager : MonoBehaviour
{
    public static StructureManager I { get; private set; }
    public Dictionary<Vector3Int, Structure> _structurePool;

    public void Initialize()
    {
        if (I == null) I = this;
        _structurePool = new Dictionary<Vector3Int, Structure>();
    }

    public bool AddStructure(Structure obj)
    {
        if (obj == null) return false;
        _structurePool.Add(Vector3Int.FloorToInt(obj.transform.position - new Vector3(0.5f, 0.5f)), obj);
        return true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetStructurePositions();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckStructurePositionOnBlockTilemap();
        }
    }

    public bool IsStructureOnPos(Vector3Int pos)
    {
        return _structurePool[pos] != null;
    }
    
    public void GetStructurePositions()
    {
        foreach (var structure in _structurePool)
        {
            Debug.Log(structure.Key);
        }
    }

    public void CheckStructurePositionOnBlockTilemap()
    {
        foreach (var structure in _structurePool)
        {
            Debug.Log(IsStructureOnPos(structure.Key));
        }
    }
}
