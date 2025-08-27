using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public static float Threshold = 0.05f;
    public static float cameraTrackingSpeed = 10f;
    public static float toleranceY = 3.5f;
    public static float yTrackingDampening = 3f;

    private Vector3 mainPosition;
    private Vector3 positionOffset = Vector3.zero;
    private Vector3 mainRotation;
    private Vector3 rotationOffset;
    private Transform positionTrackingTarget;
    private Vector2 positionTrackingOffset = Vector2.zero;
    private float currentZ;
    private bool canStopMovement = false;
    private bool canStopRotation = false;
    private Coroutine panCoroutine;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainPosition = transform.position;
        currentZ = transform.position.z;
    }

    private void Update()
    {
        transform.position = mainPosition + positionOffset;
        transform.rotation = Quaternion.Euler(mainRotation + rotationOffset);
    }

    public static void CameraPanTo(Vector2 targetPosition, float duration)
    {
        Instance.CameraPanTo_(targetPosition, duration);
    }

    private void CameraPanTo_(Vector2 targetPosition, float duration)
    {
        if (panCoroutine != null)
        {
            StopCoroutine(panCoroutine);
        }

        canStopMovement = true;
        panCoroutine = StartCoroutine(CameraPanCoroutine(targetPosition, duration));
    }

    public static void CameraFollow(Transform targetPosition)
    {
        Instance.CameraFollow_(targetPosition);
    }

    private void CameraFollow_(Transform targetPosition)
    {
        if (panCoroutine != null)
        {
            StopCoroutine(panCoroutine);
        }

        canStopMovement = false;
        positionTrackingTarget = targetPosition;
        panCoroutine = StartCoroutine(CameraPanCoroutine(mainPosition, 0f));
    }

    private IEnumerator CameraPanCoroutine(Vector2 targetPosition, float duration)
    {
        if (canStopMovement)
        {
            if (duration > 0)
            {
                Vector2 startPosition = mainPosition;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    Vector2 LerpPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
                    mainPosition = new Vector3(LerpPosition.x, LerpPosition.y, currentZ);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            mainPosition = new Vector3(targetPosition.x, targetPosition.y, currentZ);
            panCoroutine = null;
        }
        else
        {
            while (true)
            {
                targetPosition = positionTrackingTarget.transform.position;
                if ((mainPosition - new Vector3(targetPosition.x, targetPosition.y, currentZ)).sqrMagnitude > Threshold * Threshold)
                {
                    float posZ = Mathf.Lerp(mainPosition.x, targetPosition.x, cameraTrackingSpeed * Time.deltaTime);

                    float posY;
                    if (
                        Mathf.Lerp(mainPosition.y, targetPosition.y, (cameraTrackingSpeed / yTrackingDampening) * Time.deltaTime)
                        !=
                        Mathf.Clamp(Mathf.Lerp(mainPosition.y, targetPosition.y, (cameraTrackingSpeed / yTrackingDampening) * Time.deltaTime), targetPosition.y - toleranceY, targetPosition.y + toleranceY))
                    {
                        posY = Mathf.Lerp(mainPosition.y, targetPosition.y, Mathf.Max(cameraTrackingSpeed * 10f) * Time.deltaTime);
                    }
                    else
                    {
                        posY = Mathf.Lerp(mainPosition.y, targetPosition.y, (cameraTrackingSpeed / yTrackingDampening) * Time.deltaTime);
                    }

                    mainPosition = new Vector3(
                        posZ,
                        posY,
                        currentZ
                    );
                }
                yield return null;
            }
        }
    }

    public static void CameraZoomTo(float targetZ, float duration)
    {
        Instance.CameraZoomTo_(-targetZ, duration);
    }

    private void CameraZoomTo_(float targetZ, float duration)
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }

        zoomCoroutine = StartCoroutine(CameraZoomCoroutine(targetZ, duration));
    }

    private IEnumerator CameraZoomCoroutine(float targetZ, float duration)
    {
        if (duration > 0)
        {
            float startZ = currentZ;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                mainPosition.z = currentZ = Mathf.Lerp(startZ, targetZ, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        mainPosition.z = currentZ = targetZ;
        zoomCoroutine = null;
    }

    //public static void 
}