using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator PlayerAnimator;

    [Header("Daño recibido")]
    public float knockbackDuration = 0.4f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackVelocity; // ✅ guardamos el knockback aquí

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                knockbackVelocity = Vector2.zero;
            }
            return;
        }

        moveInput = Keyboard.current != null
            ? new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
              ).normalized
            : Vector2.zero;

        PlayerAnimator.SetFloat("Horizontal", moveInput.x);
        PlayerAnimator.SetFloat("Vertical", moveInput.y);
        PlayerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            // ✅ aplicamos el knockback en FixedUpdate donde vive la física
            playerRb.linearVelocity = knockbackVelocity;
            return;
        }
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage, Vector2 knockbackForce)
    {
        for (int i = 0; i < damage; i++)
            GameManager.instance.PerderVidas();

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        knockbackVelocity = knockbackForce; // ✅ guardamos para aplicar en FixedUpdate
    }
}