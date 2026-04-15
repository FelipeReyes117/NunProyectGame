using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 3;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // 👇 Ignora la colisión entre la bala y el jugador
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
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player")) // 👈 Por si acaso, ignora al jugador
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rigidbody.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
    }
}