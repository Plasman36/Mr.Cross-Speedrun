using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //movement stuff
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float speedTemp;

    //wall sliding
    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    //wall jumping
    private bool isWallJumping;
    private float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(12f, 25f);

    //coyote stuff
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //jump buffering
    public float jumpBufferingTime = 0.2f;
    private float jumpBufferingCounter;

    //gravity modifications
    private float gravity = 5;
    public float gravitymod = 10;
    public float fastFallMod = 1.5f;

    //Clamp Fall Speed
    public float fallClamp;
    public float fallClampMod;
    private float fallClampTemp;

    //Apex Speed Mod
    public float apexMod;
    public float yMod;
    public bool isJumping;
    public bool ISWORKING;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        speedTemp = speed;
        fallClampTemp = fallClamp;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //coyote time counter
        if (IsGrounded())
        {
            GravDown();
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
            ISWORKING = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //jump buffering
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferingCounter = jumpBufferingTime;
            isJumping = true;
        }
        else{
            jumpBufferingCounter -= Time.deltaTime;
        }

        if (jumpBufferingCounter>0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferingCounter = 0f;
            isJumping = true;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            coyoteTimeCounter = 0f;

            GravUp();
        }

        //checks for grav
        if (rb.velocity.y < 0f)
        {
            GravUp();
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
        //Clamp Fall
        if(rb.velocity.y < fallClamp)
        {
            rb.velocity = new Vector2(rb.velocity.x, fallClamp);
        }

        //Apex Mod
        if (rb.velocity.y <= 3 && rb.velocity.y >= -3 && isJumping == true)
        {
            speed *= apexMod;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yMod);
            ISWORKING = true;
        }
        else
        {
            speed = speedTemp;
        }

        //Speed Fall
        if (Input.GetKeyDown(KeyCode.S))
        {
            fallClamp = fallClampMod;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y+fastFallMod);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            fallClamp = fallClampTemp;
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
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
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
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
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void GravUp()
    {
        rb.gravityScale = gravitymod;
    }

    void GravDown()
    {
        rb.gravityScale = gravity;
    }

    void FastFall()
    {
        rb.gravityScale = fastFallMod;
    }
}