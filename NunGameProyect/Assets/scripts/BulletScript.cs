using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3;
    public int damage = 1;

    [Header("Partículas")]
    public GameObject hitEffect; // ✅ arrastra el prefab aquí

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D bulletCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (bulletCollider != null && playerCollider != null)
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 direction = transform.right;
                enemy.TakeDamage(damage, direction);
            }
            SpawnHitEffect();
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player") &&
                 !collision.CompareTag("Bullet") &&
                 !collision.CompareTag("Gun") &&
                 !collision.CompareTag("heard") &&
                 !collision.CompareTag("CameraBounds"))
        {
            SpawnHitEffect(); // ✅ también al chocar con muros
            Destroy(gameObject);
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffect == null) return;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
    }

    void Update()
    {
        rigidbody.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
    }
}