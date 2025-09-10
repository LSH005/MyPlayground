using UnityEngine;

public class ME_CameraMovement : MonoBehaviour
{
    private Vector3 trackingPosition = Vector3.zero;
    private float cameraSpeed = 8f;
    private float cameraControlSpeed = 10;

    void Start()
    {
        trackingPosition.z = transform.position.z;
    }


    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0f)
        {
            cameraControlSpeed += 1;
            cameraControlSpeed = Mathf.Clamp(cameraControlSpeed, 0, 60);
        }
        else if (scrollInput < 0f)
        {
            cameraControlSpeed -= 1;
            cameraControlSpeed = Mathf.Clamp(cameraControlSpeed, 0, 60);
        }

        if (Input.GetKey(KeyCode.W)) trackingPosition.y += cameraControlSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S)) trackingPosition.y -= cameraControlSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D)) trackingPosition.x += cameraControlSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A)) trackingPosition.x -= cameraControlSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
        {
            trackingPosition.z -= cameraControlSpeed * Time.deltaTime;
            trackingPosition.z = Mathf.Clamp(trackingPosition.z, -25, -5);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            trackingPosition.z += cameraControlSpeed * Time.deltaTime;
            trackingPosition.z = Mathf.Clamp(trackingPosition.z, -25, -5);
        }

        if (Vector3.Distance(transform.position, trackingPosition) > 0.02f)
        {
            transform.position = Vector3.Lerp(transform.position, trackingPosition, cameraSpeed * Time.deltaTime);
        }
    }
}
