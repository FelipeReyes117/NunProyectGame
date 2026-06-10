using UnityEngine;

public class BulletEnemyScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3f;
    public int damage = 1;

    private Collider2D bulletCollider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();

        // Ignora colisión con todos los enemigos
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (EnemyController enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (bulletCollider != null && enemyCollider != null)
                Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
        }
    }

    // ✅ Método para ignorar al enemigo que disparó
    public void SetOwner(Collider2D ownerCollider)
    {
        if (bulletCollider != null && ownerCollider != null)
            Physics2D.IgnoreCollision(bulletCollider, ownerCollider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Vector2 knockDir = transform.right;
                player.TakeDamage(damage, knockDir * 5f);
            }
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Enemy") &&
                 !collision.CompareTag("Bullet") &&
                 !collision.CompareTag("Gun") &&
                 !collision.CompareTag("heard") &&
                 !collision.CompareTag("Items") &&
                 !collision.CompareTag("CameraBounds"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rigidbody.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
    }
}