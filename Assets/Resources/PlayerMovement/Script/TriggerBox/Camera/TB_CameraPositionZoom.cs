using UnityEngine;

public class TB_CameraPositionZoom : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("��ǥ Z�� / ���ӽð�")]
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
