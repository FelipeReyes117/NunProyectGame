using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DesactivarVida(int indice)
    {
        vidas[indice].SetActive(false); 
    }
    public void ActivarVidas(int indice)
    {
        vidas[(int)indice].SetActive(true);
    }
}
