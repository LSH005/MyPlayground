using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public static float Threshold = 0.05f;
    public static float cameraTrackingSpeed = 8f;
    public static float toleranceY = 3.5f;
    public static float yTrackingDampening = 3f;
    public static bool normalizeRotation = true;

    private Vector3 mainPosition;
    private Vector3 mainRotation;
    private Vector3 rotationOffset = Vector3.zero;
    private Transform positionTrackingTarget;
    private Transform rotationTrackingTarget;
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

    private void LateUpdate()
    {
        transform.position = mainPosition;
        transform.rotation = Quaternion.Euler(mainRotation);
    }
    private Vector3 NormalizeAngles(Vector3 angles)
    {
        float x = angles.x % 360f;
        float y = angles.y % 360f;
        float z = angles.z % 360f;

        if (x < 0) x += 360f;
        if (y < 0) y += 360f;
        if (z < 0) z += 360f;

        return new Vector3(x, y, z);
    }

    public static void DollyTo(Vector2 targetPosition, float duration)
    {
        if (Instance.panCoroutine != null)
        {
            Instance.StopCoroutine(Instance.panCoroutine);
        }
        Instance.canStopMovement = true;
        Instance.panCoroutine = Instance.StartCoroutine(Instance.CameraMoveCoroutine(targetPosition,Vector3.zero, duration));
    }

    public static void TargetTracking(Transform targetPosition, Vector3 offset)
    {
        if (Instance.panCoroutine != null)
        {
            Instance.StopCoroutine(Instance.panCoroutine);
        }

        Instance.canStopMovement = false;
        Instance.positionTrackingTarget = targetPosition;
        Instance.panCoroutine = Instance.StartCoroutine(Instance.CameraMoveCoroutine(Vector3.zero, offset, 1019.1019f));
    }

    private IEnumerator CameraMoveCoroutine(Vector2 targetPosition, Vector3 offset, float duration)
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
        }
        else
        {
            while (true)
            {
                targetPosition = positionTrackingTarget.transform.position + offset;
                if ((mainPosition - new Vector3(targetPosition.x, targetPosition.y, currentZ)).sqrMagnitude > Threshold * Threshold)
                {
                    float posX = Mathf.Lerp(mainPosition.x, targetPosition.x, cameraTrackingSpeed * Time.deltaTime);

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
                        posX,
                        posY,
                        currentZ
                    );
                }
                yield return null;
            }
        }
        panCoroutine = null;
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

    public static void RotateTo(Vector3 targetRotation, float duration)
    {
        if (Instance.rotationCoroutine != null)
        {
            Instance.StopCoroutine(Instance.rotationCoroutine);
        }
        Instance.canStopRotation = true;
        Instance.rotationCoroutine = Instance.StartCoroutine(Instance.CameraRotationCoroutine(targetRotation, Vector3.zero, duration));
    }

    public static void RotationTracking(Transform target, Vector3 offset)
    {
        if (Instance.rotationCoroutine != null)
        {
            Instance.StopCoroutine(Instance.rotationCoroutine);
        }

        Instance.rotationTrackingTarget = target;
        Instance.canStopRotation = false;
        Instance.rotationCoroutine = Instance.StartCoroutine(Instance.CameraRotationCoroutine(Vector3.zero, offset, 1019.1019f));
    }

    private IEnumerator CameraRotationCoroutine(Vector3 targetRotation, Vector3 offset, float duration)
    {
        if (canStopRotation)
        {
            if (duration > 0)
            {
                Vector3 startRotation = mainRotation;
                float elapsedTime = 0f;

                if (normalizeRotation)
                {
                    while (elapsedTime < duration)
                    {
                        mainRotation = Vector3.Lerp(startRotation, targetRotation, elapsedTime / duration);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    while (elapsedTime < duration)
                    {
                        float t = elapsedTime / duration;

                        float x = Mathf.LerpAngle(startRotation.x, targetRotation.x, t);
                        float y = Mathf.LerpAngle(startRotation.y, targetRotation.y, t);
                        float z = Mathf.LerpAngle(startRotation.z, targetRotation.z, t);

                        mainRotation = new Vector3(x, y, z);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
            }
            mainRotation = targetRotation;
        }
        else
        {
            while (true)
            {
                Vector3 targetDirection = ((rotationTrackingTarget.position - mainPosition) + offset).normalized;
                Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
                Vector3 desiredEulerAngles = desiredRotation.eulerAngles;

                float x = Mathf.LerpAngle(mainRotation.x, desiredEulerAngles.x, cameraTrackingSpeed * Time.deltaTime);
                float y = Mathf.LerpAngle(mainRotation.y, desiredEulerAngles.y, cameraTrackingSpeed * Time.deltaTime);
                float z = Mathf.LerpAngle(mainRotation.z, desiredEulerAngles.z, cameraTrackingSpeed * Time.deltaTime);

                mainRotation = new Vector3(x, y, z);

                yield return null;
            }
        }

        rotationCoroutine = null;

        if (normalizeRotation)
        {
            mainRotation = NormalizeAngles(mainRotation);
        }
    }
}