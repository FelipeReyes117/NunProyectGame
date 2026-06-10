using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }

    [Header("Comportamiento")]
    public EnemyType enemyType = EnemyType.Melee;
    public float preferredDistance = 4f;
    public float distanceTolerance = 0.5f;

    private Transform player;
    public float detectionRadius = 7.0f;
    public float Speed = 2.0f;

    [Header("Estadísticas")]
    public int health = 2;
    public float knockbackForce = 10f;

    [System.Serializable]
    public class DropEntry
    {
        public string nombre;
        public GameObject prefab;
        [Range(0, 100)]
        public float chance;
    }

    [Header("Drops (Recompensas)")]
    public DropEntry[] drops;

    // ── Partículas ────────────────────────────────────────────────
    [Header("Partículas")]
    public GameObject spawnParticle;
    public GameObject hitParticle;
    public GameObject deathParticle;
    // ─────────────────────────────────────────────────────────────

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isKnockedBack;
    private float knockbackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"[{gameObject.name}] No se encontró un objeto con Tag 'Player'.");

        // ✅ Partícula de spawn
        SpawnParticle(spawnParticle);
    }

    void Update()
    {
        if (player == null) return;

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) isKnockedBack = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            switch (enemyType)
            {
                case EnemyType.Melee:  MoveMelee(distanceToPlayer);  break;
                case EnemyType.Ranged: MoveRanged(distanceToPlayer); break;
            }
        }
        else
        {
            movement = Vector2.zero;
        }

        rb.linearVelocity = movement * Speed;
    }

    private void MoveMelee(float distanceToPlayer)
    {
        movement = (player.position - transform.position).normalized;
    }

    private void MoveRanged(float distanceToPlayer)
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (distanceToPlayer > preferredDistance + distanceTolerance)
            movement = direction;
        else if (distanceToPlayer < preferredDistance - distanceTolerance)
            movement = -direction;
        else
            movement = Vector2.zero;
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        isKnockedBack = true;
        knockbackTimer = 0.2f;

        
        SpawnParticle(hitParticle);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0) Die();
    }

    private void Die()
    {
        
        SpawnParticle(deathParticle);
        TryDrop();
        Destroy(gameObject);
    }

    private void TryDrop()
    {
        foreach (DropEntry drop in drops)
        {
            if (drop.prefab == null) continue;
            float roll = Random.Range(0f, 100f);
            if (roll <= drop.chance)
            {
                Instantiate(drop.prefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }

    
    private void SpawnParticle(GameObject particlePrefab)
    {
        if (particlePrefab == null) return;
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Player"))
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Dirección del empuje = desde el enemigo hacia el player
            Vector2 knockDir = (other.transform.position - transform.position).normalized;
            player.TakeDamage(1, knockDir * knockbackForce);
        }
    }
    }
}