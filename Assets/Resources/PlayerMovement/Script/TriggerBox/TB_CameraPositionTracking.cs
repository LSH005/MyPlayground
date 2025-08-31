using UnityEngine;

public class TB_CameraPositionTracking : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("��ġ")]
    public Vector2 targetPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.PositionTracking(targetPosition);
        }
    }
}
