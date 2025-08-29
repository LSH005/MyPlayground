using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    private PlayerController pc;

    private void Awake()
    {
        pc = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.TargetTracking(target, Vector3.zero);
            CameraMovement.RotationTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.TargetTracking(target, new Vector3(-6, -6f, 0));
            CameraMovement.RotationTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.TargetTracking(target, new Vector3(0, 6f, 0));
            CameraMovement.RotationTracking(target, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraMovement.PositionShaking(1, 0.035f, 1);
            CameraMovement.RotationShaking(14, 0.05f, 1);
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
