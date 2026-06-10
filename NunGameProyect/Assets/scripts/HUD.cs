using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas;

    public void DesactivarVida(int indice)
    {
        if (indice >= 0 && indice < vidas.Length)
            vidas[indice].SetActive(false);
    }

    public void ActivarVidas(int indice)
    {
        if (indice >= 0 && indice < vidas.Length)
            vidas[indice].SetActive(true);
    }

   
    public void ActualizarVidas(int vidasActuales)
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            if (vidas[i] != null)
                vidas[i].SetActive(i < vidasActuales);
        }
    }
}