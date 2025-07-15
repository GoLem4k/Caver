using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradesManager : PausedBehaviour
{
    public static UpgradesManager I { get; private set; }

    [SerializeField] private List<UpgradeData> _upTotal;
    [SerializeField] private List<GameObject> cardObjInChoice;
    private Dictionary<int, int> cardCounter;
    
    public GameObject upMenu;
    public GameObject upGroup;
    public GameObject upCard;

    public int choiceAmmountBase = 5;
    
    public System.Random rng;
    
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

        if (choiceAmmount > choiceAmmountBase) choiceAmmount = choiceAmmountBase;
        
        while (cardInChoiceCount < choiceAmmount && safeCounter < 100)
        {
            safeCounter++;

            UpgradeData newCard = _upTotal[rng.Next(_upTotal.Count)];

            if (cardsInChoiceData.Contains(newCard) || cardCounter[newCard.id] <= 0)
                continue;

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


    public void MakeChoice(UpgradeData up)
    {
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
    }
    
}
