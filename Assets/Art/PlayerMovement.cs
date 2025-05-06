using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 7f;
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;
    public int maxJumps = 2;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform cameraTransform;
    public Transform groundCheck;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isMoving;
    private bool isGrounded;
    private int jumpCount;
    private bool isDashing;
    private float dashTimer;
    private Vector3 dashDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            cameraTransform = FindObjectOfType<Camera>().transform;
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
            else
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
                return;
            }
        }

        float moveX = Input.GetAxis("Horizontal");
        //float moveZ = Input.GetAxis("Vertical");
        float moveZ = 0f;

        Vector3 move = cameraTransform.right * moveX + GetFlatCameraDirection() * moveZ;
        move.y = 0f; // Ensure the movement is parallel to the ground
        move = Vector3.ClampMagnitude(move, 1f);

        if (move.magnitude >= 0.1f)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            // Apply the angle offset to the target rotation
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            // Smoothly rotate the player towards the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        _ = controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpCount++;

        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            dashDirection = move.normalized;
            isDashing = true;
            dashTimer = dashTime;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public Vector3 GetFlatCameraDirection()
    {
        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0f; // Zero out the Y component to make it horizontal
        flatForward.Normalize();
        return flatForward;
    }
}