using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // 이 스크립트가 붙을 오브젝트에 BoxCollider2D를 강제
public class obstacleRemover : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        Vector2 originalSize = boxCollider.size;
        boxCollider.size = Vector2.zero;

        Collider2D hit = Physics2D.OverlapBox(transform.position, Vector2.zero, 0);

        if (hit != null && hit.CompareTag("obstacle"))
        {
            Destroy(gameObject);
        }
        else
        {
            boxCollider.size = originalSize;
        }
    }
}