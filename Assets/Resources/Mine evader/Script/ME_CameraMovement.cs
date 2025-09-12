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

        if (scrollInput != 0)
        {
            if (scrollInput > 0f) cameraControlSpeed += 1;
            else cameraControlSpeed -= 1;

            cameraControlSpeed = Mathf.Clamp(cameraControlSpeed, 0, 60);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W)) trackingPosition.y += cameraControlSpeed * Time.deltaTime;
            else trackingPosition.y -= cameraControlSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D)) trackingPosition.x += cameraControlSpeed * Time.deltaTime;
            else trackingPosition.x -= cameraControlSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        {
            if (Input.GetKey(KeyCode.Q)) trackingPosition.z -= cameraControlSpeed * Time.deltaTime;
            else trackingPosition.z += cameraControlSpeed * Time.deltaTime;

            trackingPosition.z = Mathf.Clamp(trackingPosition.z, -55, -5);
        }
        

        if (Vector3.Distance(transform.position, trackingPosition) > 0.02f)
        {
            transform.position = Vector3.Lerp(transform.position, trackingPosition, cameraSpeed * Time.deltaTime);
        }
    }
}
