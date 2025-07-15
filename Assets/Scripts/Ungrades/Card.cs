using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : PausedBehaviour, IPointerClickHandler
{
    public UpgradeData data;
    private Image image;

    [SerializeField] public bool isActive;

    public static List<Card> AllCards = new(); // список всех карт
    private UIScaler _uiScaler;
    
    public void Initialize()
    {
        GetComponent<Image>().sprite = data.image;
        _uiScaler = GetComponent<UIScaler>();
    }

    private void Start()
    {
        _uiScaler = GetComponent<UIScaler>();
        AllCards.Add(this);
    }

    public override void PausedUpdate()
    {
        
    }
    
    private void DeSelect()
    {
        isActive = false;
        _uiScaler.SimulatePointerExit();
    }

    public void Select()
    {
        foreach (var card in AllCards)
        {
            if (card != this)
            {
                card.DeSelect();
            }
        }

        isActive = true;
        _uiScaler.SimulatePointerEnter();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradesManager.I.MakeChoice(data);
        //if (isActive) UpgradesManager.I.MakeChoice(data);
        //else Select();
    }
}
