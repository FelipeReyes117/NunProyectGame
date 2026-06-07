using UnityEngine;
using System.Collections;

public class SpeedBoostItem : MonoBehaviour
{
    [Header("configuracion de velocidad")]
    public float speedMultiplier = 1.8f; // Cuanto se aumenta la velocidad 
    public float duration = 5f; // Duración del efecto de velocidad
    public GameObject particleEffect; //Efecto de velocidad nose xd
    

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player= other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ApplySpeedBoost(speedMultiplier, duration, particleEffect);
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
