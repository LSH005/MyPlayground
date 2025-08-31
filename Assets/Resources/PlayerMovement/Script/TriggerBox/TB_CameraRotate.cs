using UnityEngine;

public class TB_CameraRotate : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("���� / ���ӽð�")]
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
