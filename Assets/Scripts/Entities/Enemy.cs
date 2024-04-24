using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    private int experience = 30;

    protected override void Die()
    {
        GiveExperienceToPlayer();
        base.Die();
    }

    private void GiveExperienceToPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponentInChildren<PlayerExperience>().GainExperience(experience);
    }
}