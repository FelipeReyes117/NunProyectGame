using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // ── Movimiento ────────────────────────────────────────────────
    private float baseSpeed = 3f;
    private float speed;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator PlayerAnimator;
    // ─────────────────────────────────────────────────────────────

    [Header("Daño recibido")]
    public float knockbackDuration = 0.4f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private Vector2 knockbackVelocity;

    // ── Speed Boost ───────────────────────────────────────────────
    private bool hasSpeedBoost = false;
    private GameObject activeSpeedParticle;
    // ─────────────────────────────────────────────────────────────

    // ── Escudo ────────────────────────────────────────────────────
    private bool hasShield = false;
    private GameObject activeShieldEffect;
    // ─────────────────────────────────────────────────────────────

    void Start()
    {
        speed = baseSpeed; //  inicializa la velocidad base
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
            playerRb.linearVelocity = knockbackVelocity;
            return;
        }
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    // ── Daño y Knockback ──────────────────────────────────────────
    public void TakeDamage(int damage, Vector2 knockbackForce)
    {
        //  Escudo absorbe el golpe
        if (hasShield)
        {
            hasShield = false;
            if (activeShieldEffect != null)
                Destroy(activeShieldEffect);
            return;
        }

        for (int i = 0; i < damage; i++)
            GameManager.instance.PerderVidas();

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        knockbackVelocity = knockbackForce;
    }

    // ── Speed Boost ───────────────────────────────────────────────
    public void ApplySpeedBoost(float multiplier, float duration, GameObject particlePrefab)
    {
        if (hasSpeedBoost) return; // evita stackear el buff
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration, particlePrefab));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration, GameObject particlePrefab)
    {
        hasSpeedBoost = true;
        speed = baseSpeed * multiplier; //  aumenta velocidad

        //  Instancia partículas como hijo del player
        if (particlePrefab != null)
            activeSpeedParticle = Instantiate(
                particlePrefab,
                transform.position,
                Quaternion.identity,
                transform  // ← hijo del player, lo sigue
            );

        yield return new WaitForSeconds(duration);

        speed = baseSpeed; //  restaura velocidad base
        hasSpeedBoost = false;

        if (activeSpeedParticle != null)
            Destroy(activeSpeedParticle);
    }

    // ── Escudo ────────────────────────────────────────────────────
    public void ApplyShield(GameObject shieldPrefab)
    {
        if (hasShield) return; // ya tiene escudo
        hasShield = true;

        if (shieldPrefab != null)
            activeShieldEffect = Instantiate(
                shieldPrefab,
                transform.position,
                Quaternion.identity,
                transform  // ← hijo del player, lo sigue
            );
    }
}