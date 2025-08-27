using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.RotateTo(new Vector3(-45, 0, 0), 2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.RotateTo(new Vector3(-4, 40, -20), 2f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.RotateTo(Vector3.zero, 3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.RotateTo(new Vector3(0, 0, -360), 3f);
        }
    }
}
