using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runSpeed = 6f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform playerCamera;
    public Animator animator;

    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;
    private bool isClimbing;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Mouse look rotation
        HandleMouseLook();

        // Normal movement and actions
        isGrounded = controller.isGrounded;
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        bool isRunning = horizontal != 0f || vertical != 0f;
        animator.SetBool("isRunning", isRunning);

        controller.Move(move * runSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        bool isRunning = animator.GetBool("isRunning");

        if (isGrounded && Input.GetButtonDown("Jump") && !isJumping)
        {
            if (isRunning)
            {
                animator.SetTrigger("jumpRun");
            }
            else
            {
                animator.SetTrigger("jumpIdle");
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }

        if (isGrounded && velocity.y < 0)
        {
            // Reset jump state when grounded
            animator.ResetTrigger("jumpRun");
            animator.ResetTrigger("jumpIdle");

            velocity.y = -2f;
            isJumping = false;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void Bounce(float bounceHeight)
    {
        velocity.y = Mathf.Sqrt(bounceHeight * -2f * gravity);
        isJumping = true; // Prevents double-jumping mid-bounce
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }


}
