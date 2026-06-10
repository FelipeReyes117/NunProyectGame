using UnityEngine;

public class VIda : MonoBehaviour
{
    [Header("Ajustes de Despawn")]
    public float tiempoDeVida = 5f;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            bool vidaRecuperada = GameManager.instance.RecuperarVidas();
            if (vidaRecuperada)
                Destroy(gameObject);
        }
    }
}