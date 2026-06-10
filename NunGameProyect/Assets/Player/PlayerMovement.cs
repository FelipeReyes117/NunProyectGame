using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // ── Movimiento ────────────────────────────────────────────────
    private float baseSpeed = 4f;
    private float speed;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator PlayerAnimator;
    private myControls controls;
    private AudioSource footstepAudio;
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

    void Awake()
    {
        controls = new myControls();
    }
public Vector2 GetMoveDirection()
{
    return moveInput;
}
    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
    	footstepAudio = GetComponent<AudioSource>();
        speed = baseSpeed;
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

        PlayerAnimator.SetFloat("Horizontal", moveInput.x);
        PlayerAnimator.SetFloat("Vertical", moveInput.y);
        PlayerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
        
         if (moveInput != Vector2.zero)
    {
        if (!footstepAudio.isPlaying)
            footstepAudio.Play();
    }
    else
    {
        footstepAudio.Stop();
    }
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
        if (hasSpeedBoost) return;
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration, particlePrefab));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration, GameObject particlePrefab)
    {
        hasSpeedBoost = true;
        speed = baseSpeed * multiplier;

        if (particlePrefab != null)
            activeSpeedParticle = Instantiate(
                particlePrefab,
                transform.position,
                Quaternion.identity,
                transform
            );

        yield return new WaitForSeconds(duration);

        speed = baseSpeed;
        hasSpeedBoost = false;

        if (activeSpeedParticle != null)
            Destroy(activeSpeedParticle);
    }

    // ── Escudo ────────────────────────────────────────────────────
    public void ApplyShield(GameObject shieldPrefab)
    {
        if (hasShield) return;
        hasShield = true;

        if (shieldPrefab != null)
            activeShieldEffect = Instantiate(
                shieldPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
    }
}
