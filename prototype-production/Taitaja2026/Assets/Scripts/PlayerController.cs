using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // INPUT
    private PlayerControls input;
    private Vector2 moveInput;

    // AUDIO
    [Header("Audio")]
    public AudioSource DashSFX;
    public AudioSource JumpSFX;

    // MOVEMENT
    [Header("Movement")]
    private float horizontal;
    private bool isFacingRight = true;
    private bool doubleJump;
    private bool wasGrounded;

    public float walkSpeed = 7f;
    public float runSpeed = 10f;
    public float jumpingPower = 16f;
    private float doubleJumpPower;

    // DASHING
    [Header("Dashing")]
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    // WALL SLIDE
    [Header("Wall Sliding")]
    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    // WALL JUMP
    [Header("Wall Jumping")]
    private bool isWallJumping;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // REFERENCES
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;

    private void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Jump.performed += OnJump;
        input.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        input.Player.Jump.performed -= OnJump;
        input.Player.Dash.performed -= OnDash;
        input.Player.Disable();
    }

    private void Start()
    {
        doubleJumpPower = jumpingPower * 0.75f;
    }

    private void Update()
    {
        if (isDashing)
            return;

        moveInput = input.Player.Move.ReadValue<Vector2>();
        horizontal = moveInput.x;

        bool grounded = IsGrounded();
        if (grounded && !wasGrounded)
        {
            doubleJump = true;
        }
        wasGrounded = grounded;

        WallSlide();
        WallJump();

        if (!isWallJumping)
            Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing || isWallJumping)
            return;

        float speed = input.Player.Run.IsPressed() && IsGrounded()
            ? runSpeed
            : walkSpeed;

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }


    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isDashing)
            return;

        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            JumpSFX?.Play();
        }
        else if (doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpPower);
            doubleJump = false;
            JumpSFX?.Play();
        }
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        if (canDash)
            StartCoroutine(Dash());
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(
                rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue)
            );
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (wallJumpingCounter > 0f && input.Player.Jump.triggered)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(
                wallJumpingDirection * wallJumpingPower.x,
                wallJumpingPower.y
            );

            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        tr.emitting = true;
        DashSFX?.Play();

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
