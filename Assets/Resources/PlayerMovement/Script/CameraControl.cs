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
            CameraMovement.TargetTracking(target, new Vector3(0, 2, 0));
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.RotationTracking(target, Vector3.zero);
        }
    }
}
