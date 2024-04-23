using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Knight Movement Shield Charge", menuName = "ScriptableObjects/Skills/Active/Knight/Movement/Shield Charge", order = 3)]
public class KnightMovementShieldCharge : ActiveSkill
{
    [SerializeField]
    float chargeSpeed = 40f;

    [SerializeField]
    float chargeTime = 4f;

    public override void Use(Transform player)
    {
        Debug.Log($"Used {SkillName} OnPlayer!");

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        PlayerMovement playerMovement = player.GetComponentInChildren<PlayerMovement>();
        playerMovement.ChangeMaxSpeedForTime(chargeSpeed, chargeTime);
        playerRb.velocityX = chargeSpeed * Math.Sign(-player.localScale.x);
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
