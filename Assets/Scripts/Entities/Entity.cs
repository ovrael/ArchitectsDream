using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth;
    protected float currentHealth;

    [SerializeField]
    protected float healthRegeneraion;

    [SerializeField]
    protected float armor;

    protected virtual void Awake()
    {
        Debug.Log($"Set {gameObject.name} curr:{currentHealth} to max:{maxHealth}");
        currentHealth = maxHealth;
        Debug.Log($"After set {gameObject.name} curr:{currentHealth} to max:{maxHealth}");
    }

    protected virtual void Update()
    {
        RegenerateHealth();
    }

    protected void RegenerateHealth()
    {
        if (currentHealth > maxHealth) return;

        currentHealth += healthRegeneraion * Time.deltaTime;
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
    }


    protected void CheckDeath()
    {
        if (currentHealth <= 0)
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

        currentHealth -= (damage - armor);
        CheckDeath();
    }
}