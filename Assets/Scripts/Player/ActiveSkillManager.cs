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
    [SerializeField]
    ActiveSkill movement;
    [SerializeReference]
    ActiveSkill utility;

    ActiveSkill[] skills;

    [Header("Hud")]
    [SerializeField]
    GameObject skillsHud;

    // Start is called before the first frame update
    void Awake()
    {
        GetSkillsHud();
        skills = new ActiveSkill[] { attack1, attack2, movement, utility };

        foreach (var skill in skills)
        {
            LoadSkill(skill);
        }
    }

    private void GetSkillsHud()
    {
        if (skillsHud != null) return;

        skillsHud = GameManager.Instance.HudManager.SkillsHud;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack1") && attack1.IsCooldownReady())
        {
            attack1.Use(playerTransform);
            attack1.ClearCooldown();
        }

        if (Input.GetButtonDown("Attack2") && attack2.IsCooldownReady())
        {
            attack2.Use(playerTransform);
            attack2.ClearCooldown();
        }

        if (Input.GetButtonDown("Movement") && movement.IsCooldownReady())
        {
            movement.Use(playerTransform);
            movement.ClearCooldown();
        }

        if (Input.GetButtonDown("Utility") && utility.IsCooldownReady())
        {
            utility.Use(playerTransform);
            utility.ClearCooldown();

        }

        foreach (var skill in skills)
        {
            skill.UpdateCooldown(Time.deltaTime);
            UpdateSkillHud(skill);
        }
    }

    private void UpdateSkillHud(ActiveSkill skill)
    {
        Transform skillCooldown = skillsHud.transform.Find($"{skill.ActiveSkillType}").Find("Cooldown");
        skillCooldown.GetComponent<Image>().fillAmount = skill.GetCooldownRatio();
    }

    private void LoadSkill(ActiveSkill skill)
    {
        Transform skillChild = skillsHud.transform.Find($"{skill.ActiveSkillType}");

        // Update skill icon sprite
        Transform skillIcon = skillChild.Find("Icon");
        skillIcon.GetComponent<Image>().sprite = skill.Sprite;

        // Clear cooldown sprite
        Transform skillCooldown = skillChild.Find("Cooldown");
        skillCooldown.GetComponent<Image>().fillAmount = 0;
    }
}
