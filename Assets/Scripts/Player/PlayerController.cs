using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float horizontalMovement;

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
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        //Debug.Log("Grounded?: " + IsGrounded());
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump key pressed!");
        if (context.performed && IsGrounded())
        {
            //Debug.Log("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckCollider.radius, groundLayer);
    }
}
