using UnityEngine;

public class TB_CameraSetFOV : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("��ǥ FOV�� / ���ӽð�")]
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
