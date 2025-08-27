using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.TargetTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.RotationTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.PositionShaking(1f, 0.025f, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.PositionShaking(0.2f, 0.04f, 5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CameraMovement.PositionShaking(100f, (5/10), 5f);
        }
    }
}
