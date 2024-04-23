using AssemblyCSharp.Assets.Scripts.Entities;

using UnityEngine;

public class Knight_Sword : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision == null)
            return;

        if (!collision.CompareTag("Enemy"))
            return;

        AttackEnemy(collision.gameObject);
    }

    private void AttackEnemy(GameObject enemyObject)
    {
        Entity enemy = enemyObject.GetComponent<Entity>();
        enemy.TakeDamage(damage);
    }
}
