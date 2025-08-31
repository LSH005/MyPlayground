using UnityEngine;

public class TB_CameraRotate : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("각도 / 지속시간")]
    public Vector3 targetPosition;
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.RotateTo(targetPosition, duration);
        }
    }
}
