using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f;
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

    private float coyoteTimeCounter;

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

        UpdateStates();
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        /*
        //moveInput = Input.GetAxisRaw("Horizontal");
        제 1대 방향키 동작 코드 (사망)
        사망 사유 : 개발자는 화살표 키를 감지하는 것을 원치 않음
        
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
        제 2대 방향키 동작 코드 (사망)
        사망 사유 : 마음에 안 들음.

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveInput = -1;
            }
            else
            {
                moveInput = 1;
            }
        }
        else
        {
            moveInput = 0;
        }
        제 3대 방향키 동작 코드 (사망)
        사망 사유 : 제 4대 방향키 동작 코드가 더 뛰어난 계산 효율을 가짐
        */

        if (Input.GetKey(KeyCode.A)) moveInput = -1;
        else if (Input.GetKey(KeyCode.D)) moveInput = 1;
        else moveInput = 0;
        // 제 4대 방향키 동작 코드

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
                CheckFlip();
            }
            if (AirborneTimeCounter <= 0 && anim.GetBool("isRunning"))
            {
                isAirborne = false;
            }
        }
        else if ((isAirborne && (anim.GetBool("isRunning")) || isCrashingWall || isGrounded))
        {
            isAirborne = false;
            CheckFlip();
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("isJumping", false);
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            anim.SetBool("isJumping", true);
        }

    }

    private void LateUpdate()
    {
        // 방향 전환
        if (!isAirborne)
        {
            CheckFlip();
        }

        // 점프
        if (Input.GetKey(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            SetWallRunStiffness(0.2f);

            if (anim.GetBool("isSliding"))
            {
                SetAirborne(0.05f);
                anim.SetBool("isSliding", false);
            }
            anim.SetBool("isJumping", true);
        }

        // 슬라이딩
        if (anim.GetBool("isSliding"))
        {
            if (isCrashingWall || coyoteTimeCounter <= 0f)
            {
                // 슬라이딩 해제 조건 :
                // 벽에 충돌 || 공중에 뜸 || Flip() 호출됨 (방향을 반대로 꺽음)
                anim.SetBool("isSliding", false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && anim.GetBool("isRunning") && !isCrashingWall && isGrounded)
            {
                anim.SetBool("isSliding", true);
            }
        }

        // 월 킥 (공중에서 벽에 붙기)
        if (isTouchingWall && anim.GetBool("isJumping") && !anim.GetBool("isWallKicking") && !isAirborne && canWallRun)
        {
            anim.SetBool("isWallKicking", true);
        }
        if (anim.GetBool("isWallKicking"))
        {
            rb.velocity = new Vector2(0f, -1f);

            if (isGrounded || !isTouchingWall || (isFacingRight && moveInput < 0) || (!isFacingRight && moveInput > 0))
            {
                // 월 킥 해제 조건 :
                // 땅에 닿음 || 벽에서 떨어짐 || 오른쪽 벽에 붙어 A 누르기 || 왼쪽 벽에 붙어 D 누르기
                anim.SetBool("isWallKicking", false);
                Flip();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // 월 킥 발동 조건 : 벽에서 미끄러지는 도중 스페이스 바 누르기
                anim.SetBool("isWallKicking", false);
                SetAirborne(0.1f);

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
        else if(!isAirborne && !anim.GetBool("isWallKicking"))
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

        UpdateStates();
    }

    void UpdateStates()
    {
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
    }

    void SetAirborne(float time)
    {
        AirborneTimeCounter = AirborneTimeCounter < time ? time : AirborneTimeCounter;
        isAirborne = true;
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

    void CheckFlip()
    {
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
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