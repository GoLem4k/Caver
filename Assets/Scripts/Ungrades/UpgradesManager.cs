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
        rng = new System.Random(WorldGenerator.SEED);
    }

    /*
    id 1 - урон пуль
    id 2 - шанс крита
    id 3 - урон отскока
    id 4 - сила отскока
    id 5 - скорость движения
    id 6 - прирост опыта в секунду
    id 7 - объём здоровья
    id 8 - регенерация здоровья
    id 101 - возможность стрелять
    id 102 - возможность критовать
    id 201 - возможность строить
    id 202 - глобальный множитель опыта за сломаную руду
    */
    
    public void MakeUpgrade(int id)
    {
        PlayerDataManager.I.RemoveSkillPoint();
        switch (id)
        {
            case 1:
                RunData.I.bulletDamage += 5;
                break;
            case 2:
                RunData.I.critChance += 0.05f;
                break;
            case 3:
                RunData.I.damage += 5f;
                break;
            case 4:
                RunData.I.reboundScale += 0.8f;
                break;
            case 5:
                RunData.I.movementSpeed += 0.3f;
                break;
            case 6:
                RunData.I.expPerSecond += 1f;
                break;
            case 7:
                RunData.I.health += 10f;
                break;
            case 8:
                RunData.I.regenerationSpeed += 0.2f;
                break;
            case 101:
                RunData.I.canShoot = true;
                break;
            case 102:
                RunData.I.canCrit = true;
                break;
            case 202:
                RunData.I.globalExpMultiplier += 0.01f;
                break;
        }
    }
    
}
