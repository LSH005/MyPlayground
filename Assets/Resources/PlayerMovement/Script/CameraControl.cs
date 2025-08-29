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
            CameraMovement.DollyTo(Vector2.zero, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.ExplodingFOV(20, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CameraMovement.SetFOV(60, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CameraMovement.PositionShaking(1, 0.035f, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CameraMovement.PositionShaking(1, 0.035f, 1);
            CameraMovement.RotationShaking(14, 0.05f, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CameraMovement.PositionShaking(5, 0.035f, Mathf.Infinity);
            CameraMovement.RotationShaking(14, 0.05f, Mathf.Infinity);
        }
    }
}
