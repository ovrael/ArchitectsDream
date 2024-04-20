using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    Transform playerTransform;

    [Header("Skills")]
    [SerializeReference]
    ActiveSkill attack1;
    [SerializeReference]
    ActiveSkill attack2;
    [SerializeReference]
    ActiveSkill movement;
    [SerializeReference]
    ActiveSkill utility;

    ActiveSkill[] skills;

    [Header("Hud")]
    [SerializeField]
    GameObject skillsHUD;

    // Start is called before the first frame update
    void Awake()
    {
        skills = new ActiveSkill[] { attack1, attack2, movement, utility };

        foreach (var skill in skills)
        {
            LoadSkill(skill);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack1"))
        {
            attack1.Use();
        }

        if (Input.GetButtonDown("Attack2"))
        {
            attack2.Use();
        }

        if (Input.GetButtonDown("Movement"))
        {
            movement.UseOnPlayer(playerTransform);
        }

        if (Input.GetButtonDown("Utility"))
        {
            utility.Use();
        }

        foreach (var skill in skills)
        {
            skill.UpdateCooldown(Time.deltaTime);
            UpdateSkillHud(skill);
        }
    }

    private void UpdateSkillHud(ActiveSkill skill)
    {
        Transform skillCooldown = skillsHUD.transform.Find($"{skill.ActiveSkillType}").Find("Cooldown");
        skillCooldown.GetComponent<Image>().fillAmount = skill.GetCooldownRatio();
    }

    private void LoadSkill(ActiveSkill skill)
    {
        Transform skillChild = skillsHUD.transform.Find($"{skill.ActiveSkillType}");

        // Update skill icon sprite
        Transform skillIcon = skillChild.Find("Icon");
        skillIcon.GetComponent<Image>().sprite = skill.Sprite;

        // Clear cooldown sprite
        Transform skillCooldown = skillChild.Find("Cooldown");
        skillCooldown.GetComponent<Image>().fillAmount = 0;
    }
}
