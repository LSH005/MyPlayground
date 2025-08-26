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
            CameraMovement.CameraPanTo(target.position, 2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.CameraPanTo(target.position, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.CameraFollow(target);
        }
    }
}
