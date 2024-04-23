using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Entities
{
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
}
