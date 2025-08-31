using UnityEngine;

public class TB_CameraSetFOV : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("목표 FOV값 / 지속시간")]
    public float targetFOV;
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.SetFOV(targetFOV, duration);
        }
    }
}
