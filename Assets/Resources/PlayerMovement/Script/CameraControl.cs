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
            CameraMovement.RotationTracking(target, new Vector3(0, 0, 180));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.RotationTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.PositionShaking(0.2f, 0.04f, 5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CameraMovement.PositionShaking(0.2f, 0.04f, Mathf.Infinity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CameraMovement.normalizeRotation = false;
            CameraMovement.RotateTo(new Vector3(0, 0, 720f), 1f);
            CameraMovement.normalizeRotation = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CameraMovement.normalizeRotation = true;
        }
    }
}
