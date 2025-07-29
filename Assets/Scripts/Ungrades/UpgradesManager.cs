using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using Debug = UnityEngine.Debug;

public class UpgradesManager : PausedBehaviour
{
    public static UpgradesManager I { get; private set; }
    
    private System.Random rng;
    
    public void Initialize()
    {
        if (I == null) I = this;
        rng = new System.Random(RunData.I.SEED);
    }

    /*
    id 1 - скорость
    id 2 - сила рывка
    id 3 - кд рывка
    id 4 - урон
    id 5 - кд урона
    id 6 - крит шанс
    id 7 - радиус обзора
    id 8 - радиус ломания блоков
    id 17 - макс хп
    id 18 - хп реген
    id 19 - exp per sec
    id 20 - шанс удвоить ессенцию
    id 21 - глобальный множитель опыта
    id 33 - крит анлок
    id 34 - рывок анлок
    id 35 - эффективность сжигания эссенции
    id 36 - максимальный радиус световых конусов
    id 37 - указатель на ближайшую чашу анлок
    */
    
    public void MakeUpgrade(int id)
    {
        PlayerDataManager.I.RemoveSkillPoint();
        switch (id)
        {
            case 1:
                RunData.I.movementSpeed += 0.3f;
                break;
            case 2:
                RunData.I.leapScale += 0.8f;
                break;
            case 3:
                RunData.I.leapCooldown -= 0.2f;
                break;
            case 4:
                RunData.I.damage += 4f;
                break;
            case 5:
                RunData.I.damageCooldown -= 0.2f;
                break;
            case 6:
                RunData.I.critChance += 0.05f;
                break;
            case 7:
                RunData.I.viewScale += 0.2f;
                break;
            case 8:
                RunData.I.damageRange += 1f;
                break;
            
            case 17:
                RunData.I.health += 50f;
                break;
            case 18:
                RunData.I.regenerationSpeed += 1f;
                break;
            case 19:
                RunData.I.expPerSecond += 1f;
                break;
            case 20:
                RunData.I.doubleEssenceChance += 5f;
                break;
            case 21:
                RunData.I.globalExpMultiplier += 0.1f;
                break;
            
            case 33:
                RunData.I.canCrit = true;
                RunData.I.critChance += 0.05f;
                break;
            case 34:
                RunData.I.canLeap = true;
                break;
            case 35:
                RunData.I.essenceEfficiency += 0.2f;
                break;
            case 36:
                RunData.I.fireBowlRange += 0.25f;
                break;
            case 37:
                RunData.I.fireBowlPointer = true;
                break;

        }
    }
    
}
