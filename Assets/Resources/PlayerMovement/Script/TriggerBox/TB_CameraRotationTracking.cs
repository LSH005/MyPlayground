using UnityEngine;

public class TB_CameraRotationTracking : MonoBehaviour
{
    [Header("еб╠в")]
    public string compareTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.RotationTracking(other.transform, Vector3.zero);
        }
    }
}
