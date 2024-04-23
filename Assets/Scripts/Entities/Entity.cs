using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssemblyCSharp.Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        private float health;

        [SerializeField]
        private float armor;

        protected void CheckDeath()
        {
            if (health <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Add damage type?
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            if (armor >= damage)
                return;

            health -= (damage - armor);
            CheckDeath();
        }
    }
}
