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
    GameObject exprienceHud;
    public GameObject ExperienceHud { get => exprienceHud; }

    [SerializeField]
    GameObject skillsHud;
    public GameObject SkillsHud { get => skillsHud; }
}
