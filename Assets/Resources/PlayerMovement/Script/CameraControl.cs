using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.CameraPanTo(target.position, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.CameraPanTo(target.position, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.CameraFollow(target);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.CameraZoomTo(5f, 2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CameraMovement.CameraZoomTo(10f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CameraMovement.CameraZoomTo(20f, 0.2f);
        }
    }
}
