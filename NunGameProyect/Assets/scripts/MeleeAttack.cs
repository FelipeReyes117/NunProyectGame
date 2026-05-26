using UnityEngine;

public class MeleeAttack : EnemyAttackBase
{
    [Header("Melee")]
    public int damage = 1;
    public float knockbackForce = 5f;

    protected override void Attack()
    {
        // Revisa si el player sigue en rango
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, 
                            LayerMask.GetMask("Player"));

        if (hit != null)
        {
            Vector2 knockDir = (hit.transform.position - transform.position).normalized;

            // Llama al método de daño del player (ajusta según tu PlayerController)
            hit.GetComponent<PlayerMovement>()?.TakeDamage(damage, knockDir * knockbackForce);
        }
    }

    // Dibuja el rango en el editor para visualizarlo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}