using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.CameraFollow(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.CameraFollow(target, new Vector3(0, 2, 0));
        }
    }
}
