using UnityEngine;
using System.Collections;

public class ChargeAttack : EnemyAttackBase
{
    [Header("Charge")]
    public float chargeSpeed = 12f;
    public float chargeDuration = 0.4f;
    public int damage = 1;

    private Rigidbody2D rb;
    private bool isCharging = false;
    private EnemyController enemyController;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
    }

    protected override void Attack()
    {
        if (!isCharging)
            StartCoroutine(DoCharge());
    }

    private IEnumerator DoCharge()
    {
        isCharging = true;
        if (enemyController != null) enemyController.enabled = false;

        Vector2 chargeDirection = (player.position - transform.position).normalized;

        float timer = 0f;
        while (timer < chargeDuration)
        {
            
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.5f, 
                                LayerMask.GetMask("Player"));
            if (hit != null)
            {
                PlayerMovement playerMovement = hit.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                    playerMovement.TakeDamage(damage, chargeDirection * 6f);
                
                break; 
            }

            rb.linearVelocity = chargeDirection * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        if (enemyController != null) enemyController.enabled = true;
        isCharging = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}