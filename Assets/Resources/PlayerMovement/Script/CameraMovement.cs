using System.Collections;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public static float Threshold = 0.05f;
    public static float cameraTrackingSpeed = 8f;
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
    private Coroutine rotationCoroutine;

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
        if (Instance.panCoroutine != null)
        {
            Instance.StopCoroutine(Instance.panCoroutine);
        }
        Instance.canStopMovement = true;
        Instance.panCoroutine = Instance.StartCoroutine(Instance.CameraPanCoroutine(targetPosition, duration));
    }

    public static void CameraFollow(Transform targetPosition)
    {
        if (Instance.panCoroutine != null)
        {
            Instance.StopCoroutine(Instance.panCoroutine);
        }

        Instance.canStopMovement = false;
        Instance.positionTrackingTarget = targetPosition;
        Instance.panCoroutine = Instance.StartCoroutine(Instance.CameraPanCoroutine(Vector3.zero, 1019.1019f));
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
                        posY = Mathf.Lerp(mainPosition.y, targetPosition.y, Mathf.Max(cameraTrackingSpeed * 1.75f, 10f) * Time.deltaTime);
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
        if (Instance.zoomCoroutine != null)
        {
            Instance.StopCoroutine(Instance.zoomCoroutine);
        }

        Instance.zoomCoroutine = Instance.StartCoroutine(Instance.CameraZoomCoroutine(-targetZ, duration));
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

    public static void CameraRotateTo(Vector3 targetRotation, float duration)
    {
        if (Instance.rotationCoroutine != null)
        {
            Instance.StopCoroutine(Instance.rotationCoroutine);
        }
        Instance.canStopRotation = true;
        Instance.rotationCoroutine = Instance.StartCoroutine(Instance.CameraRotationCoroutine(targetRotation, duration));
    }

    private IEnumerator CameraRotationCoroutine(Vector3 targetRotation, float duration)
    {
        if (canStopRotation)
        {
            if (duration > 0)
            {
                Vector3 startRotation = mainRotation;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    mainRotation = Vector3.Lerp(startRotation, targetRotation, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            mainRotation = targetRotation;
            rotationCoroutine = null;
        }
    }
}