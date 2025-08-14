using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f; // 코요테 타임 지속 시간
    public float quickTrunTime = 0.15f;
    [Header("Ground")]
    //public Transform groundCheck;
    public Transform groundCheckLeft;
    public Transform groundCheckCenter;
    public Transform groundCheckRight;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    [Header("Wall")]
    public Transform wallCheckEyes;
    public Transform wallCheckFeet;
    public LayerMask wallLayer;
    public float wallCheckRadius = 0.2f;

    private float wallRunStiffnessTimeCounter = 0;
    private float AirborneTimeCounter = 0;
    private float moveInput;
    private float afterMoveInput = 0f;
    private bool isAirborne = false;
    private bool isGrounded = true;
    private bool canWallRun = false;
    private bool isTouchingWall = false;
    private bool isCrashingWall = false;
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private Animator anim;

    private float coyoteTimeCounter; // 코요테 타임 카운터

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        bool eyesTouchingWall = Physics2D.OverlapCircle(wallCheckEyes.position, wallCheckRadius, wallLayer);
        bool feetTouchingWall = Physics2D.OverlapCircle(wallCheckFeet.position, wallCheckRadius, wallLayer);
        isTouchingWall = eyesTouchingWall || feetTouchingWall;

        bool eyesCrashingWall = Physics2D.OverlapCircle(wallCheckEyes.position, wallCheckRadius, groundLayer);
        bool feetCrashingWall = Physics2D.OverlapCircle(wallCheckFeet.position, wallCheckRadius, groundLayer);
        isCrashingWall = eyesCrashingWall || feetCrashingWall;

        bool leftFoot = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, groundLayer);
        bool centerFoot = Physics2D.OverlapCircle(groundCheckCenter.position, groundCheckRadius, groundLayer);
        bool rightFoot = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, groundLayer);
        isGrounded = leftFoot || centerFoot || rightFoot;
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //moveInput = Input.GetAxisRaw("Horizontal");
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            moveInput = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
        }
        else
        {
            moveInput = 1;
        }

        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f);

        if (anim.GetBool("isRunning"))
        {
            afterMoveInput = 0;
        }
        else
        {
            afterMoveInput += Time.deltaTime;
        }

        if (wallRunStiffnessTimeCounter > 0)
        {
            canWallRun = false;
            wallRunStiffnessTimeCounter -= Time.deltaTime;
            if (wallRunStiffnessTimeCounter <= 0)
            {
                canWallRun = true;
            }
        }

        if (AirborneTimeCounter > 0)
        {
            isAirborne = true;
            AirborneTimeCounter -= Time.deltaTime;
            if (isCrashingWall)
            {
                isAirborne = false;
                AirborneTimeCounter = 0;
            }
            if (AirborneTimeCounter <= 0 && anim.GetBool("isRunning"))
            {
                isAirborne = false;
            }
        }
        else if (isAirborne && (anim.GetBool("isRunning") || isCrashingWall || isGrounded))
        {
            isAirborne = false;
        }

        if (!isAirborne)
        {
            if (moveInput > 0 && !isFacingRight) Flip();
            else if (moveInput < 0 && isFacingRight) Flip();
        }
        
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("isJumping", false);

            if (Input.GetKey(KeyCode.LeftShift) && anim.GetBool("isRunning"))
            {
                anim.SetBool("isSliding", true);
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            anim.SetBool("isJumping", true);
        }

        if (Input.GetKey(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            var vel = rb.velocity;
            vel.y = jumpForce;
            rb.velocity = vel;

            coyoteTimeCounter = 0f;
            SetWallRunStiffness(0.2f);
            SetAirborne(0f, false);

            anim.SetBool("isJumping", true);

            if (anim.GetBool("isSliding"))
            {
                anim.SetBool("isSliding", false);
            }
        }

        if (anim.GetBool("isSliding"))
        {
            if (isCrashingWall)
            {
                anim.SetBool("isSliding", false);
            }
            
            if (coyoteTimeCounter <= 0f)
            {
                anim.SetBool("isSliding", false);
            }
        }

        if (isTouchingWall && anim.GetBool("isJumping") && !anim.GetBool("isWallKicking") && !isAirborne && canWallRun)
        {
            anim.SetBool("isWallKicking", true);
        }
        if (anim.GetBool("isWallKicking"))
        {
            rb.velocity = new Vector2(0f, -1f);

            if (isGrounded ||
                !isTouchingWall)
            {
                anim.SetBool("isWallKicking", false);
                Flip();
            }
            else if (isFacingRight && moveInput < 0 || !isFacingRight && moveInput > 0)
            {
                Flip();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isWallKicking", false);
                SetAirborne(0.1f, true);

                if (isFacingRight)
                {
                    rb.velocity = new Vector2(jumpForce * -1, jumpForce);
                }
                else
                {
                    rb.velocity = new Vector2(jumpForce, jumpForce);
                }
                Flip();
            }
        }
    }

    void FixedUpdate()
    {

        if (anim.GetBool("isSliding"))
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(1.2f * moveSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-1.2f * moveSpeed, 0);
            }
        }
        else if(!isAirborne)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        anim.SetBool("isSliding", false);
        if (anim.GetBool("isWallKicking"))
        {
            anim.SetBool("isWallKicking", false);
            wallRunStiffnessTimeCounter = 0.1f;
        }
    }

    void SetAirborne(float time, bool isAbs)
    {
        AirborneTimeCounter = AirborneTimeCounter < time ? time : AirborneTimeCounter;

        if (isAbs)
        {
            isAirborne = true;
        }
        else if (anim.GetBool("isRunning"))
        {
            isAirborne = false;
            AirborneTimeCounter = 0f;
        }
        else
        {
            isAirborne = true;
        }
    }

    void SetWallRunStiffness(float time)
    {
        if (time > 0)
        {
            canWallRun = false;

            if (time > wallRunStiffnessTimeCounter)
            {
                wallRunStiffnessTimeCounter = time;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckLeft != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckLeft.position, groundCheckRadius);
        }
        if (groundCheckCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckCenter.position, groundCheckRadius);
        }
        if (groundCheckRight != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckRight.position, groundCheckRadius);
        }

        //if (groundCheck != null)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        //}

        if (wallCheckEyes != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckEyes.position, wallCheckRadius);
        }

        if (wallCheckFeet != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckFeet.position, wallCheckRadius);
        }
    }
}