using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float horizontalMovement;
    float horizontalInput;
    float horizontalMovementSmoothSpeed = 6.5f;

    [Header("Jumping")]
    public float jumpPower = 10f;

    [SerializeField] Transform groundCheck;
    CircleCollider2D groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckCollider = groundCheck.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        horizontalMovement = Mathf.Lerp(horizontalMovement, horizontalInput, Time.deltaTime * horizontalMovementSmoothSpeed);
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// Handles horizontal movement. Should be called by PlayerInput component.
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    /// <summary>
    /// Handles jumping. Should be called by PlayerInput component.
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        // Makes the player jump if jump key is pressed and player is grounded
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        // "Stops" the player's jump early if jump key is released, allows for variable jump height
        if (context.canceled && rb.velocity.y > 0)
        {
            Vector2 velocity = rb.velocity;
            float gravity = rb.gravityScale;
            velocity = new Vector2(velocity.x, -(velocity.y * gravity * 0.5f));

            rb.velocity += velocity;
        }
    }

    /// <summary>
    /// Checks if the player is considered "standing on" anything in the Ground layer.
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckCollider.radius, groundLayer);
    }
}
