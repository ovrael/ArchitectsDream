using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    GameObject healthHud;
    public GameObject HealthHud { get => healthHud; }

    [SerializeField]
    Image experienceBar;
    public Image ExperienceBar { get => experienceBar; }

    [SerializeField]
    GameObject skillsHud;
    public GameObject SkillsHud { get => skillsHud; }
}
