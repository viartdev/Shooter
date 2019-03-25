using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    [SerializeField] public List<Text> UIElements;

    public enum Stat
    {
        Health,
        Armor,
        Ammo,
        Weapon
    }

    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        foreach (var UIElement in UIElements)
        {
            switch (UIElement.name)
            {
                case "HealthCount":
                    UIElement.text = "100";
                    break;

                case "ArmorCount":
                    UIElement.text = "0";
                    break;

                case "AmmoCount":
                    UIElement.text = "120";
                    break;
                case "WeaponType":
                    UIElement.text = "USP";
                    break;
            }
        }
    }

    public void ChangeUI(Stat stat, string val)
    {
        switch (stat)
        {
            case Stat.Health:
                UIElements[0].text = val;
                break;
            case Stat.Armor:
                UIElements[1].text = val;
                break;
            case Stat.Ammo:
                UIElements[2].text = val;
                break;
            case Stat.Weapon:
                UIElements[3].text = val;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }
}
