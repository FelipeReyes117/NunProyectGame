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
        yield return null; // espera un frame

        PlayerWeapon pw = FindAnyObjectByType<PlayerWeapon>();
        if (pw != null && armaGuardada != null)
            pw.EquiparArma(armaGuardada, municionGuardada);
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
            vidas = 0;
            armaGuardada = null;
            municionGuardada = 0;
            SceneManager.LoadScene(1);
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