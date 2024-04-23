using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField]
    private Image experienceBar;

    private int experience = 0;
    private int level = 1;
    private int experienceNeeded = 200;
    private readonly int levelExperienceChange = 100;

    private void Awake()
    {
        UpdateHud();
    }

    internal void GainExperience(int experienceAmount)
    {
        experience += experienceAmount;
        experienceBar.fillAmount = ((float)experience) / experienceNeeded;
        UpdateHud();

        while (experience >= experienceNeeded)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceNeeded;
        experienceNeeded += levelExperienceChange;

        UpdateHud();

        Debug.Log($"Player leveled up to: {level}!");
    }

    private void UpdateHud()
    {
        experienceBar.fillAmount = ((float)experience) / experienceNeeded;
    }
}
