using UnityEngine;

public class VIda : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool VidaRecuperada = GameManager.instance.RecuperarVIdas();
            if (VidaRecuperada)
            {
                Destroy(gameObject);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
