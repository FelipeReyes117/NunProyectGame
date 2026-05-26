using UnityEngine;

public class BulletEnemyScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3f;
    public int damage = 1;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // Ignora colisión con todos los enemigos para que no se choquen entre sí
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        Collider2D bulletCollider = GetComponent<Collider2D>();

        foreach (EnemyController enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (bulletCollider != null && enemyCollider != null)
                Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Knockback en la misma dirección que va la bala
                Vector2 knockDir = transform.right;
                player.TakeDamage(damage, knockDir * 5f);
            }

            Destroy(gameObject);
        }
        // Se destruye con paredes u otros objetos que no sean enemigos ni la bala misma
        else if (!collision.CompareTag("Enemy") && 
                 !collision.CompareTag("Bullet") && 
                 !collision.CompareTag("Gun") && 
                 !collision.CompareTag("heard"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rigidbody.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
    }
}