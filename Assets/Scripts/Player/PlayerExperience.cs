using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField]
    private Image experienceBar;

    [SerializeField]
    private TMP_Text levelText;

    private int experience = 0;
    private int level = 1;
    private int experienceNeeded = 200;
    private readonly int levelExperienceChange = 100;

    private void Awake()
    {
        UpdateHud();
    }

    private void Start()
    {
        GetExperienceHud();
    }

    private void GetExperienceHud()
    {
        if (experienceBar == null)
            experienceBar = GameManager.Instance.HudManager.ExperienceHud.GetComponentInChildren<Image>();

        if (levelText == null)
            levelText = GameManager.Instance.HudManager.ExperienceHud.GetComponentInChildren<TMP_Text>();
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
        levelText.text = $"{level}";
    }

    private void UpdateHud()
    {
        experienceBar.fillAmount = ((float)experience) / experienceNeeded;
    }
}
