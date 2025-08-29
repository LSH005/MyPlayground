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
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            
        }
    }
}
