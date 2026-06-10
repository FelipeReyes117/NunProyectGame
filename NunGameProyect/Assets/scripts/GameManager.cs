using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HUD hud;
    private int vidas = 3;

    public weaponData armaGuardada = null;
    public int municionGuardada = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hud = FindAnyObjectByType<HUD>();
        if (hud != null)
            hud.ActualizarVidas(vidas);

        // ✅ Espera un frame antes de restaurar el arma
        StartCoroutine(RestaurarArmaDelayed());
    }

    private IEnumerator RestaurarArmaDelayed()
    {
        yield return null;
    PlayerWeapon pw = FindAnyObjectByType<PlayerWeapon>();
    Debug.Log("PlayerWeapon encontrado: " + (pw != null));
    Debug.Log("armaDefault: " + (pw != null ? pw.armaDefault != null ? pw.armaDefault.name : "null" : "pw es null"));
    if (pw != null)
    {
        if (armaGuardada != null)
            pw.EquiparArma(armaGuardada, municionGuardada);
        else
            pw.EquiparArma(pw.armaDefault, 0);
    }
    }

    public void GuardarArma(weaponData arma, int municion)
    {
        armaGuardada = arma;
        municionGuardada = municion;
    }

    public void PerderVidas()
    {
        vidas -= 1;
        if (vidas <= 0)
        {
            vidas = 3;
            armaGuardada = null;
            municionGuardada = 0;
            SceneManager.LoadScene(1);
            return;
        }
        if (hud != null)
            hud.DesactivarVida(vidas);
    }

    public bool RecuperarVidas()
    {
        if (vidas >= 3) return false;
        vidas += 1;
        if (hud != null)
            hud.ActivarVidas(vidas);
        return true;
    }
}
