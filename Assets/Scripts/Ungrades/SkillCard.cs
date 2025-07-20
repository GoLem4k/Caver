using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillCard : PausedBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeData data;
    public GameObject discWindow;
    public int currentLevel;
    public bool dependence;
    public SkillCard[] dependences;
    public GameObject[] wires;
    
    public void Initialize()
    {
        GetComponent<Image>().sprite = data.imageInactive;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerDataManager.I.skillPoints > 0 && dependence == false)
        {
            UpgradesManager.I.MakeUpgrade(data.id);
            currentLevel++;
            PlayerDataManager.I.skillPoints--;
            if (discWindow != null)
            {
                discWindow.GetComponent<TextMeshProUGUI>().text = GetDiscription();
                discWindow.SetActive(true);
            }
        }

        if (currentLevel == 1)
        {
            SetACtiveSprite();
            foreach (var skill in dependences)
            {
                skill.dependence = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (discWindow != null)
        {
            discWindow.GetComponent<TextMeshProUGUI>().text = GetDiscription();
            discWindow.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (discWindow != null)
        {
            discWindow.SetActive(false);
        }
    }

    public string GetDiscription()
    {
        string disc = data.discription;
        disc += $"\n{currentLevel}/{data.maxLevel}";
        return disc;
    }

    public void SetACtiveSprite()
    {
        GetComponent<Image>().sprite = data.imageActive;
    }
}