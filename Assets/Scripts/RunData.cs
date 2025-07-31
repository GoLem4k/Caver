using System;
using System.ComponentModel;
using UnityEngine;

public class RunData : PausedBehaviour
{
    public static RunData I { get; private set; }
    [SerializeField] public GlobalRunData globalRunData;
    
    [Header("Сид забега")] public int SEED;
    [Header("Точка спавна")] public Vector3Int SPAWNPOINT;
    [Header("Размер мира")] public int WorldRadius;
    [Header("Размер чанка где может заспавнится чаша")] public int fireBowlChank = 16;
    
    
    [Header("Скорость игрока")] public float movementSpeed = 3f;
    [Header("Сила рывка")] public float leapScale = 5f;
    [Header("КД рывка")] public float leapCooldown = 2.5f;
    
    [Header("Урон при отскоке и ударе")] public float damage = 10;
    [Header("КД удара")] public float damageCooldown = 1f;
    [Header("Шанс крита")] public float critChance = 0f;
    
    [Header("Радиус обзора")] public float viewScale = 1f;
    [Header("Радиус ломания")] public float damageRange = 2f;
    
    
    [Header("Запас здоровья")] public float health = 100f;
    [Header("Скорость регенерации")] public float regenerationSpeed = 1f;
    [Header("Опыт в секунду")] public float expPerSecond = 1f;
    [Header("Шанс удвоения эссенции")] public float doubleEssenceChance = 0f;
    [Header("Глобальный множитель опыта")] public float globalExpMultiplier = 1f;
    
    [Header("Возможность критовать")] public bool canCrit = false;
    [Header("Возможность Делать рывок")] public bool canLeap = false;
    [Header("Эффективность эссенций")] public float essenceEfficiency = 1f;
    [Header("Радиус света от чаш с огнём")] public float fireBowlRange = 1f;
    [Header("Указатель на чашу")] public bool fireBowlPointer = false;
    public void Initialize()
    {
        if (I == null) I = this;
        SEED = globalRunData.SEED;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchPause();
    }
}
