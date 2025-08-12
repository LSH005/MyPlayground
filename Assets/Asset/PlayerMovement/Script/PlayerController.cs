using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float coyoteTime = 0.1f; // 코요테 타임 지속 시간
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private float moveInput;
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

        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
        {
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("isJumping", false);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (coyoteTimeCounter <= 0f)
            {
                anim.SetBool("isJumping", true);
            }
        }

        if (Input.GetKey(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f;
            anim.SetBool("isJumping", true);
        }


        moveInput = Input.GetAxisRaw("Horizontal");

        
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0.1f);

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}