using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Camera mainCamera;
    public Transform spawner;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private Vector2 lastDirection = Vector2.right;

    [Header("Sistema de Armas")]
    public weaponData armaDefault;
    public weaponData armaActual;
    public int municionRestante;

    private float nextFireTime = 0f;
    private bool isFiring = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponentInParent<PlayerMovement>();
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
            spriteRenderer.sprite = nuevaArma.spriteArmas;
    }

    public void OnFireButtonDown() { isFiring = true; }
    public void OnFireButtonUp() { isFiring = false; }

    private void CheckFiring()
    {
        bool mousePress = Mouse.current != null && Mouse.current.leftButton.isPressed;
        if ((mousePress || isFiring) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + armaActual.cadenciaFuego;
        }
    }

    private void Shoot()
    {
        if (audioSource != null)
            audioSource.Play();

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
                EquiparArma(armaDefault, 0);
        }
    }

    public void RotateTowardsMouse()
    {
        float angle;

        #if UNITY_ANDROID || UNITY_IOS
            if (playerMovement != null)
            {
                Vector2 dir = playerMovement.GetMoveDirection();
                if (dir != Vector2.zero)
                    lastDirection = dir;
            }
            angle = (Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg + 360) % 360;
        #else
            angle = GetAngleTowardsMouse();
        #endif

        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (spriteRenderer != null)
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
