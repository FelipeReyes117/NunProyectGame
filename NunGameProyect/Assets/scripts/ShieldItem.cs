using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    public GameObject shieldEffect; // Prefab of the shield to be instantiated
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ApplyShield(shieldEffect);
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
