using UnityEngine;

public class TB_CameraDolly : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("위치 / 지속시간")]
    public Vector2 targetPosition;
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.DollyTo(targetPosition, duration);
        }
    }
}
