using System.ComponentModel;
using UnityEngine;

public class RunData : MonoBehaviour
{
    public static RunData I { get; private set; }
    [SerializeField] private GlobalRunData _GlobalRunData;
    
    [Header("Сид забега")] public int SEED;
    [Header("Размер мира")] public int WorldRadius;
    [Header("Размер чанка где может заспавнится чаша")] public int fireBowlChank = 16;
    
    [Header("Урон при отскоке и ударе")] public float damage = 30f;
    [Header("КД удара")] public float damageCooldown = 1f;
    [Header("Скорость игрока")] public float movementSpeed = 3f;
    [Header("Сила отскока")] public float reboundScale = 5f;
    [Header("Опыт в секунду")] public float expPerSecond = 1f;
    [Header("Запас здоровья")] public float health = 100f;
    [Header("Скорость регенерации")] public float regenerationSpeed = 1f;
    [Header("Возможность критовать")] public bool canCrit = false;
    [Header("Шанс крита")] public float critChance = 0f;
    [Header("Возможность стрелять")] public bool canShoot = false;
    [Header("Урон пуль")] public float bulletDamage = 10f;
    [Header("Опыт за поломку блока")] public float blockBreakExp = 5f;
    [Header("Глобальный множитель опыта")] public float globalExpMultiplier = 1f;
    [Header("КД рывка")] public float leapCooldown = 2.5f;
    
    public void Initialize()
    {
        if (I == null) I = this;
        switch (_GlobalRunData.WorldSize)
        {
            case 1:
                WorldRadius = 64;
                fireBowlChank = 16;
                break;
            case 2:
                WorldRadius = 96;
                fireBowlChank = 20;
                break;
            case 3:
                WorldRadius = 128;
                fireBowlChank = 24;
                break;
            default:
                WorldRadius = 96;
                fireBowlChank = 20;
                break;
        }

        SEED = _GlobalRunData.SEED;
    }
}
