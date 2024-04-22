using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Knight Attack1 Sweep", menuName = "ScriptableObjects/Skills/Active/Knight/Attack1/Sweep", order = 1)]
public class KnightAttack1Sweep : ActiveSkill
{
    public override void Use(Transform player)
    {
        Animator playerAnimator = player.GetComponentInChildren<Animator>();
        playerAnimator.Play("Knight_Attack1_Sweep");
    }
}
