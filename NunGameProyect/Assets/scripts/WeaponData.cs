using UnityEngine;

[CreateAssetMenu(fileName = "NuevaArma", menuName = "Proyecto Nun/Arma")]
public class weaponData : ScriptableObject
{
    public string nombreArma;
    public Sprite spriteArmas; // agrega esto 
    public GameObject prefabBala;
    public float cadenciaFuego = 0.5f;
    
    [Header("Ajustes de Multidisparo")]
    public int numeroDeBalas = 1;
    public float dispersion = 0f; // Asegúrate de que tenga la 'r'
    
    [Header("Munición")]
    public bool esInfinita = false; 
}