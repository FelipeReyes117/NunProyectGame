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

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>(); // Estaba mal escrito "GetComponet"
    }

    void Update()
    {
        moveInput = Keyboard.current != null
            ? new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
              ).normalized
            : Vector2.zero;

        PlayerAnimator.SetFloat("Horizontal", moveInput.x); // moveX no existía
        PlayerAnimator.SetFloat("Vertical", moveInput.y);   // moveY no existía
        PlayerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}