using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HUD hud;
    private int vidas = 3;

    void Awake()
    {
        instance = this; // 👈 Y esto también
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerderVidas()
    {
        vidas -= 1;

        if (vidas == 0)
        {
            //Reinicio de nivel
            SceneManager.LoadScene(1);
        }
        hud.DesactivarVida(vidas);

    }

    public bool RecuperarVIdas()
    {
        if (vidas > 3)
        {
            return false;
        }
        hud.ActivarVidas(vidas);
        vidas += 1;
        return true;
        
    }
}
