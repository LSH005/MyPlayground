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

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CameraMovement.CameraRotateTo(Vector3.zero, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CameraMovement.CameraRotateTo(new Vector3(20, 45, 0), 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraMovement.CameraRotateTo(new Vector3(0, 20, 0), 0f);
        }
    }
}
