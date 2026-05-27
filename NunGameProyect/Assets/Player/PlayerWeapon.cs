using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Camera mainCamera; 
    public Transform spawner;

    [Header("Sistema de Armas")]
    public weaponData armaDefault; 
    public weaponData armaActual;  
    public int municionRestante;

    private float nextFireTime = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Empezamos siempre con la pistola infinita
        EquiparArma(armaDefault, 0); 
    }

    void Update()
    {
        RotateTowardsMouse();
        CheckFiring();
    }

    public void EquiparArma(weaponData nuevaArma, int municion)
    {
        armaActual = nuevaArma;
        municionRestante = municion;
       if (nuevaArma != null && spriteRenderer != null && nuevaArma.spriteArmas != null)
        {
            spriteRenderer.sprite = nuevaArma.spriteArmas;
        }
    }

    private void CheckFiring() 
    {
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime) 
        {
            Shoot();
            nextFireTime = Time.time + armaActual.cadenciaFuego;
        }
    }

    private void Shoot()
    {
        float anguloInicial = -(armaActual.dispersion * (armaActual.numeroDeBalas - 1)) / 2f;

        for (int i = 0; i < armaActual.numeroDeBalas; i++)
        {
            float desviacion = anguloInicial + (i * armaActual.dispersion);
            
            GameObject bullet = Instantiate(armaActual.prefabBala); 
            bullet.transform.position = spawner.position;
            bullet.transform.rotation = spawner.rotation;
            bullet.transform.Rotate(0, 0, desviacion); 
            
            Destroy(bullet, 2f);
        }

        if (!armaActual.esInfinita)
        {
            municionRestante--;
            if (municionRestante <= 0)
            {
                EquiparArma(armaDefault, 0); 
            }
        }
    }

    // --- TUS FUNCIONES DE ROTACIÓN RECONSTRUIDAS ---

    public void RotateTowardsMouse()
    {
        float angle = GetAngleTowardsMouse();
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        if(spriteRenderer != null)
            spriteRenderer.flipY = angle >= 90 && angle <= 270;
    }

    public float GetAngleTowardsMouse()
    {
        if (Mouse.current == null) return 0f;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseScreen = new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreen);

        Vector3 mouseDirection = mouseWorldPosition - transform.position;
        mouseDirection.z = 0;

        float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360) % 360;
        return angle;
    }
}