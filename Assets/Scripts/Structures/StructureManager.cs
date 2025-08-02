using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StructureManager : MonoBehaviour
{
    public static StructureManager I { get; private set; }
    private Dictionary<Vector3Int, Structure> _structurePool;

    [SerializeField] private GameObject _fireBowl;
    [SerializeField] private GameObject[] _altar;
    private int altarID;

    public Vector3Int[] NEIGHBOURS8X = new Vector3Int[] {
        new Vector3Int(1, 0, 0),     // справа
        new Vector3Int(-1, 0, 0),    // слева
        new Vector3Int(0, 1, 0),     // сверху
        new Vector3Int(0, -1, 0),    // снизу
        new Vector3Int(-1, 1, 0),    // сверху и слева
        new Vector3Int(1, 1, 0),    // сверху и справа
        new Vector3Int(-1, -1, 0),    // снизу и слева
        new Vector3Int(1, -1, 0)    // снизу и справа
    };
    
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

    public bool TrySpawnStructure(Vector3Int pos, StructureType type, bool isForced = false)
    {
        GameObject structure;
        if ((!IsStructureOnPos(pos) && !TileManager.I.IsBlockOnPos(pos)) || isForced)
        {
            switch (type)
            {
                case StructureType.FireBowl:
                    structure = _fireBowl;
                    break;
                case StructureType.Altar:
                    structure = _altar[altarID++];
                    break;
                default:
                    Debug.Log("Неудачная попытка заспавнить структуру, структура не определена");
                    return false;
            }

            if (isForced)
            {
                foreach (var offset in NEIGHBOURS8X)
                {
                    TileManager.I.SetCell(pos + offset, BlockType.None);
                }
            }
            
            structure.transform.position = pos + new Vector3(0.5f, 0.5f);
            GameObject newStruct = Instantiate(structure);
            _structurePool.Add(pos, newStruct.GetComponent<Structure>());
            newStruct.GetComponent<Structure>().Initialize();

        } else return false;
        return true;
    }

    public bool IsStructureOnPos(Vector3Int pos)
    {
        return _structurePool.ContainsKey(pos);
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
