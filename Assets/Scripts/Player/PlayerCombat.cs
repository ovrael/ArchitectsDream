using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : Entity
{
    [Header("Hud")]
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private TMP_Text healthText;

    [Header("Buttons")]

    [InspectorButton("DamageFromInspector")]
    [SerializeField]
    private bool shouldTakeDamage;

    protected override void Awake()
    {
        base.Awake();
        GetHealthHud();
        UpdateHud();
    }

    private void GetHealthHud()
    {
        if (healthBar == null)
            healthBar = GameManager.Instance.HudManager.HealthHud.GetComponentInChildren<Image>();
        if (healthText == null)
            healthText = GameManager.Instance.HudManager.HealthHud.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        UpdateHud();
        base.Update();
    }

    protected override void Die()
    {
        Destroy(transform.parent);
    }

    void UpdateHud()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{System.Math.Round(currentHealth, 1),5} / {System.Math.Round(maxHealth, 1),5}";
    }


    private void DamageFromInspector()
    {
        TakeDamage(maxHealth * 0.15f);
    }
}
