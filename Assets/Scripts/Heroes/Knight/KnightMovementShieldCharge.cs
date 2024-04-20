using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Knight Movement Shield Charge", menuName = "ScriptableObjects/Skills/Active/Knight/Movement/Shield Charge", order = 3)]
public class KnightMovementShieldCharge : ActiveSkill
{
    public override void UseOnPlayer(Transform player)
    {
        if (!IsCooldownReady())
            return;

        Debug.Log($"Used {SkillName} OnPlayer!");

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.velocityX = 50f * Math.Sign(-player.localScale.x);

        cooldownTimer = 0;


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
