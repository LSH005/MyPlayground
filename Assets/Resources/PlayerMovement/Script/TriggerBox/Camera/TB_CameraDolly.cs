using UnityEngine;

public class TB_CameraDolly : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("��ġ / ���ӽð�")]
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
