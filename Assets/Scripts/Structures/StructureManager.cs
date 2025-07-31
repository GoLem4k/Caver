using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StructureManager : MonoBehaviour
{
    public static StructureManager I { get; private set; }
    private Dictionary<Vector3Int, Structure> _structurePool;

    [SerializeField] private GameObject _fireBowl;
    [SerializeField] private GameObject _altar;

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

    public bool TrySpawnStructure(Vector3Int pos, StructureType type)
    {
        GameObject structure;
        if (!IsStructureOnPos(pos))
        {
            switch (type)
            {
                case StructureType.FireBowl:
                    structure = _fireBowl;
                    break;
                case StructureType.Altar:
                    structure = _altar;
                    break;
                default:
                    Debug.Log("Неудачная попытка заспавнить структуру, структура не определена");
                    return false;
            }

            structure.transform.position = pos + new Vector3(0.5f, 0.5f);
            GameObject newStruct = Instantiate(structure);
            _structurePool.Add(pos, newStruct.GetComponent<Structure>());
            
        }
        return true;
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

    public enum StructureType
    {
        None,
        FireBowl,
        Altar
    }
}
