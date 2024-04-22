using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Knight Attack2 Stab", menuName = "ScriptableObjects/Skills/Active/Knight/Attack2/Stab", order = 2)]
public class KnightAttack2Stab : ActiveSkill
{
    public override void Use(Transform player)
    {
        Animator playerAnimator = player.GetComponentInChildren<Animator>();
        playerAnimator.Play("Knight_Attack2_Stab");
    }
}
