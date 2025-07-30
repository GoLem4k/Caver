using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillLocker : MonoBehaviour
{
    public int dependencies;
    public int maxDependencies;
    public SkillCard[] dependentsSkills;
    public SkillLocker[] dependentsLockers;
    public SwitchableImage[] wires;

    public Sprite unlockSprite;

    public SwitchableImage img;

    private void Start()
    {
        img = GetComponentInParent<SwitchableImage>();
    }

    public void RemoveDependence()
    {
        dependencies--;
        img.SetActiveSprite();
        if (dependencies == 0)
        {
            GetComponent<Image>().sprite = unlockSprite;
            img.SetSuperSprite();
            foreach (var skill in dependentsSkills)
            {
                skill.dependencies--;
                skill.skillIcon.sprite = (skill.dependencies == 0) ? skill.data.imageActive : skill.data.imageInactive;
            }
            foreach (var wire in wires)
            {
                wire.GetComponent<SwitchableImage>().SetActiveSprite();
            }
            foreach (var locker in dependentsLockers)
            {
                locker.RemoveDependence();
            }
        }
    }
}
