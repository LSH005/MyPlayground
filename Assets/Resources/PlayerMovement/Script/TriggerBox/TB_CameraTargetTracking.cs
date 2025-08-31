using UnityEngine;

public class TB_CameraTargetTracking : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            CameraMovement.TargetTracking(other.transform, Vector3.zero);
        }
    }
}
