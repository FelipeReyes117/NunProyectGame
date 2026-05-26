using UnityEngine;
using System.Collections; 

public class EnemyController : MonoBehaviour 
{
   
    private Transform player;

    public float detectionRadius = 7.0f;
    public float Speed = 2.0f;

    [Header("Estadísticas")]
    public int health = 2;
    public float knockbackForce = 10f;

    // ── Sistema de drops ──────────────────────────────────────────
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
    // ─────────────────────────────────────────────────────────────

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isKnockedBack;
    private float knockbackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Auto-detección del Player por Tag
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"[{gameObject.name}] No se encontró un objeto con Tag 'Player'.");
    }

    void Update()
    {
        if (player == null) return; // Seguridad por si no se encontró el player

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) isKnockedBack = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }

        rb.linearVelocity = movement * Speed;
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        isKnockedBack = true;
        knockbackTimer = 0.2f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0) Die();
    }

    private void Die()
    {
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
                return; // Solo un drop por muerte
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PerderVidas();
        }
    }
}