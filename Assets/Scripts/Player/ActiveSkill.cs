using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ActiveSkillType
{
    Attack1,
    Attack2,
    Movement,
    Utility
}

public abstract class ActiveSkill : ScriptableObject
{
    [SerializeField]
    private string skillName;
    public string SkillName { get => skillName; private set => skillName = value; }

    [SerializeField]
    private string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; private set => sprite = value; }

    [SerializeField]
    private ActiveSkillType activeSkillType;
    public ActiveSkillType ActiveSkillType { get => activeSkillType; private set => activeSkillType = value; }

    [SerializeField]
    private float cooldown;
    public float Cooldown { get => cooldown; private set => cooldown = value; }

    protected float cooldownTimer;
    public float GetCooldownRatio()
    {
        if (cooldownTimer >= cooldown)
        {
            return 0f;
        }

        return 1 - (cooldownTimer / cooldown);
    }

    protected bool IsCooldownReady()
    {
        return cooldownTimer >= Cooldown;
    }

    public virtual void Use()
    {
        if (!IsCooldownReady())
            return;

        Debug.Log($"Used {SkillName}!");

        cooldownTimer = 0;
    }

    public virtual void UseOnPlayer(Transform player)
    {
        if (!IsCooldownReady())
            return;

        Debug.Log($"Used {SkillName}!");

        cooldownTimer = 0;
    }

    public virtual void UpdateCooldown(float timeAmount)
    {
        cooldownTimer += timeAmount;
    }
}
