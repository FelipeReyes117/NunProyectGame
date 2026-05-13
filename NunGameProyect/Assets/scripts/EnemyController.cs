using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public float detectionRadius = 7.0f;
    public float Speed = 2.0f;

    [Header("Estadísticas")]
    public int health = 2;
    public float knockbackForce = 10f; 

    [Header("Drops (Recompensas)")]
    public GameObject lifePrefab;
    [Range(0, 100)] 
    public float dropChance = 30f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isKnockedBack;
    private float knockbackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) isKnockedBack = false;
            return;
        }

        float DistanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (DistanceToPlayer < detectionRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
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

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        float aleatorio = Random.Range(0f, 100f);
        
        if (aleatorio <= dropChance)
        {
            Instantiate(lifePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PerderVidas();
        }
    }
}