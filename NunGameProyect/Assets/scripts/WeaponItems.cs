using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    public weaponData armaAEntregar; 
    public int cantidadMunicion = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("Player"))
        {
            // Buscamos el script del arma en el jugador o sus hijos
            PlayerWeapon pw = other.GetComponentInChildren<PlayerWeapon>();
            
            if (pw != null)
            {
                pw.EquiparArma(armaAEntregar, cantidadMunicion);
                Destroy(gameObject); // El item del suelo desaparece
            }
        }

    }
}
