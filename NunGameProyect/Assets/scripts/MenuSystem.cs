using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private Animator transitionAnimator;

    void Start()
    {
        if (transitionAnimator == null)
            transitionAnimator = GetComponentInChildren<Animator>();
    }

    public IEnumerator SceneLoad(int sceneIndex)
    {
        if (transitionAnimator != null)
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(transitionTime);
    }
    SceneManager.LoadScene(sceneIndex);
    }

    public void Jugar()
    {
        Debug.Log("Iniciando el juego...");
        StartCoroutine(SceneLoad(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void VolverAlMenu()
    {
        Debug.Log("Volviendo al menú principal...");
        StartCoroutine(SceneLoad(0));
    }

    public void Opciones()
    {
        Debug.Log("Abriendo opciones...");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
