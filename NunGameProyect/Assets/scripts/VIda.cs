using UnityEngine;

public class VIda : MonoBehaviour
{
    [Header("Ajustes de Despawn")]
    public float tiempoDeVida = 5f; // Variable cambiante desde el Inspector

    void Start()
    {
        // Esto le dice a Unity: "Destruye este objeto cuando pasen X segundos"
        // Se programa desde que el objeto nace (Start)
        Destroy(gameObject, tiempoDeVida);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool VidaRecuperada = GameManager.instance.RecuperarVIdas();
            if (VidaRecuperada)
            {
                Destroy(gameObject);
            }
        }
    }
}