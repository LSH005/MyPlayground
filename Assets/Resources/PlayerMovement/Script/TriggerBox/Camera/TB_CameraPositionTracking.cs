using UnityEngine;

public class TB_CameraPositionTracking : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("위치")]
    public Vector2 targetPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.PositionTracking(targetPosition);
        }
    }
}
