using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class EndGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject pantallaFinal;        // panel negro
    public TextMeshProUGUI textoFinal;      // texto del mensaje
    public string mensaje = "¡Has terminado la Demo del juego!";

    [Header("Configuración")]
    public float tiempoEspera = 7f;         // segundos antes de reiniciar

    void Start()
    {
        if (pantallaFinal != null)
            pantallaFinal.SetActive(false);

      
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.OnTodosEnemigosEliminados += MostrarPantallaFinal;
        else
            Debug.LogWarning("No se encontró EnemySpawner!");
    }

    private void MostrarPantallaFinal()
    {
        StartCoroutine(FinDelJuego());
    }

    private IEnumerator FinDelJuego()
    {
        // ✅ Muestra pantalla negra con mensaje
        if (pantallaFinal != null)
            pantallaFinal.SetActive(true);

        if (textoFinal != null)
            textoFinal.text = mensaje;

       
        yield return new WaitForSeconds(tiempoEspera);

    
        if (GameManager.instance != null)
        {
            GameManager.instance.GuardarArma(null, 0);
        }

      
        SceneManager.LoadScene("MainMenu");
    }
}