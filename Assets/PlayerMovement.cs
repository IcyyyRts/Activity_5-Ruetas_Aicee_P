using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // A = forward, S = backward, D = left, W = right
        if (Keyboard.current.aKey.isPressed) moveZ =  1f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        if (Keyboard.current.dKey.isPressed) moveX = -1f;
        if (Keyboard.current.wKey.isPressed) moveX =  1f;

        // Shift = sprint
        float currentSpeed = Keyboard.current.leftShiftKey.isPressed
                             ? sprintSpeed : moveSpeed;

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        rb.linearVelocity = new Vector3(
            moveDir.x * currentSpeed,
            rb.linearVelocity.y,
            moveDir.z * currentSpeed
        );

        // Rotate to face movement direction
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                10f * Time.fixedDeltaTime
            );
        }
    }

    void Update()
    {
        // Space = jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}