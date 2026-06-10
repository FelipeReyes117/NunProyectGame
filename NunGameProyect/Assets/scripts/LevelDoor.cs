using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelDoor : MonoBehaviour
{
    [Header("Siguiente Nivel")]
    public string siguienteEscena = "Level_2";

    [Header("Transición")]
    public float duracionTransicion = 2f;
    public GameObject pantallaTransicion;
    public TextMeshProUGUI textoNivel;

    private bool puertaAbierta = false;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr  = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        CerrarPuerta();

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.OnTodosEnemigosEliminados += AbrirPuerta;

        if (pantallaTransicion != null)
            pantallaTransicion.SetActive(false);
    }

    private void CerrarPuerta()
    {
        puertaAbierta = false;
        if (sr  != null) sr.enabled   = true;
        if (col != null)
        {
            col.enabled   = true;
            col.isTrigger = false; 
        }
    }

    private void AbrirPuerta()
    {
        puertaAbierta = true;
        if (sr  != null) sr.enabled = false;

        
        if (col != null)
        {
            col.enabled   = true;       
            col.isTrigger = true;       
        }
        Debug.Log("Puerta abierta!");
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && puertaAbierta)
            StartCoroutine(TransicionNivel());
    }

    private IEnumerator TransicionNivel()
    {
        if (pantallaTransicion != null)
            pantallaTransicion.SetActive(true);

        if (textoNivel != null)
            textoNivel.text = siguienteEscena.Replace("_", " ");

        yield return new WaitForSeconds(duracionTransicion);
        SceneManager.LoadScene(siguienteEscena);
    }
}