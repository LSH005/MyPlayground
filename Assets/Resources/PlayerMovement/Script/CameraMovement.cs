using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public static float Threshold = 0.05f;
    public static float cameraTrackingSpeed = 7f;

    private Vector3 mainPosition;
    private Vector3 positionOffset;
    private Vector3 mainRotation;
    private Vector3 rotationOffset;
    private Transform trackingTarget;
    private float currentZ;
    private bool canStopMovement = false;
    private bool canStopRotation = false;
    private Coroutine PanCoroutine;

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
    }

    private void Start()
    {
        mainPosition = transform.position;
        positionOffset = Vector3.zero;
        currentZ = transform.position.z;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, mainPosition + positionOffset) > Threshold)
        {
            transform.position = mainPosition + positionOffset;
        }
    }

    public static void CameraPanTo(Vector2 targetPosition, float duration)
    {
        Instance.CameraPanTo_(targetPosition, duration);
    }

    private void CameraPanTo_(Vector2 targetPosition, float duration)
    {
        if (PanCoroutine != null)
        {
            StopCoroutine(PanCoroutine);
        }

        canStopMovement = true;
        PanCoroutine = StartCoroutine(CameraPanCoroutine(targetPosition, duration));
    }

    public static void CameraFollow(Transform targetPosition)
    {
        Instance.CameraFollow_(targetPosition);
    }

    private void CameraFollow_(Transform targetPosition)
    {
        if (PanCoroutine != null)
        {
            StopCoroutine(PanCoroutine);
        }

        canStopMovement = false;
        trackingTarget = targetPosition;
        PanCoroutine = StartCoroutine(CameraPanCoroutine(mainPosition, 0f));
    }

    private IEnumerator CameraPanCoroutine(Vector2 targetPosition, float duration)
    {
        if (canStopMovement)
        {
            if (duration <= 0)
            {
                mainPosition = new Vector3(targetPosition.x, targetPosition.y, currentZ);
            }
            else
            {
                Vector2 startPosition = mainPosition;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    float t = elapsedTime / duration;
                    Vector2 LerpPosition = Vector2.Lerp(startPosition, targetPosition, t);
                    mainPosition = new Vector3(LerpPosition.x, LerpPosition.y, currentZ);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                mainPosition = new Vector3(targetPosition.x, targetPosition.y, currentZ);
            }
        }
        else
        {
            while (true)
            {
                targetPosition = trackingTarget.transform.position;
                if ((mainPosition - new Vector3(targetPosition.x, targetPosition.y, currentZ)).sqrMagnitude > Threshold * Threshold)
                {
                    mainPosition = Vector3.Lerp(mainPosition, new Vector3(targetPosition.x, targetPosition.y, currentZ), cameraTrackingSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }
    }
}