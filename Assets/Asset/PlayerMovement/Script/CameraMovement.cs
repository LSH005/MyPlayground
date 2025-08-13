using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform followTarget;
    public float followSpeed = 10f;

    private float OriginalPosZ;

    private void Awake()
    {
        OriginalPosZ = transform.position.z;
    }

    void Update()
    {
        if (followTarget == null)
        {
            return;
        }

        Vector3 targetPos = followTarget.position;
        targetPos.z = OriginalPosZ;
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }
}