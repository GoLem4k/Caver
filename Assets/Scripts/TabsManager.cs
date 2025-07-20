using System;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    public GameObject[] tabs;
    public Image[] tabsButtons;
    public Sprite[] inactiveButton, activeButton;
    public GameObject upgradeMenu;

    public void SwitchTabs(int tabID)
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[tabID].SetActive(true);

        int i = 0;
        foreach (var image in tabsButtons)
        {
            image.sprite = inactiveButton[i++];
        }
        tabsButtons[tabID].sprite = activeButton[tabID];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        }
    }

    public void CloseMenu()
    {
        upgradeMenu.SetActive(false);
    }
}
