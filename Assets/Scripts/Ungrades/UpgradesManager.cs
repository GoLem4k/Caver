using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using Debug = UnityEngine.Debug;

public class UpgradesManager : PausedBehaviour
{
    public static UpgradesManager I { get; private set; }

    [SerializeField] private List<UpgradeData> _upTotal;
    [SerializeField] private List<GameObject> cardObjInChoice;
    [SerializeField] private List<UpgradeData> _playerUpgrades;
    private Dictionary<int, int> cardCounter;
    
    public GameObject upMenu;
    public GameObject upGroup;
    public GameObject upCard;

    public int choiceAmmountBase = 5;
    
    private System.Random rng;
    
    public void Initialize()
    {
        if (I == null) I = this;
        rng = new System.Random(WorldGenerator.SEED);
        upMenu.SetActive(false);
        cardCounter = new Dictionary<int, int>();
        foreach (var data in _upTotal)
        {
            cardCounter.Add(data.id, data.totalCount);
        }
    }

    public void UpgradeChoice()
    {
        int cardInChoiceCount = 0;
        List<UpgradeData> cardsInChoiceData = new();
        int safeCounter = 0; // защита от бесконечного цикла

        int choiceAmmount = cardCounter.Count(pair => pair.Value > 0);
        if (choiceAmmount == 0)
        {
            PAUSE = false;
            return;
        }

        if (choiceAmmount > RunData.I.choiceAmmount) choiceAmmount = RunData.I.choiceAmmount;
        
        while (cardInChoiceCount < choiceAmmount && safeCounter < 250)
        {
            safeCounter++;

            UpgradeData newCard = _upTotal[rng.Next(_upTotal.Count)];

            if (cardsInChoiceData.Contains(newCard) || cardCounter[newCard.id] <= 0)
                continue;
            
            if (newCard.type == UpgradeType.Support && rng.NextDouble() < 0.75f) continue;
            if (newCard.type == UpgradeType.Active && rng.NextDouble() < 0.25f) continue;
            if (newCard.type == UpgradeType.Magic && rng.NextDouble() < 0.40f) continue;
            
            if (!_playerUpgrades.Any(x => x.id == newCard.needId) && !(newCard.needId == 0)) continue;

            cardsInChoiceData.Add(newCard); // сохраняем, чтобы не повторялись

            GameObject cardObj = Instantiate(upCard, upGroup.transform);
            Card cardComp = cardObj.GetComponent<Card>();
            cardComp.data = newCard;
            cardComp.Initialize();
            cardObjInChoice.Add(cardObj);

            cardInChoiceCount++;
        }

        upMenu.SetActive(true);
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
    
    public void MakeChoice(UpgradeData up)
    {
        _playerUpgrades.Add(up);
        Debug.Log(up);
        upMenu.SetActive(false);
        PAUSE = false;
        foreach (var upgrade in cardCounter)
        {
            if (upgrade.Key == up.id)
            {
                cardCounter[upgrade.Key] -= 1;
                break; // Если id уникальны, выходим из цикла
            }
        }

        foreach (var obj in cardObjInChoice)
        {
            Destroy(obj);
        }

        switch (up.id)
        {
            case 1:
                RunData.I.bulletDamage += 5;
                break;
            case 2:
                RunData.I.critChance += 0.05f;
                break;
            case 3:
                RunData.I.reboundDamage += 5f;
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
            case 201:
                RunData.I.canPlaceBlock = true;
                break;
            case 202:
                RunData.I.globalExpMultiplier += 0.01f;
                break;
        }
    }
    
}
