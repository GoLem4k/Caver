using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CustomButton))]
public class SkillCard : PausedBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeData data;
    public GameObject discWindow;
    public int currentLevel;
    public int dependencies;
    public SkillCard[] dependentsSkills;
    public SkillLocker[] dependentsLockers;
    public SwitchableImage[] wires;

    public Image skillIcon;
    
    public void Start()
    {
        skillIcon = GetComponent<Image>();
        skillIcon.sprite = (dependencies == 0) ? data.imageActive : data.imageInactive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerDataManager.I.skillPoints > 0 && dependencies == 0 && currentLevel < data.maxLevel)
        {
            UpgradesManager.I.MakeUpgrade(data.id);
            currentLevel++;
            if (discWindow != null)
            {
                discWindow.GetComponent<TextMeshProUGUI>().text = GetDiscription();
                discWindow.SetActive(true);
            }
            if (currentLevel == 1)
            {
                GetComponentInParent<SwitchableImage>().SetActiveSprite();
                foreach (var skill in dependentsSkills)
                {
                    skill.dependencies--;
                    skill.skillIcon.sprite = (skill.dependencies == 0) ? skill.data.imageActive : skill.data.imageInactive;
                }

                foreach (var locker in dependentsLockers)
                {
                    locker.RemoveDependence();
                }

                foreach (var wire in wires)
                {
                    wire.GetComponent<SwitchableImage>().SetActiveSprite();
                }
            }

            if (currentLevel == data.maxLevel)
            {
                GetComponentInParent<SwitchableImage>().SetSuperSprite();
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
        skillIcon.sprite = data.imageActive;
    }
}