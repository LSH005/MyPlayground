using UnityEngine;

public class TB_CameraPositionZoom : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("목표 Z값 / 지속시간")]
    public float targetZ;
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.PositionZoom(targetZ, duration);
        }
    }
}
