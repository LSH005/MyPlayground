using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance { get; private set; }
    public static float Threshold = 0.05f;
    public static float cameraTrackingSpeed = 8f;
    public static float toleranceY = 3.5f;
    public static float yTrackingDampening = 3f;
    public static bool normalizeRotation = true;

    private Vector3 mainPosition;
    private Vector2 shakePositionOffset = Vector2.zero;
    private Vector3 mainRotation;
    private Transform positionTrackingTarget;
    private Transform rotationTrackingTarget;
    private float currentZ;
    private float shakeRotationOffset = 0f;
    private bool canStopMovement = false;
    private bool canStopRotation = false;
    private bool isRotating = false;
    private Coroutine panCoroutine;
    private Coroutine zoomCoroutine;
    private Coroutine rotationCoroutine;
    private Coroutine positionShakingCoroutine;
    private Coroutine normalizeCoroutine;
    private Coroutine rotationShakingCoroutine;

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
        if (normalizeRotation && !isRotating)
        {
            mainRotation = NormalizeAngles(mainRotation);
        }
    }

    private void LateUpdate()
    {
        transform.position = mainPosition + new Vector3(shakePositionOffset.x, shakePositionOffset.y, 0f);
        transform.rotation = Quaternion.Euler(new Vector3(mainRotation.x, mainRotation.y, mainRotation.z + shakeRotationOffset));
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

    /// <summary>
    /// 인수 : 위치 - 기간
    /// </summary>
    public static void DollyTo(Vector2 targetPosition, float duration)
    {
        if (Instance.panCoroutine != null)
        {
            Instance.StopCoroutine(Instance.panCoroutine);
        }
        Instance.canStopMovement = true;
        Instance.panCoroutine = Instance.StartCoroutine(Instance.CameraMoveCoroutine(targetPosition,Vector3.zero, duration));
    }
    /// <summary>
    /// 인수 : 타겟 - 오프셋
    /// </summary>
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
    /// <summary>
    /// 인수 : Z좌표 - 기간
    /// </summary>
    public static void PositionZoom(float targetZ, float duration)
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
    /// <summary>
    /// 인수 : 각도 - 기간
    /// </summary>
    public static void RotateTo(Vector3 targetRotation, float duration)
    {
        if (Instance.rotationCoroutine != null)
        {
            Instance.StopCoroutine(Instance.rotationCoroutine);
        }
        Instance.canStopRotation = true;
        Instance.isRotating = true;
        Instance.rotationCoroutine = Instance.StartCoroutine(Instance.CameraRotationCoroutine(targetRotation, Vector3.zero, duration));
    }
    /// <summary>
    /// 인수 : 타겟 - 오프셋
    /// </summary>
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
                        float t = elapsedTime / duration;

                        float x = Mathf.LerpAngle(startRotation.x, targetRotation.x, t);
                        float y = Mathf.LerpAngle(startRotation.y, targetRotation.y, t);
                        float z = Mathf.LerpAngle(startRotation.z, targetRotation.z, t);

                        mainRotation = new Vector3(x, y, z);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    while (elapsedTime < duration)
                    {
                        mainRotation = Vector3.Lerp(startRotation, targetRotation, elapsedTime / duration);
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
                Vector3 targetDirection = rotationTrackingTarget.position - mainPosition;

                if (targetDirection == Vector3.zero)
                {
                    yield return null;
                    continue;
                }

                Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
                Vector3 finalTargetEulerAngles = desiredRotation.eulerAngles + offset;

                float x = Mathf.LerpAngle(mainRotation.x, finalTargetEulerAngles.x, cameraTrackingSpeed * Time.deltaTime);
                float y = Mathf.LerpAngle(mainRotation.y, finalTargetEulerAngles.y, cameraTrackingSpeed * Time.deltaTime);
                float z = Mathf.LerpAngle(mainRotation.z, finalTargetEulerAngles.z, cameraTrackingSpeed * Time.deltaTime);

                mainRotation = new Vector3(x, y, z);
                yield return null;
            }

        }    
    }

    /// <summary>
    /// 인수 : 최대 거리 - 주기 - 기간
    /// </summary>
    public static void PositionShaking(float intensity, float period, float duration)
    {
        if (Instance.positionShakingCoroutine != null)
        {
            Instance.StopCoroutine(Instance.positionShakingCoroutine);
        }

        Instance.positionShakingCoroutine = Instance.StartCoroutine(Instance.PositionShakingCoroutine(intensity, period, duration));
    }

    IEnumerator PositionShakingCoroutine(float intensity, float period, float duration)
    {
        float elapsedTime = 0f; // 총 경과 시간
        float periodTimer = 0f; // 주기 내 시간

        Vector2 startPoint = Vector2.zero;
        Vector2 targetPoint = Vector2.zero;

        while (elapsedTime < duration)
        {
            if (periodTimer >= period || elapsedTime == 0f)
            {
                periodTimer = 0f;
                startPoint = shakePositionOffset;

                float currentIntensity = intensity * (1 - (elapsedTime / duration));

                // Random.insideUnitCircle.normalized : 길이가 1인 랜덤 방향 벡터
                targetPoint = Random.insideUnitCircle.normalized * currentIntensity;
            }

            elapsedTime += Time.deltaTime;
            periodTimer += Time.deltaTime;

            shakePositionOffset = Vector2.Lerp(startPoint, targetPoint, periodTimer / period);

            yield return null;
        }

        shakePositionOffset = Vector2.zero;
        positionShakingCoroutine = null;
    }

    /// <summary>
    /// 인수 : 최대 각도 - 주기 - 기간
    /// </summary>
    public static void RotationShaking(float intensity, float period, float duration)
    {
        if (Instance.rotationShakingCoroutine != null)
        {
            Instance.StopCoroutine(Instance.rotationShakingCoroutine);
        }

        Instance.rotationShakingCoroutine = Instance.StartCoroutine(Instance.RotationShakingCoroutine(intensity, period, duration));
    }
    IEnumerator RotationShakingCoroutine(float intensity, float period, float duration)
    {
        float elapsedTime = 0f; // 총 경과 시간
        float periodTimer = 0f; // 주기 내 시간

        float startRotation = shakeRotationOffset;
        float targetRotation = 0f;
        int sign = 1;

        while (elapsedTime < duration)
        {
            if (periodTimer >= period || elapsedTime == 0f)
            {
                periodTimer = 0f;

                startRotation = shakeRotationOffset;
                targetRotation = intensity * (1 - (elapsedTime / duration));
                targetRotation *= sign;
                sign = -sign;
            }

            elapsedTime += Time.deltaTime;
            periodTimer += Time.deltaTime;

            shakeRotationOffset = Mathf.LerpAngle(startRotation, targetRotation, periodTimer / period);

            yield return null;
        }

        float lastRotation = shakeRotationOffset;
        float returnTimer = 0f;

        while (returnTimer < period)
        {
            returnTimer += Time.deltaTime;
            shakeRotationOffset = Mathf.LerpAngle(lastRotation, 0f, returnTimer / (period/2));

            yield return null;
        }

        shakeRotationOffset = 0f;
        rotationShakingCoroutine = null;
    }
}