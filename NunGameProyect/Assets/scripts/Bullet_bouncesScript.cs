using UnityEngine;

public class Bullet_bouncesScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 10f;
    public int damage = 1;

    [Header("Mecánica de Rebote")]
    public int maxBounces = 2;
    private int currentBounces = 0;
    private Vector2 moveDirection;

    [Header("Partículas")]
    public GameObject hitEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = transform.right;
        rb.linearVelocity = moveDirection * speed;

      
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D bulletCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (bulletCollider != null && playerCollider != null)
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Bullet") ||
            collision.gameObject.CompareTag("Gun") ||
            collision.gameObject.CompareTag("heard") ||
            collision.gameObject.CompareTag("Items") ||
            collision.gameObject.CompareTag("CameraBounds"))
        {
            return;
        }

        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.TakeDamage(damage, moveDirection);
        }

      
        ValidarRebote(collision);
    }

    private void ValidarRebote(Collision2D collision)
    {
        SpawnHitEffect();

    if (currentBounces < maxBounces)
    {
        currentBounces++;

        Vector2 wallNormal = collision.contacts[0].normal;
        moveDirection = Vector2.Reflect(moveDirection, wallNormal).normalized;

        // ✅ Empuja la bala fuera de la pared antes de rebotar
        transform.position += (Vector3)(wallNormal * 0.1f);

        rb.linearVelocity = Vector2.zero; // ✅ resetea velocidad primero
        rb.linearVelocity = moveDirection * speed;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    else
    {
        Destroy(gameObject);
    }
    }

    private void SpawnHitEffect()
    {
        if (hitEffect == null) return;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
    }
}