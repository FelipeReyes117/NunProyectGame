using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Player;
    public float detectionRadius = 7.0f;
    public float Speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float DistanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (DistanceToPlayer < detectionRadius)
        {
      
            Vector2 direction = (Player.position - transform.position).normalized;
            movement = new Vector2(direction.x, direction.y);
        }
        else
        {
            movement = Vector2.zero;
        }

        rb.MovePosition(rb.position + movement * Speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PerderVidas();
        }
    }
}