using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3;
    public int damage = 1; // Cuánto daño hace cada bala

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
            // Buscamos el script del enemigo
            EnemyController enemy = collision.GetComponent<EnemyController>();
            
            if (enemy != null)
            {
                // Calculamos la dirección del empuje (la misma dirección que lleva la bala)
                Vector2 direction = transform.right; 
                enemy.TakeDamage(damage, direction);
            }

            Destroy(gameObject); // La bala siempre se destruye al chocar
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet")&& !collision.CompareTag("Gun")&& !collision.CompareTag("heard"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Nota: He cambiado fixedDeltaTime por deltaTime porque esto corre en Update
        rigidbody.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
    }
}