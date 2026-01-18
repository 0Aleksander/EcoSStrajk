using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 70f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float coyoteTime = 0.1f;      // forgiveness after leaving ground
    [SerializeField] private float jumpBufferTime = 0.1f;  // forgiveness before landing
    [SerializeField] private float fallGravityMultiplier = 1.6f;
    [SerializeField] private float lowJumpMultiplier = 1.3f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.6f, 0.15f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInput;

    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    private bool jumpHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        // Input (simple & reliable)
        moveInput = Input.GetAxisRaw("Horizontal");

        bool jumpPressed =
            Input.GetKeyDown(KeyCode.Space) ||
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame);

            if (jumpPressed) Debug.Log("JUMP PRESSED");

        bool jumpHeld =
            Input.GetKey(KeyCode.Space) ||
            (Keyboard.current != null && Keyboard.current.spaceKey.isPressed);

        // Jump pressed?
        if (jumpPressed)            
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;


        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (jumpPressed) Debug.Log("Grounded=" + isGrounded + " | coyote=" + coyoteTimer);


        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Jump if buffered + coyote allows
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            Jump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        ApplyBetterJumpFeel();
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDiff * accelRate;

        rb.AddForce(Vector2.right * movement);

        // Optional clamp for consistent max speed
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -moveSpeed, moveSpeed), rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyBetterJumpFeel()
    {
        // Falling -> heavier gravity
        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = 3f * fallGravityMultiplier;
        }
        // Rising but jump released -> shorter hop
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.gravityScale = 3f * lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 3f; // base gravity
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
