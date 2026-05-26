using UnityEngine;


public abstract class EnemyAttackBase : MonoBehaviour
{
    [Header("Configuración Base")]
    public float attackRange = 2f;     
    public float attackCooldown = 2f;  

    protected Transform player;
    protected float cooldownTimer;

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }
    }

    // Cada ataque implementa su propia lógica
    protected abstract void Attack();
}