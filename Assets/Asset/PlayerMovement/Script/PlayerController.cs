using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpForce = 15f; // ���� ��
    public float coyoteTime = 0.25f; // �ڿ��� Ÿ��
    public Transform groundCheck; // �ٴ��� ������ ��ġ
    public LayerMask groundLayer; // �ٴ����� �ν��� ���̾�
    public float groundCheckRadius = 0.2f; // �ٴ� ���� ���� ������

    private float moveInput;
    public float coyote;
    private bool isGrounded;
    private bool isAfterCoyoteTime;
    private bool isFacingRight = true;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("PlayerMovement");
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            coyote = 0f;
            isAfterCoyoteTime = false;
        }
        else if (coyote <= coyoteTime)
        {
            coyote += Time.deltaTime;
            if (coyote >= coyoteTime)
            {
                isAfterCoyoteTime = true;
            }
        }

        if (Input.GetKey(KeyCode.Space) && !isAfterCoyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            coyote = coyoteTime;
            isAfterCoyoteTime = true;
        }

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
