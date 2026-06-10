using UnityEngine;

public class TreasureChest : MonoBehaviour
{
     [Header("Sprites")]
    public Sprite spriteAbierto;   // sprite del cofre abierto
    public Sprite spriteCerrado;   // sprite del cofre cerrado

    [Header("Recompensa")]
    public weaponData armaRecompensa;  // arma que otorga
    public int municionRecompensa = 10;

    private SpriteRenderer sr;
    private bool estaAbierto = false;
    private bool armaEntregada = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && spriteCerrado != null)
            sr.sprite = spriteCerrado;

       
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.OnTodosEnemigosEliminados += AbrirCofre;
    }

    private void AbrirCofre()
    {
        estaAbierto = true;
        if (sr != null && spriteAbierto != null)
            sr.sprite = spriteAbierto; // ✅ cambia sprite a abierto
        Debug.Log("Cofre abierto!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player") && estaAbierto && !armaEntregada)
        {
            armaEntregada = true;

            PlayerWeapon pw = other.GetComponentInParent<PlayerWeapon>();
            if (pw == null)
                pw = other.GetComponentInChildren<PlayerWeapon>();

            if (pw != null && armaRecompensa != null)
                pw.EquiparArma(armaRecompensa, municionRecompensa);

            Debug.Log("Arma entregada!");
        }
    }
}
