using System.ComponentModel;
using UnityEngine;

public class RunData : MonoBehaviour
{
    public static RunData I { get; private set; }

    [Header("Урон при отскоке")]
    public float reboundDamage = 30f;
    [Header("Скорость игрока")]
    public float movementSpeed = 3f;
    [Header("Сила отскока")]
    public float reboundScale = 5f;
    [Header("Опыт в секунду")]
    public float expPerSecond = 1f;
    [Header("Запас здоровья")]
    public float health = 100f;
    [Header("Скорость регенерации")]
    public float regenerationSpeed = 1f;
    [Header("Количество карточек на выбор")]
    public int choiceAmmount = 3;
    [Header("Возможность критовать")]
    public bool canCrit = false;
    [Header("Шанс крита")]
    public float critChance = 0f;
    [Header("Возможность стрелять")]
    public bool canShoot = false;
    [Header("Урон пуль")]
    public float bulletDamage = 10f;
    [Header("Опыт за поломку блока")]
    public float blockBreakExp = 5f;
    [Header("Возможность размещать блоки")]
    public bool canPlaceBlock = false;
    [Header("Количество перемешиваний")]
    public int refreshCount = 0;
    [Header("Глобальный множитель опыта")]
    public float globalExpMultiplier = 1f;
    
    public void Initialize()
    {
        if (I == null) I = this;
    }
}
