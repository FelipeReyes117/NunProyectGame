using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Camera Camera;
    public Transform spawner;
    public GameObject bulletPrefab; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RotateTowardsMouse();
        CheckFiring();
    }

    public void RotateTowardsMouse()
    {
        float angle = GetAngleTowardsMouse();
        transform.rotation = Quaternion.Euler(0, 0, angle);

        spriteRenderer.flipY = angle >= 90 && angle <= 270;
    }

    public float GetAngleTowardsMouse()
    {
        if (Mouse.current == null) return 0f;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseScreen = new Vector3(mousePosition.x, mousePosition.y, Camera.nearClipPlane);
        Vector3 mouseWorldPosition = Camera.ScreenToWorldPoint(mouseScreen);

        Vector3 mouseDirection = mouseWorldPosition - transform.position;
        mouseDirection.z = 0;

        float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360) % 360;
        return angle;
    }

    private void CheckFiring() 
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            GameObject bullet = Instantiate(bulletPrefab); 
            bullet.transform.position = spawner.position;
            bullet.transform.rotation = spawner.rotation;
            Destroy(bullet, 2f);
        }
    }
}